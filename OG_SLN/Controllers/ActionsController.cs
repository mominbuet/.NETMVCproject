using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OG_SLN.Models;

namespace OG_SLN.Controllers
{
    public class ActionsController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /Actions/

        public ActionResult Index()
        {
            /*
            var gen_controller_action = db.GEN_CONTROLLER_ACTION;
            return View(gen_controller_action.ToList());
             */

            ControllerList list = new ControllerList();

            //ViewBag.ControllerList = list.GetControllerActions();
            //List<ClsController> clslist = list.GetControllerActions();

            List<GEN_CONTROLLER_ACTION> db_action_list = db.GEN_CONTROLLER_ACTION.OrderBy(a => a.ACTION_NAME).OrderBy(a => a.CONTROLLER_NAME).ToList();

            List<ControllerActionViewModel> action_list = list.GetControllerActions().OrderBy(a => a.actionName).OrderBy(a => a.controllerName).ToList();

            foreach (var code_item in action_list)
            {
                foreach (var db_item in db_action_list)
                {
                    if ((code_item.controllerName.Trim() == db_item.CONTROLLER_NAME.Trim())
                        && (code_item.actionName.Trim() == db_item.ACTION_NAME.Trim()))
                    {
                        code_item.IsActive = db_item.IS_ACTIVE.Value == 1 ? true : false;
                        code_item.IsAutoInclude = db_item.IS_AUTO_INCLUDE.Value == 1 ? true : false;
                        if (db_item.IS_PUBLIC.HasValue)
                        {
                            code_item.IsPublic = db_item.IS_PUBLIC.Value == 1 ? true : false;
                        }
                        else
                        {
                            code_item.IsPublic = false;
                        }
                        
                        if (db_item.IS_MENU.HasValue)
                        {
                            code_item.IsMenu = db_item.IS_MENU.Value == 1 ? true : false;
                        }
                        else
                        {
                            code_item.IsMenu = false;
                        }

                        code_item.menuName = db_item.MENU_NAME;
                    }
                }
            }

            return View(action_list);
        }

        [HttpPost]
        public ActionResult Index(ControllerActionViewModel[] controllers)
        {
            foreach (var cont in controllers)
            {

                GEN_CONTROLLER_ACTION record = db.GEN_CONTROLLER_ACTION.
                    Where(a => a.ACTION_NAME == cont.actionName && a.CONTROLLER_NAME == cont.controllerName).
                    FirstOrDefault();
                if (record == null)
                {
                    GEN_CONTROLLER_ACTION conac = new GEN_CONTROLLER_ACTION();

                    conac.ACTION_NAME = cont.actionName;
                    conac.CONTROLLER_NAME = cont.controllerName;
                    conac.IS_AUTO_INCLUDE = cont.IsAutoInclude == true ? 1 : 0;
                    conac.IS_ACTIVE = cont.IsActive == true ? 1 : 0;
                    conac.IS_PUBLIC = cont.IsPublic == true ? 1 : 0;
                    conac.IS_MENU = cont.IsMenu == true ? 1 : 0;
                    conac.MENU_NAME = cont.menuName;

                    db.GEN_CONTROLLER_ACTION_INSERT(conac.CONTROLLER_NAME, conac.ACTION_NAME, conac.IS_ACTIVE,
                        conac.IS_PUBLIC, conac.IS_AUTO_INCLUDE, conac.PARENT_ACTION_NO, conac.IS_MENU, 
                        conac.MENU_NAME, conac.PARENT_MENU_NO, conac.IS_SUB_MENU, conac.DETAILS, conac.SL_NUM);
                }
                else
                {
                    record.IS_AUTO_INCLUDE = cont.IsAutoInclude == true ? 1 : 0;
                    record.IS_ACTIVE = cont.IsActive == true ? 1 : 0;
                    record.IS_PUBLIC = cont.IsPublic == true ? 1 : 0;
                    record.IS_MENU = cont.IsMenu == true ? 1 : 0;
                    record.MENU_NAME = cont.menuName;

                    db.GEN_CONTROLLER_ACTION_UPDATE(record.ACTION_NO, record.CONTROLLER_NAME, record.ACTION_NAME,
                        record.IS_ACTIVE, record.IS_PUBLIC, record.IS_AUTO_INCLUDE, record.PARENT_ACTION_NO,
                        record.IS_MENU, record.MENU_NAME, record.PARENT_MENU_NO, record.IS_SUB_MENU, 
                        record.DETAILS, record.SL_NUM);
                }
            }
            //db.SaveChanges();

            ControllerList list = new ControllerList();
            List<ControllerActionViewModel> clist = list.GetControllerActions().OrderBy(a => a.actionName).OrderBy(a => a.controllerName).ToList();

            return View(clist);
        }

        //
        // GET: /Actions/Details/5

        public ActionResult Details(decimal id = 0)
        {
            GEN_CONTROLLER_ACTION gen_controller_action = db.GEN_CONTROLLER_ACTION.Single(g => g.ACTION_NO == id);
            if (gen_controller_action == null)
            {
                return HttpNotFound();
            }
            return View(gen_controller_action);
        }

        //
        // GET: /Actions/Create

        public ActionResult Create()
        {
            ViewBag.PARENT_ACTION_NO = new SelectList(db.GEN_CONTROLLER_ACTION, "ACTION_NO", "CONTROLLER_NAME");
            ViewBag.PARENT_MENU_NO = new SelectList(db.GEN_CONTROLLER_ACTION, "ACTION_NO", "CONTROLLER_NAME");
            return View();
        }

        //
        // POST: /Actions/Create

        [HttpPost]
        public ActionResult Create(GEN_CONTROLLER_ACTION gen_controller_action)
        {
            if (ModelState.IsValid)
            {
                db.GEN_CONTROLLER_ACTION.AddObject(gen_controller_action);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PARENT_ACTION_NO = new SelectList(db.GEN_CONTROLLER_ACTION, "ACTION_NO", "CONTROLLER_NAME", gen_controller_action.PARENT_ACTION_NO);
            ViewBag.PARENT_MENU_NO = new SelectList(db.GEN_CONTROLLER_ACTION, "ACTION_NO", "CONTROLLER_NAME", gen_controller_action.PARENT_MENU_NO);
            return View(gen_controller_action);
        }

        //
        // GET: /Actions/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            GEN_CONTROLLER_ACTION gen_controller_action = db.GEN_CONTROLLER_ACTION.Single(g => g.ACTION_NO == id);
            if (gen_controller_action == null)
            {
                return HttpNotFound();
            }
            ViewBag.PARENT_ACTION_NO = new SelectList(db.GEN_CONTROLLER_ACTION, "ACTION_NO", "CONTROLLER_NAME", gen_controller_action.PARENT_ACTION_NO);
            ViewBag.PARENT_MENU_NO = new SelectList(db.GEN_CONTROLLER_ACTION, "ACTION_NO", "CONTROLLER_NAME", gen_controller_action.PARENT_MENU_NO);
            return View(gen_controller_action);
        }

        //
        // POST: /Actions/Edit/5

        [HttpPost]
        public ActionResult Edit(GEN_CONTROLLER_ACTION gen_controller_action)
        {
            if (ModelState.IsValid)
            {
                db.GEN_CONTROLLER_ACTION.Attach(gen_controller_action);
                db.ObjectStateManager.ChangeObjectState(gen_controller_action, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PARENT_ACTION_NO = new SelectList(db.GEN_CONTROLLER_ACTION, "ACTION_NO", "CONTROLLER_NAME", gen_controller_action.PARENT_ACTION_NO);
            ViewBag.PARENT_MENU_NO = new SelectList(db.GEN_CONTROLLER_ACTION, "ACTION_NO", "CONTROLLER_NAME", gen_controller_action.PARENT_MENU_NO);
            return View(gen_controller_action);
        }

        //
        // GET: /Actions/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            GEN_CONTROLLER_ACTION gen_controller_action = db.GEN_CONTROLLER_ACTION.Single(g => g.ACTION_NO == id);
            if (gen_controller_action == null)
            {
                return HttpNotFound();
            }
            return View(gen_controller_action);
        }

        //
        // POST: /Actions/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            GEN_CONTROLLER_ACTION gen_controller_action = db.GEN_CONTROLLER_ACTION.Single(g => g.ACTION_NO == id);
            db.GEN_CONTROLLER_ACTION.DeleteObject(gen_controller_action);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}