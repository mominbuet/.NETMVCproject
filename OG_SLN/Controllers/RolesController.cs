using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class RolesController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /Roles/

        public ActionResult Index()
        {
            return View(db.SET_ROLE.ToList());
        }

        //
        // GET: /Roles/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_ROLE set_role = db.SET_ROLE.Single(s => s.ROLE_NO == id);
            if (set_role == null)
            {
                return HttpNotFound();
            }
            return View(set_role);
        }

        //
        // GET: /Roles/Create

        public ActionResult Create()
        {
            ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES, "USER_TYPE_NO", "USER_TYPE");
            return View();
        }

        //
        // POST: /Roles/Create

        [HttpPost]
        public ActionResult Create(SET_ROLE set_role)
        {
            if (ModelState.IsValid)
            {
                db.SET_ROLE.AddObject(set_role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES, "USER_TYPE_NO", "USER_TYPE", set_role.USER_TYPE_NO);

            return View(set_role);
        }

        //
        // GET: /Roles/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_ROLE set_role = db.SET_ROLE.Single(s => s.ROLE_NO == id);
            if (set_role == null)
            {
                return HttpNotFound();
            }
            return View(set_role);
        }

        //
        // POST: /Roles/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_ROLE set_role)
        {
            if (ModelState.IsValid)
            {
                db.SET_ROLE.Attach(set_role);
                db.ObjectStateManager.ChangeObjectState(set_role, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(set_role);
        }

        //
        // GET: /Roles/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_ROLE set_role = db.SET_ROLE.Single(s => s.ROLE_NO == id);
            if (set_role == null)
            {
                return HttpNotFound();
            }
            return View(set_role);
        }

        //
        // POST: /Roles/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_ROLE set_role = db.SET_ROLE.Single(s => s.ROLE_NO == id);
            db.SET_ROLE.DeleteObject(set_role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Permissions(long id = 0)
        {
            SET_ROLE role = db.SET_ROLE.Where(a => a.ROLE_NO == id).FirstOrDefault();
            ViewBag.ROLE_NAME = role.ROLE_NAME;

            List<GEN_CONTROLLER_ACTION> controllerList = db.GEN_CONTROLLER_ACTION
                .Where(a => a.IS_ACTIVE == 1 && a.IS_AUTO_INCLUDE == 0 && a.PARENT_ACTION_NO == null
                     && (a.IS_PUBLIC == null || a.IS_PUBLIC == 0))
                .OrderBy(a => a.ACTION_NAME).OrderBy(a => a.CONTROLLER_NAME)
                .ToList();

            List<SET_ROLE_ACTION> permit_list = db.SET_ROLE_ACTION.Where(a => a.ROLE_NO == id).ToList();


            foreach (var item in controllerList)
            {

                SET_ROLE_ACTION actionPerm = (from p in permit_list
                                             where p.ACTION_NO == item.ACTION_NO
                                             select p).FirstOrDefault() ;
                if (actionPerm == null)
                {
                    item.IS_ACTIVE = 0;
                }
                else
                {
                    item.IS_ACTIVE = actionPerm.IS_ACTIVE;
                }
            }

            ViewBag.controllerList = controllerList;
            ViewBag.userType = id;
            TempData["USER_ROLE"] = id;
            TempData.Keep();

            return View();
        }

        [HttpPost]
        public ActionResult Permissions(SET_ROLE_ACTION[] permissions)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            SET_ROLE_ACTION permission = permissions.FirstOrDefault();

            if (permission != null)
            {
                List<GEN_CONTROLLER_ACTION> allowedControllerList = db.GEN_CONTROLLER_ACTION
                    .Where(a => a.IS_ACTIVE == 1 && (a.IS_AUTO_INCLUDE == 1 || a.IS_PUBLIC == 1)).ToList();

                foreach (var allowed in allowedControllerList)
                {
                    SET_ROLE_ACTION allowedRecord = db.SET_ROLE_ACTION.
                        Where(a => a.ACTION_NO == allowed.ACTION_NO && a.ROLE_NO == permission.ROLE_NO).
                        FirstOrDefault();

                    if (allowedRecord == null) {
                        db.SET_ROLE_ACTION_INSERT(USER_NO, LOGON_NO, permission.ROLE_NO,
                            allowed.ACTION_NO, 1, "Automatically Allowed");
                    }
                }
            }

            foreach (var perm in permissions)
            {

                SET_ROLE_ACTION record = db.SET_ROLE_ACTION.
                    Where(a => a.ACTION_NO == perm.ACTION_NO && a.ROLE_NO == perm.ROLE_NO).
                    FirstOrDefault();

                if (record == null)
                {
                    SET_ROLE_ACTION rolac = new SET_ROLE_ACTION();

                    rolac.ACTION_NO = perm.ACTION_NO;
                    rolac.ROLE_NO = perm.ROLE_NO;
                    rolac.IS_ACTIVE = perm.IS_ACTIVE;

                    //db.GEN_USERS_PERMISSIONS.Add(gen_user_permission);
                    db.SET_ROLE_ACTION_INSERT(USER_NO, LOGON_NO, rolac.ROLE_NO, rolac.ACTION_NO,
                        rolac.IS_ACTIVE, rolac.DETAILS);
                }
                else
                {
                    record.IS_ACTIVE = perm.IS_ACTIVE;
                    //db.Entry(record).State = EntityState.Modified;
                    db.SET_ROLE_ACTION_UPDATE(record.ROLE_ACTION_NO, USER_NO, LOGON_NO, record.ROLE_NO,
                        record.ACTION_NO, record.IS_ACTIVE, record.DETAILS);
                }

                List<GEN_CONTROLLER_ACTION> childActions = db.GEN_CONTROLLER_ACTION
                    .Where(a => a.PARENT_ACTION_NO == perm.ACTION_NO).ToList();

                foreach (var child in childActions) {
                    SET_ROLE_ACTION childRecord = db.SET_ROLE_ACTION.
                        Where(a => a.ACTION_NO == child.ACTION_NO && a.ROLE_NO == perm.ROLE_NO).
                        FirstOrDefault();

                    if (childRecord == null)
                    {
                        db.SET_ROLE_ACTION_INSERT(USER_NO, LOGON_NO, perm.ROLE_NO, child.ACTION_NO,
                            perm.IS_ACTIVE, "Child Perms");
                    }
                    else
                    {
                        db.SET_ROLE_ACTION_UPDATE(childRecord.ROLE_ACTION_NO, USER_NO, LOGON_NO, perm.ROLE_NO,
                            childRecord.ACTION_NO, perm.IS_ACTIVE, "Child Perms");
                    }
                }
            }

            ViewBag.userRole = TempData.Peek("USER_ROLE");
            decimal userRole = ViewBag.userRole;

            SET_ROLE role = db.SET_ROLE.Where(a => a.ROLE_NO == userRole).FirstOrDefault();
            ViewBag.ROLE_NAME = role.ROLE_NAME;

            //db.SaveChanges();

            List<GEN_CONTROLLER_ACTION> controllerList = db.GEN_CONTROLLER_ACTION
                .Where(a => a.IS_ACTIVE == 1 && a.IS_AUTO_INCLUDE == 0 && a.PARENT_ACTION_NO == null
                    && (a.IS_PUBLIC == null || a.IS_PUBLIC == 0))
                .OrderBy(a => a.ACTION_NAME).OrderBy(a => a.CONTROLLER_NAME)
                .ToList();

            ViewBag.controllerList = controllerList;


            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}