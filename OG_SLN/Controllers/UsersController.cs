using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using OG_SLN.Models;
using PagedList;

namespace OG_SLN.Controllers
{
    public class UsersController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /Users/

        public ActionResult Index()
        {
            //var sec_users = db.SEC_USERS.Include("GEN_USER_TYPES").Include("SET_DEPARTMENT").Include("SET_DESIGNATION");
            //return View(sec_users.ToList());
            ObjectResult<SEC_USERS_GET_Result> sec_users = db.SEC_USERS_GET(null, null, null, null, null, null,
                   null, null, null, null, null, null, null, null, null, null, 1, 10);

            return View(sec_users.ToList());
        }

        //
        // GET: /Users/Details/5

        public ActionResult Details(decimal id = 0, string type = null)
        {
            SEC_USERS sec_users = db.SEC_USERS.Single(s => s.USER_NO == id);
            if (sec_users == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_Type = type;
            return View(sec_users);
        }

        //
        // GET: /Users/Create

        public ActionResult Create(string type = null)
        {
            if (type != null)
            {
                if (type == "Admin")
                {
                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Administrator
                        || u.USER_TYPE_NO == (decimal)EUserTypes.SuperAdministrator), "USER_TYPE_NO", "USER_TYPE");
                }
                else if (type == "Special")
                {
                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.SeniorManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.DivisionalManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.MarketingHead), "USER_TYPE_NO", "USER_TYPE");
                }
                else if (type == "Zonal")
                {
                    ViewBag.USER_PARENT_NO = new SelectList(db.SEC_USERS
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.AreaManager), "USER_NO", "USER_FULL_NAME");

                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.AreaManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager), "USER_TYPE_NO", "USER_TYPE");
                }
                else if (type == "Agent")
                {
                    ViewBag.USER_PARENT_NO = new SelectList(db.SEC_USERS
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager), "USER_NO", "USER_FULL_NAME");

                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Agent), "USER_TYPE_NO", "USER_TYPE");
                }
                else if (type == "GeneralUser")
                {
                    ViewBag.USER_PARENT_NO = new SelectList(db.SEC_USERS
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Administrator
                        || u.USER_TYPE_NO == (decimal)EUserTypes.SuperAdministrator), "USER_NO", "USER_FULL_NAME");

                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.GeneralUser), "USER_TYPE_NO", "USER_TYPE");
                }
                ViewBag.User_Type = type;
            }

            ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0),
                "ZILLA_NO", "ZILLA_NAME");
            ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0),
                "THANA_NO", "THANA_NAME");

            ViewBag.COMP_NO = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME");
            ViewBag.DEPT_NO = new SelectList(db.SET_DEPARTMENT, "DEPT_NO", "DEPT_NAME");
            ViewBag.DESIG_NO = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME");
            return View();
        }

        //
        // POST: /Users/Create

        [HttpPost]
        public ActionResult Create(SEC_USERS sec_users)
        {
            if (ModelState.IsValid)
            {
                //db.SEC_USERS.AddObject(sec_users);
                decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
                decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

                sec_users.USER_NO = 
                db.SEC_USERS_INSERT(sec_users.USER_MOBILE, sec_users.USER_ADDR, sec_users.USER_EMAIL,
                    sec_users.USER_NAME, sec_users.USER_PWD, sec_users.USER_PARENT_NO, sec_users.IS_ACTIVE,
                    sec_users.ACTIVE_FROM, sec_users.ACTIVE_TO, USER_NO, LOGON_NO,
                    sec_users.USER_TYPE_NO, sec_users.DEPT_NO, sec_users.DESIG_NO, sec_users.HR_EMP_ID,
                    sec_users.USER_FULL_NAME, sec_users.USER_NICK_NAME, sec_users.USER_CONTACT,
                    sec_users.COMP_NO, sec_users.THANA_NO).First().Value;
                //db.SaveChanges();
                if (sec_users.User_ViewType != null)
                {
                    return RedirectToAction(sec_users.User_ViewType);
                }
                return RedirectToAction("Index");
            }

            string type = sec_users.User_ViewType;

            if (type != null)
            {
                if (type == "Admin")
                {
                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Administrator
                        || u.USER_TYPE_NO == (decimal)EUserTypes.SuperAdministrator), "USER_TYPE_NO", "USER_TYPE");
                }
                else if (type == "Special")
                {
                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.SeniorManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.DivisionalManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.MarketingHead), "USER_TYPE_NO", "USER_TYPE");
                }
                else if (type == "Zonal")
                {
                    ViewBag.USER_PARENT_NO = new SelectList(db.SEC_USERS
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.AreaManager), "USER_NO", "USER_FULL_NAME");

                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.AreaManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager), "USER_TYPE_NO", "USER_TYPE");
                }
                else if (type == "Agent")
                {
                    ViewBag.USER_PARENT_NO = new SelectList(db.SEC_USERS
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager), "USER_NO", "USER_FULL_NAME");

                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Agent), "USER_TYPE_NO", "USER_TYPE");
                }
                else if (type == "GeneralUser")
                {
                    ViewBag.USER_PARENT_NO = new SelectList(db.SEC_USERS
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Administrator
                        || u.USER_TYPE_NO == (decimal)EUserTypes.SuperAdministrator), "USER_NO", "USER_FULL_NAME");

                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.GeneralUser), "USER_TYPE_NO", "USER_TYPE");
                }
                ViewBag.User_Type = type;
            }

            //ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES, "USER_TYPE_NO", "USER_TYPE", sec_users.USER_TYPE_NO);
            ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME", sec_users.DIVISION_NO);
            if (sec_users.DIVISION_NO.HasValue)
            {
                ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.DIVISION_NO == sec_users.DIVISION_NO),
                "ZILLA_NO", "ZILLA_NAME", sec_users.ZILLA_NO);
            }
            else
            {
                ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0),
                "ZILLA_NO", "ZILLA_NAME");
            }

            if (sec_users.ZILLA_NO.HasValue)
            {
                ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.ZILLA_NO == sec_users.ZILLA_NO),
                "THANA_NO", "THANA_NAME", sec_users.THANA_NO);
            }
            else
            {
                ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0),
                "THANA_NO", "THANA_NAME");
            }

            ViewBag.COMP_NO = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME", sec_users.COMP_NO);
            ViewBag.DEPT_NO = new SelectList(db.SET_DEPARTMENT, "DEPT_NO", "DEPT_NAME", sec_users.DEPT_NO);
            ViewBag.DESIG_NO = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME", sec_users.DESIG_NO);
            return View(sec_users);
        }

        //
        // GET: /Users/Edit/5

        public ActionResult Edit(decimal id = 0, string type = null)
        {
            SEC_USERS sec_users = db.SEC_USERS.Single(s => s.USER_NO == id);
            if (sec_users == null)
            {
                return HttpNotFound();
            }

            if (type != null)
            {
                if (type == "Admin")
                {
                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Administrator
                        || u.USER_TYPE_NO == (decimal)EUserTypes.SuperAdministrator), "USER_TYPE_NO", "USER_TYPE", 
                        sec_users.USER_TYPE_NO);
                }
                else if (type == "Special")
                {
                    if (sec_users.USER_PARENT_NO.HasValue)
                    {
                        ViewBag.User_Parent_Name = (from u in db.SEC_USERS
                                                    where u.USER_NO == sec_users.USER_PARENT_NO
                                                    select new { u.USER_FULL_NAME }).FirstOrDefault().USER_FULL_NAME;
                    }
                    else
                    {
                        ViewBag.User_Parent_Name = String.Empty;
                    }


                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.SeniorManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.DivisionalManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.MarketingHead), "USER_TYPE_NO", "USER_TYPE",
                        sec_users.USER_TYPE_NO);
                }
                else if (type == "Zonal")
                {
                    /*
                    ViewBag.USER_PARENT_NO = new SelectList(db.SEC_USERS
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.AreaManager), "USER_NO", "USER_FULL_NAME",
                        sec_users.USER_PARENT_NO);
                     * */
                    if (sec_users.USER_PARENT_NO.HasValue)
                    {
                        ViewBag.User_Parent_Name = (from u in db.SEC_USERS
                                                    where u.USER_NO == sec_users.USER_PARENT_NO
                                                    select new { u.USER_FULL_NAME }).FirstOrDefault().USER_FULL_NAME;
                    }
                    else
                    {
                        ViewBag.User_Parent_Name = String.Empty;
                    }

                    
                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.AreaManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager), "USER_TYPE_NO", "USER_TYPE", 
                        sec_users.USER_TYPE_NO);
                }
                else if (type == "Agent")
                {
                    /*
                    ViewBag.USER_PARENT_NO = new SelectList(db.SEC_USERS
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager), "USER_NO", "USER_FULL_NAME",
                        sec_users.USER_PARENT_NO);
                     * */

                    if (sec_users.USER_PARENT_NO.HasValue)
                    {
                        ViewBag.User_Parent_Name = (from u in db.SEC_USERS
                                                    where u.USER_NO == sec_users.USER_PARENT_NO
                                                    select new { u.USER_FULL_NAME }).FirstOrDefault().USER_FULL_NAME;
                    }
                    else
                    {
                        ViewBag.User_Parent_Name = String.Empty;
                    }

                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Agent), "USER_TYPE_NO", "USER_TYPE", 
                        sec_users.USER_TYPE_NO);
                }
                ViewBag.User_Type = type;
            }

            //ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES, "USER_TYPE_NO", "USER_TYPE", sec_users.USER_TYPE_NO);

            SET_THANA thana = null;
            SET_ZILLA zilla = null;

            if (sec_users.THANA_NO.HasValue)
            {
                thana = db.SET_THANA.Where(t => t.THANA_NO == sec_users.THANA_NO).FirstOrDefault();
                zilla = db.SET_ZILLA.Where(z => z.ZILLA_NO == thana.ZILLA_NO).FirstOrDefault();
            }

            if (thana != null)
            {
                ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.ZILLA_NO == thana.ZILLA_NO),
                "THANA_NO", "THANA_NAME", sec_users.THANA_NO);
            }
            else
            {
                ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0),
                "THANA_NO", "THANA_NAME");
            }

            if (zilla != null)
            {
                ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.DIVISION_NO == zilla.DIVISION_NO),
                "ZILLA_NO", "ZILLA_NAME", thana.ZILLA_NO);
            }
            else
            {
                ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0),
                "ZILLA_NO", "ZILLA_NAME");
            }

            if(zilla != null)
                ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME", zilla.DIVISION_NO);
            else
                ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            
            ViewBag.COMP_NO = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME", sec_users.COMP_NO);
            ViewBag.DEPT_NO = new SelectList(db.SET_DEPARTMENT
                .Where(c => c.COMP_NO == sec_users.COMP_NO), "DEPT_NO", "DEPT_NAME", sec_users.DEPT_NO);
            ViewBag.DESIG_NO = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME", sec_users.DESIG_NO);
            ViewBag.User_Type = type;
            return View(sec_users);
        }

        //
        // POST: /Users/Edit/5

        [HttpPost]
        public ActionResult Edit(SEC_USERS sec_users)
        {
            string type = sec_users.User_ViewType;

            ModelState["USER_PWD"].Errors.Clear();
            ModelState["RETYPE_PASSWORD"].Errors.Clear();

            if (ModelState.IsValid)
            {
                decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
                decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

                //db.SEC_USERS.Attach(sec_users);
                //db.ObjectStateManager.ChangeObjectState(sec_users, EntityState.Modified);
                //db.SaveChanges();

                db.SEC_USERS_UPDATE(sec_users.USER_MOBILE, sec_users.USER_ADDR, sec_users.USER_EMAIL,
                    sec_users.USER_NAME, sec_users.USER_PWD, sec_users.USER_PARENT_NO, sec_users.IS_ACTIVE,
                    sec_users.ACTIVE_FROM, sec_users.ACTIVE_TO, sec_users.USER_NO, USER_NO, LOGON_NO,
                    sec_users.USER_TYPE_NO, sec_users.DEPT_NO, sec_users.DESIG_NO, sec_users.HR_EMP_ID,
                    sec_users.USER_FULL_NAME, sec_users.USER_NICK_NAME, sec_users.USER_CONTACT, 
                    sec_users.COMP_NO, sec_users.THANA_NO);

                return RedirectToAction(type);
            }
            
            if (type != null)
            {
                if (type == "Admin")
                {
                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Administrator
                        || u.USER_TYPE_NO == (decimal)EUserTypes.SuperAdministrator), "USER_TYPE_NO", "USER_TYPE",
                        sec_users.USER_TYPE_NO);
                }
                else if (type == "Special")
                {
                    if (sec_users.USER_PARENT_NO.HasValue)
                    {
                        ViewBag.User_Parent_Name = (from u in db.SEC_USERS
                                                    where u.USER_NO == sec_users.USER_PARENT_NO
                                                    select new { u.USER_FULL_NAME }).FirstOrDefault().USER_FULL_NAME;
                    }
                    else
                    {
                        ViewBag.User_Parent_Name = String.Empty;
                    }


                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.SeniorManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.DivisionalManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.MarketingHead), "USER_TYPE_NO", "USER_TYPE",
                        sec_users.USER_TYPE_NO);
                }
                else if (type == "Zonal")
                {
                    /*
                    ViewBag.USER_PARENT_NO = new SelectList(db.SEC_USERS
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.AreaManager), "USER_NO", "USER_FULL_NAME",
                        sec_users.USER_PARENT_NO);
                    */
                    if (sec_users.USER_PARENT_NO.HasValue)
                    {
                        ViewBag.User_Parent_Name = (from u in db.SEC_USERS
                                                    where u.USER_NO == sec_users.USER_PARENT_NO
                                                    select new { u.USER_FULL_NAME }).FirstOrDefault().USER_FULL_NAME;
                    }
                    else
                    {
                        ViewBag.User_Parent_Name = String.Empty;
                    }

                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.AreaManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager), "USER_TYPE_NO", "USER_TYPE",
                        sec_users.USER_TYPE_NO);
                }
                else if (type == "Agent")
                {
                    /*
                    ViewBag.USER_PARENT_NO = new SelectList(db.SEC_USERS
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager), "USER_NO", "USER_FULL_NAME",
                        sec_users.USER_PARENT_NO);
                    */

                    if (sec_users.USER_PARENT_NO.HasValue)
                    {
                        ViewBag.User_Parent_Name = (from u in db.SEC_USERS
                                                    where u.USER_NO == sec_users.USER_PARENT_NO
                                                    select new { u.USER_FULL_NAME }).FirstOrDefault().USER_FULL_NAME;
                    }
                    else
                    {
                        ViewBag.User_Parent_Name = String.Empty;
                    }

                    ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES
                        .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Agent), "USER_TYPE_NO", "USER_TYPE",
                        sec_users.USER_TYPE_NO);
                }
                ViewBag.User_Type = type;
            }
            //ViewBag.USER_TYPE_NO = new SelectList(db.GEN_USER_TYPES, "USER_TYPE_NO", "USER_TYPE", sec_users.USER_TYPE_NO);

            ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME", sec_users.DIVISION_NO);
            if (sec_users.DIVISION_NO.HasValue)
            {
                ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.DIVISION_NO == sec_users.DIVISION_NO),
                "ZILLA_NO", "ZILLA_NAME", sec_users.ZILLA_NO);
            }
            else
            {
                ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0),
                "ZILLA_NO", "ZILLA_NAME");
            }

            if (sec_users.ZILLA_NO.HasValue)
            {
                ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.ZILLA_NO == sec_users.ZILLA_NO),
                "THANA_NO", "THANA_NAME", sec_users.THANA_NO);
            }
            else
            {
                ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0),
                "THANA_NO", "THANA_NAME");
            }

            ViewBag.COMP_NO = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME", sec_users.COMP_NO);
            ViewBag.DEPT_NO = new SelectList(db.SET_DEPARTMENT
                .Where(c => c.COMP_NO == sec_users.COMP_NO), "DEPT_NO", "DEPT_NAME", sec_users.DEPT_NO);
            ViewBag.DESIG_NO = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME", sec_users.DESIG_NO);

            return View(sec_users);
        }

        //
        // GET: /Users/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SEC_USERS sec_users = db.SEC_USERS.Single(s => s.USER_NO == id);
            if (sec_users == null)
            {
                return HttpNotFound();
            }
            return View(sec_users);
        }

        //
        // POST: /Users/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            //SEC_USERS sec_users = db.SEC_USERS.Single(s => s.USER_NO == id);
            //db.SEC_USERS.DeleteObject(sec_users);
            //db.SaveChanges();
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            db.SEC_USERS_DELETE(id, USER_NO, LOGON_NO);
            return RedirectToAction("Index");
        }

        public ActionResult ChangePassword(decimal id = 0, string type = null)
        {
            SEC_USERS sec_user = db.SEC_USERS.Single(u => u.USER_NO == id);
            ViewBag.User_Type = type;

            return View(sec_user);
        }

        [HttpPost]
        public ActionResult ChangePassword(SEC_USERS sec_users)
        {
            string type = sec_users.User_ViewType;

            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            if (sec_users.USER_PWD == sec_users.RETYPE_PASSWORD)
            {
                db.SEC_USERS_CHANGE_PASSWORD(sec_users.USER_NO, sec_users.USER_PWD, USER_NO, LOGON_NO);
                return RedirectToAction(type);
            }
            return View(sec_users);
        }
        
        public ActionResult Admin(string sortOrder, string CurrentSort, int? page)
        {

            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            ViewBag.CurrentSort = sortOrder;

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "USER_NO" : sortOrder;

            IPagedList<SEC_USERS> sec_users = null;
            AllUserSearchViewModel searchModel = null;

            var users = db.SEC_USERS.AsQueryable();

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                searchModel = new AllUserSearchViewModel();

                if (!string.IsNullOrEmpty(Request.QueryString["Search_User_Type"]))
                {
                    searchModel.Search_User_Type = decimal.Parse(Request.QueryString["Search_User_Type"]);
                    users = users.Where(u => u.USER_TYPE_NO == searchModel.Search_User_Type);
                }
                else 
                {
                    users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.SuperAdministrator
                        || u.USER_TYPE_NO == (decimal)EUserTypes.Administrator);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Company"]))
                {
                    searchModel.Search_Company = decimal.Parse(Request.QueryString["Search_Company"]);
                    users = users.Where(u => u.COMP_NO == searchModel.Search_Company);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Department"]))
                {
                    searchModel.Search_Department = decimal.Parse(Request.QueryString["Search_Department"]);
                    users = users.Where(u => u.DEPT_NO == searchModel.Search_Department);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Designation"]))
                {
                    searchModel.Search_Designation = decimal.Parse(Request.QueryString["Search_Designation"]);
                    users = users.Where(u => u.DESIG_NO == searchModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Hrid"]))
                {
                    searchModel.Search_Hrid = Request.QueryString["Search_Hrid"];
                    users = users.Where(u => u.HR_EMP_ID == searchModel.Search_Hrid);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Fullname"]))
                {
                    searchModel.Search_Fullname = Request.QueryString["Search_Fullname"];
                    users = users.Where(u => u.USER_FULL_NAME.ToUpper().Contains(searchModel.Search_Fullname.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Mobile"]))
                {
                    searchModel.Search_Mobile = Request.QueryString["Search_Mobile"];
                    users = users.Where(u => u.USER_MOBILE.ToUpper().Contains(searchModel.Search_Mobile.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Email"]))
                {
                    searchModel.Search_Email = Request.QueryString["Search_Email"];
                    users = users.Where(u => u.USER_EMAIL.ToUpper().Contains(searchModel.Search_Email.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Username"]))
                {
                    searchModel.Search_Username = Request.QueryString["Search_Username"];
                    users = users.Where(u => u.USER_NAME.ToUpper().Contains(searchModel.Search_Username.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Active"])
                    && Request.QueryString["Search_Active"] != "null")
                {
                    searchModel.Search_Active = bool.Parse(Request.QueryString["Search_Active"]);
                    decimal active = searchModel.Search_Active == true ? 1 : 0;
                    users = users.Where(u => u.IS_ACTIVE == active);
                }

                Session["User_Search_Model"] = searchModel;
            }

            if (!Request.QueryString.HasKeys() && !page.HasValue)
            {
                Session["User_Search_Model"] = null;
            }

            if (Session["User_Search_Model"] == null)
            {
                users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.SuperAdministrator
                    || u.USER_TYPE_NO == (decimal)EUserTypes.Administrator);
            }
            else 
            {
                searchModel = (AllUserSearchViewModel) Session["User_Search_Model"];
                if (searchModel.Search_User_Type.HasValue)
                {
                    users = users.Where(u => u.USER_TYPE_NO == searchModel.Search_User_Type);
                }
                else
                {
                    users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.SuperAdministrator
                        || u.USER_TYPE_NO == (decimal)EUserTypes.Administrator);
                }
                if (searchModel.Search_Company.HasValue)
                {
                    users = users.Where(u => u.COMP_NO == searchModel.Search_Company);
                }
                if (searchModel.Search_Department.HasValue)
                {
                    users = users.Where(u => u.DEPT_NO == searchModel.Search_Department);
                }
                if (searchModel.Search_Designation.HasValue)
                {
                    users = users.Where(u => u.DESIG_NO == searchModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Hrid))
                {
                    users = users.Where(u => u.HR_EMP_ID == searchModel.Search_Hrid);
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Fullname))
                {
                    users = users.Where(u => u.USER_FULL_NAME.ToUpper().Contains(searchModel.Search_Fullname.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Mobile))
                {
                    users = users.Where(u => u.USER_MOBILE.ToUpper().Contains(searchModel.Search_Mobile.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Email))
                {
                    users = users.Where(u => u.USER_EMAIL.ToUpper().Contains(searchModel.Search_Email.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Username))
                {
                    users = users.Where(u => u.USER_NAME.ToUpper().Contains(searchModel.Search_Username.ToUpper()));
                }
                if (searchModel.Search_Active.HasValue)
                {
                    decimal active = searchModel.Search_Active == true ? 1 : 0;
                    users = users.Where(u => u.IS_ACTIVE == active);
                }
            }

            sec_users = users.OrderBy(m => m.INSERT_TIME)
                            .ToPagedList(pageIndex, pageSize);
            /*
            switch (sortOrder)
            {
                case "USER_MOBILE":
                    if (sortOrder.Equals(CurrentSort))
                        sec_users = users.OrderByDescending(m => m.USER_MOBILE)
                            .ToPagedList(pageIndex, pageSize);
                    else
                        sec_users = users.OrderBy(m => m.USER_MOBILE)
                            .ToPagedList(pageIndex, pageSize);
                    break;
                case "USER_FULL_NAME":
                    if (sortOrder.Equals(CurrentSort))
                        sec_users = users.OrderByDescending(m => m.USER_FULL_NAME)
                            .ToPagedList(pageIndex, pageSize);
                    else
                        sec_users = users.OrderBy(m => m.USER_FULL_NAME)
                            .ToPagedList(pageIndex, pageSize);
                    break;

                case "Default":
                    sec_users = users.OrderBy(m => m.USER_MOBILE)
                        .ToPagedList(pageIndex, pageSize);
                    break;
            }
            */
            ViewBag.Search_User_Model = (AllUserSearchViewModel)Session["User_Search_Model"];

            if (searchModel != null && searchModel.Search_User_Type.HasValue)
            {
                ViewBag.Search_User_Type = new SelectList(db.GEN_USER_TYPES
                        .Where(t => t.USER_TYPE_NO == (decimal)EUserTypes.SuperAdministrator
                        || t.USER_TYPE_NO == (decimal)EUserTypes.Administrator), "USER_TYPE_NO", "USER_TYPE", 
                        searchModel.Search_User_Type);
            }
            else
            {
                ViewBag.Search_User_Type = new SelectList(db.GEN_USER_TYPES
                        .Where(t => t.USER_TYPE_NO == (decimal)EUserTypes.SuperAdministrator
                        || t.USER_TYPE_NO == (decimal)EUserTypes.Administrator), "USER_TYPE_NO", "USER_TYPE");
            }

            if (searchModel != null && searchModel.Search_Company.HasValue)
            {
                ViewBag.Search_Company = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME", 
                    searchModel.Search_Company);
            }
            else
            {
                ViewBag.Search_Company = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME");
            }

            if (searchModel != null && searchModel.Search_Department.HasValue)
            {
                if (searchModel.Search_Company.HasValue)
                {
                    ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                        .Where(c => c.COMP_NO == searchModel.Search_Company), "DEPT_NO", "DEPT_NAME", 
                        searchModel.Search_Department);
                }
                else
                {
                    ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                    .Where(c => c.COMP_NO == 1), "DEPT_NO", "DEPT_NAME", searchModel.Search_Department);
                }
            }
            else
            {
                ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                    .Where(c => c.COMP_NO == 1), "DEPT_NO", "DEPT_NAME");
            }

            if (searchModel != null && searchModel.Search_Designation.HasValue)
            {
                ViewBag.Search_Designation = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME",
                    searchModel.Search_Designation);
            }
            else
            {
                ViewBag.Search_Designation = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME");
            }

            return View(sec_users);
        }

        public ActionResult Special(string sortOrder, string CurrentSort, int? page)
            // For Senior Manager, Divisional Manager, Marketing Head
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            ViewBag.CurrentSort = sortOrder;

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "USER_NO" : sortOrder;

            IPagedList<SEC_USERS> sec_users = null;
            AllUserSearchViewModel searchModel = null;

            var users = db.SEC_USERS.AsQueryable();

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                searchModel = new AllUserSearchViewModel();

                if (!string.IsNullOrEmpty(Request.QueryString["Search_User_Type"]))
                {
                    searchModel.Search_User_Type = decimal.Parse(Request.QueryString["Search_User_Type"]);
                    users = users.Where(u => u.USER_TYPE_NO == searchModel.Search_User_Type);
                }
                else
                {
                    users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.SeniorManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.DivisionalManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.MarketingHead);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Company"]))
                {
                    searchModel.Search_Company = decimal.Parse(Request.QueryString["Search_Company"]);
                    users = users.Where(u => u.COMP_NO == searchModel.Search_Company);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Department"]))
                {
                    searchModel.Search_Department = decimal.Parse(Request.QueryString["Search_Department"]);
                    users = users.Where(u => u.DEPT_NO == searchModel.Search_Department);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Designation"]))
                {
                    searchModel.Search_Designation = decimal.Parse(Request.QueryString["Search_Designation"]);
                    users = users.Where(u => u.DESIG_NO == searchModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Hrid"]))
                {
                    searchModel.Search_Hrid = Request.QueryString["Search_Hrid"];
                    users = users.Where(u => u.HR_EMP_ID == searchModel.Search_Hrid);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Fullname"]))
                {
                    searchModel.Search_Fullname = Request.QueryString["Search_Fullname"];
                    users = users.Where(u => u.USER_FULL_NAME.ToUpper().Contains(searchModel.Search_Fullname.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Mobile"]))
                {
                    searchModel.Search_Mobile = Request.QueryString["Search_Mobile"];
                    users = users.Where(u => u.USER_MOBILE.ToUpper().Contains(searchModel.Search_Mobile.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Email"]))
                {
                    searchModel.Search_Email = Request.QueryString["Search_Email"];
                    users = users.Where(u => u.USER_EMAIL.ToUpper().Contains(searchModel.Search_Email.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Username"]))
                {
                    searchModel.Search_Username = Request.QueryString["Search_Username"];
                    users = users.Where(u => u.USER_NAME.ToUpper().Contains(searchModel.Search_Username.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Active"])
                    && Request.QueryString["Search_Active"] != "null")
                {
                    searchModel.Search_Active = bool.Parse(Request.QueryString["Search_Active"]);
                    decimal active = searchModel.Search_Active == true ? 1 : 0;
                    users = users.Where(u => u.IS_ACTIVE == active);
                }

                Session["User_Search_Model"] = searchModel;
            }

            if (!Request.QueryString.HasKeys() && !page.HasValue)
            {
                Session["User_Search_Model"] = null;
            }

            if (Session["User_Search_Model"] == null)
            {
                users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.SeniorManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.DivisionalManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.MarketingHead);
            }
            else
            {
                searchModel = (AllUserSearchViewModel)Session["User_Search_Model"];
                if (searchModel.Search_User_Type.HasValue)
                {
                    users = users.Where(u => u.USER_TYPE_NO == searchModel.Search_User_Type);
                }
                else
                {
                    users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.SeniorManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.DivisionalManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.MarketingHead);
                }
                if (searchModel.Search_Company.HasValue)
                {
                    users = users.Where(u => u.COMP_NO == searchModel.Search_Company);
                }
                if (searchModel.Search_Department.HasValue)
                {
                    users = users.Where(u => u.DEPT_NO == searchModel.Search_Department);
                }
                if (searchModel.Search_Designation.HasValue)
                {
                    users = users.Where(u => u.DESIG_NO == searchModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Hrid))
                {
                    users = users.Where(u => u.HR_EMP_ID == searchModel.Search_Hrid);
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Fullname))
                {
                    users = users.Where(u => u.USER_FULL_NAME.ToUpper().Contains(searchModel.Search_Fullname.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Mobile))
                {
                    users = users.Where(u => u.USER_MOBILE.ToUpper().Contains(searchModel.Search_Mobile.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Email))
                {
                    users = users.Where(u => u.USER_EMAIL.ToUpper().Contains(searchModel.Search_Email.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Username))
                {
                    users = users.Where(u => u.USER_NAME.ToUpper().Contains(searchModel.Search_Username.ToUpper()));
                }
                if (searchModel.Search_Active.HasValue)
                {
                    decimal active = searchModel.Search_Active == true ? 1 : 0;
                    users = users.Where(u => u.IS_ACTIVE == active);
                }
            }

            sec_users = users.OrderBy(m => m.INSERT_TIME)
                            .ToPagedList(pageIndex, pageSize);

            ViewBag.Search_User_Model = (AllUserSearchViewModel)Session["User_Search_Model"];

            if (searchModel != null && searchModel.Search_User_Type.HasValue)
            {
                ViewBag.Search_User_Type = new SelectList(db.GEN_USER_TYPES
                        .Where(t => t.USER_TYPE_NO == (decimal)EUserTypes.SeniorManager
                        || t.USER_TYPE_NO == (decimal)EUserTypes.DivisionalManager
                        || t.USER_TYPE_NO == (decimal)EUserTypes.MarketingHead), "USER_TYPE_NO", "USER_TYPE",
                        searchModel.Search_User_Type);
            }
            else
            {
                ViewBag.Search_User_Type = new SelectList(db.GEN_USER_TYPES
                        .Where(t => t.USER_TYPE_NO == (decimal)EUserTypes.SeniorManager
                        || t.USER_TYPE_NO == (decimal)EUserTypes.DivisionalManager
                        || t.USER_TYPE_NO == (decimal)EUserTypes.MarketingHead), "USER_TYPE_NO", "USER_TYPE");
            }

            if (searchModel != null && searchModel.Search_Company.HasValue)
            {
                ViewBag.Search_Company = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME",
                    searchModel.Search_Company);
            }
            else
            {
                ViewBag.Search_Company = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME");
            }

            if (searchModel != null && searchModel.Search_Department.HasValue)
            {
                if (searchModel.Search_Company.HasValue)
                {
                    ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                        .Where(c => c.COMP_NO == searchModel.Search_Company), "DEPT_NO", "DEPT_NAME",
                        searchModel.Search_Department);
                }
                else
                {
                    ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                    .Where(c => c.COMP_NO == 1), "DEPT_NO", "DEPT_NAME", searchModel.Search_Department);
                }
            }
            else
            {
                ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                    .Where(c => c.COMP_NO == 1), "DEPT_NO", "DEPT_NAME");
            }

            if (searchModel != null && searchModel.Search_Designation.HasValue)
            {
                ViewBag.Search_Designation = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME",
                    searchModel.Search_Designation);
            }
            else
            {
                ViewBag.Search_Designation = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME");
            }
            return View(sec_users);
        }

        public ActionResult Zonal(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            ViewBag.CurrentSort = sortOrder;

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "USER_NO" : sortOrder;

            IPagedList<SEC_USERS> sec_users = null;
            AllUserSearchViewModel searchModel = null;

            var users = db.SEC_USERS.AsQueryable();

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                searchModel = new AllUserSearchViewModel();

                if (!string.IsNullOrEmpty(Request.QueryString["Search_User_Type"]))
                {
                    searchModel.Search_User_Type = decimal.Parse(Request.QueryString["Search_User_Type"]);
                    users = users.Where(u => u.USER_TYPE_NO == searchModel.Search_User_Type);
                }
                else
                {
                    users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.AreaManager);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Company"]))
                {
                    searchModel.Search_Company = decimal.Parse(Request.QueryString["Search_Company"]);
                    users = users.Where(u => u.COMP_NO == searchModel.Search_Company);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Department"]))
                {
                    searchModel.Search_Department = decimal.Parse(Request.QueryString["Search_Department"]);
                    users = users.Where(u => u.DEPT_NO == searchModel.Search_Department);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Designation"]))
                {
                    searchModel.Search_Designation = decimal.Parse(Request.QueryString["Search_Designation"]);
                    users = users.Where(u => u.DESIG_NO == searchModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Hrid"]))
                {
                    searchModel.Search_Hrid = Request.QueryString["Search_Hrid"];
                    users = users.Where(u => u.HR_EMP_ID == searchModel.Search_Hrid);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Fullname"]))
                {
                    searchModel.Search_Fullname = Request.QueryString["Search_Fullname"];
                    users = users.Where(u => u.USER_FULL_NAME.ToUpper().Contains(searchModel.Search_Fullname.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Mobile"]))
                {
                    searchModel.Search_Mobile = Request.QueryString["Search_Mobile"];
                    users = users.Where(u => u.USER_MOBILE.ToUpper().Contains(searchModel.Search_Mobile.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Email"]))
                {
                    searchModel.Search_Email = Request.QueryString["Search_Email"];
                    users = users.Where(u => u.USER_EMAIL.ToUpper().Contains(searchModel.Search_Email.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Username"]))
                {
                    searchModel.Search_Username = Request.QueryString["Search_Username"];
                    users = users.Where(u => u.USER_NAME.ToUpper().Contains(searchModel.Search_Username.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Active"])
                    && Request.QueryString["Search_Active"] != "null")
                {
                    searchModel.Search_Active = bool.Parse(Request.QueryString["Search_Active"]);
                    decimal active = searchModel.Search_Active == true ? 1 : 0;
                    users = users.Where(u => u.IS_ACTIVE == active);
                }

                Session["User_Search_Model"] = searchModel;
            }

            if (!Request.QueryString.HasKeys() && !page.HasValue)
            {
                Session["User_Search_Model"] = null;
            }

            if (Session["User_Search_Model"] == null)
            {
                users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager
                    || u.USER_TYPE_NO == (decimal)EUserTypes.AreaManager);
            }
            else
            {
                searchModel = (AllUserSearchViewModel)Session["User_Search_Model"];
                if (searchModel.Search_User_Type.HasValue)
                {
                    users = users.Where(u => u.USER_TYPE_NO == searchModel.Search_User_Type);
                }
                else
                {
                    users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager
                        || u.USER_TYPE_NO == (decimal)EUserTypes.AreaManager);
                }
                if (searchModel.Search_Company.HasValue)
                {
                    users = users.Where(u => u.COMP_NO == searchModel.Search_Company);
                }
                if (searchModel.Search_Department.HasValue)
                {
                    users = users.Where(u => u.DEPT_NO == searchModel.Search_Department);
                }
                if (searchModel.Search_Designation.HasValue)
                {
                    users = users.Where(u => u.DESIG_NO == searchModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Hrid))
                {
                    users = users.Where(u => u.HR_EMP_ID == searchModel.Search_Hrid);
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Fullname))
                {
                    users = users.Where(u => u.USER_FULL_NAME.ToUpper().Contains(searchModel.Search_Fullname.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Mobile))
                {
                    users = users.Where(u => u.USER_MOBILE.ToUpper().Contains(searchModel.Search_Mobile.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Email))
                {
                    users = users.Where(u => u.USER_EMAIL.ToUpper().Contains(searchModel.Search_Email.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Username))
                {
                    users = users.Where(u => u.USER_NAME.ToUpper().Contains(searchModel.Search_Username.ToUpper()));
                }
                if (searchModel.Search_Active.HasValue)
                {
                    decimal active = searchModel.Search_Active == true ? 1 : 0;
                    users = users.Where(u => u.IS_ACTIVE == active);
                }
            }

            sec_users = users.OrderBy(m => m.INSERT_TIME)
                            .ToPagedList(pageIndex, pageSize);
            
            ViewBag.Search_User_Model = (AllUserSearchViewModel)Session["User_Search_Model"];

            if (searchModel != null && searchModel.Search_User_Type.HasValue)
            {
                ViewBag.Search_User_Type = new SelectList(db.GEN_USER_TYPES
                        .Where(t => t.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager
                        || t.USER_TYPE_NO == (decimal)EUserTypes.AreaManager), "USER_TYPE_NO", "USER_TYPE",
                        searchModel.Search_User_Type);
            }
            else
            {
                ViewBag.Search_User_Type = new SelectList(db.GEN_USER_TYPES
                        .Where(t => t.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager
                        || t.USER_TYPE_NO == (decimal)EUserTypes.AreaManager), "USER_TYPE_NO", "USER_TYPE");
            }

            if (searchModel != null && searchModel.Search_Company.HasValue)
            {
                ViewBag.Search_Company = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME",
                    searchModel.Search_Company);
            }
            else
            {
                ViewBag.Search_Company = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME");
            }

            if (searchModel != null && searchModel.Search_Department.HasValue)
            {
                if (searchModel.Search_Company.HasValue)
                {
                    ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                        .Where(c => c.COMP_NO == searchModel.Search_Company), "DEPT_NO", "DEPT_NAME",
                        searchModel.Search_Department);
                }
                else
                {
                    ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                    .Where(c => c.COMP_NO == 1), "DEPT_NO", "DEPT_NAME", searchModel.Search_Department);
                }
            }
            else
            {
                ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                    .Where(c => c.COMP_NO == 1), "DEPT_NO", "DEPT_NAME");
            }

            if (searchModel != null && searchModel.Search_Designation.HasValue)
            {
                ViewBag.Search_Designation = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME",
                    searchModel.Search_Designation);
            }
            else
            {
                ViewBag.Search_Designation = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME");
            }
            return View(sec_users);
        }

        public ActionResult Agent(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            ViewBag.CurrentSort = sortOrder;

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "USER_NO" : sortOrder;

            IPagedList<SEC_USERS> sec_users = null;
            AllUserSearchViewModel searchModel = null;

            var users = db.SEC_USERS.AsQueryable();

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                searchModel = new AllUserSearchViewModel();

                if (!string.IsNullOrEmpty(Request.QueryString["Search_User_Type"]))
                {
                    searchModel.Search_User_Type = decimal.Parse(Request.QueryString["Search_User_Type"]);
                    users = users.Where(u => u.USER_TYPE_NO == searchModel.Search_User_Type);
                }
                else
                {
                    users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Agent);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Company"]))
                {
                    searchModel.Search_Company = decimal.Parse(Request.QueryString["Search_Company"]);
                    users = users.Where(u => u.COMP_NO == searchModel.Search_Company);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Department"]))
                {
                    searchModel.Search_Department = decimal.Parse(Request.QueryString["Search_Department"]);
                    users = users.Where(u => u.DEPT_NO == searchModel.Search_Department);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Designation"]))
                {
                    searchModel.Search_Designation = decimal.Parse(Request.QueryString["Search_Designation"]);
                    users = users.Where(u => u.DESIG_NO == searchModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Hrid"]))
                {
                    searchModel.Search_Hrid = Request.QueryString["Search_Hrid"];
                    users = users.Where(u => u.HR_EMP_ID == searchModel.Search_Hrid);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Fullname"]))
                {
                    searchModel.Search_Fullname = Request.QueryString["Search_Fullname"];
                    users = users.Where(u => u.USER_FULL_NAME.ToUpper().Contains(searchModel.Search_Fullname.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Mobile"]))
                {
                    searchModel.Search_Mobile = Request.QueryString["Search_Mobile"];
                    users = users.Where(u => u.USER_MOBILE.ToUpper().Contains(searchModel.Search_Mobile.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Email"]))
                {
                    searchModel.Search_Email = Request.QueryString["Search_Email"];
                    users = users.Where(u => u.USER_EMAIL.ToUpper().Contains(searchModel.Search_Email.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Username"]))
                {
                    searchModel.Search_Username = Request.QueryString["Search_Username"];
                    users = users.Where(u => u.USER_NAME.ToUpper().Contains(searchModel.Search_Username.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Active"])
                    && Request.QueryString["Search_Active"] != "null")
                {
                    searchModel.Search_Active = bool.Parse(Request.QueryString["Search_Active"]);
                    decimal active = searchModel.Search_Active == true ? 1 : 0;
                    users = users.Where(u => u.IS_ACTIVE == active);
                }

                Session["User_Search_Model"] = searchModel;
            }

            if (!Request.QueryString.HasKeys() && !page.HasValue)
            {
                Session["User_Search_Model"] = null;
            }

            if (Session["User_Search_Model"] == null)
            {
                users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Agent);
            }
            else
            {
                searchModel = (AllUserSearchViewModel)Session["User_Search_Model"];
                if (searchModel.Search_User_Type.HasValue)
                {
                    users = users.Where(u => u.USER_TYPE_NO == searchModel.Search_User_Type);
                }
                else
                {
                    users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.Agent);
                }
                if (searchModel.Search_Company.HasValue)
                {
                    users = users.Where(u => u.COMP_NO == searchModel.Search_Company);
                }
                if (searchModel.Search_Department.HasValue)
                {
                    users = users.Where(u => u.DEPT_NO == searchModel.Search_Department);
                }
                if (searchModel.Search_Designation.HasValue)
                {
                    users = users.Where(u => u.DESIG_NO == searchModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Hrid))
                {
                    users = users.Where(u => u.HR_EMP_ID == searchModel.Search_Hrid);
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Fullname))
                {
                    users = users.Where(u => u.USER_FULL_NAME.ToUpper().Contains(searchModel.Search_Fullname.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Mobile))
                {
                    users = users.Where(u => u.USER_MOBILE.ToUpper().Contains(searchModel.Search_Mobile.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Email))
                {
                    users = users.Where(u => u.USER_EMAIL.ToUpper().Contains(searchModel.Search_Email.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Username))
                {
                    users = users.Where(u => u.USER_NAME.ToUpper().Contains(searchModel.Search_Username.ToUpper()));
                }
                if (searchModel.Search_Active.HasValue)
                {
                    decimal active = searchModel.Search_Active == true ? 1 : 0;
                    users = users.Where(u => u.IS_ACTIVE == active);
                }
            }

            sec_users = users.OrderBy(m => m.INSERT_TIME)
                            .ToPagedList(pageIndex, pageSize);

            ViewBag.Search_User_Model = (AllUserSearchViewModel)Session["User_Search_Model"];

            if (searchModel != null && searchModel.Search_User_Type.HasValue)
            {
                ViewBag.Search_User_Type = new SelectList(db.GEN_USER_TYPES
                        .Where(t => t.USER_TYPE_NO == (decimal)EUserTypes.Agent), "USER_TYPE_NO", "USER_TYPE",
                        searchModel.Search_User_Type);
            }
            else
            {
                ViewBag.Search_User_Type = new SelectList(db.GEN_USER_TYPES
                        .Where(t => t.USER_TYPE_NO == (decimal)EUserTypes.Agent), "USER_TYPE_NO", "USER_TYPE");
            }

            if (searchModel != null && searchModel.Search_Company.HasValue)
            {
                ViewBag.Search_Company = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME",
                    searchModel.Search_Company);
            }
            else
            {
                ViewBag.Search_Company = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME");
            }

            if (searchModel != null && searchModel.Search_Department.HasValue)
            {
                if (searchModel.Search_Company.HasValue)
                {
                    ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                        .Where(c => c.COMP_NO == searchModel.Search_Company), "DEPT_NO", "DEPT_NAME",
                        searchModel.Search_Department);
                }
                else
                {
                    ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                    .Where(c => c.COMP_NO == 1), "DEPT_NO", "DEPT_NAME", searchModel.Search_Department);
                }
            }
            else
            {
                ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                    .Where(c => c.COMP_NO == 1), "DEPT_NO", "DEPT_NAME");
            }

            if (searchModel != null && searchModel.Search_Designation.HasValue)
            {
                ViewBag.Search_Designation = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME",
                    searchModel.Search_Designation);
            }
            else
            {
                ViewBag.Search_Designation = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME");
            }
            return View(sec_users);
        }

        public ActionResult GeneralUser(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            ViewBag.CurrentSort = sortOrder;

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "USER_NO" : sortOrder;

            IPagedList<SEC_USERS> sec_users = null;
            AllUserSearchViewModel searchModel = null;

            var users = db.SEC_USERS.AsQueryable();

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                searchModel = new AllUserSearchViewModel();

                if (!string.IsNullOrEmpty(Request.QueryString["Search_User_Type"]))
                {
                    searchModel.Search_User_Type = decimal.Parse(Request.QueryString["Search_User_Type"]);
                    users = users.Where(u => u.USER_TYPE_NO == searchModel.Search_User_Type);
                }
                else
                {
                    users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.GeneralUser);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Company"]))
                {
                    searchModel.Search_Company = decimal.Parse(Request.QueryString["Search_Company"]);
                    users = users.Where(u => u.COMP_NO == searchModel.Search_Company);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Department"]))
                {
                    searchModel.Search_Department = decimal.Parse(Request.QueryString["Search_Department"]);
                    users = users.Where(u => u.DEPT_NO == searchModel.Search_Department);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Designation"]))
                {
                    searchModel.Search_Designation = decimal.Parse(Request.QueryString["Search_Designation"]);
                    users = users.Where(u => u.DESIG_NO == searchModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Hrid"]))
                {
                    searchModel.Search_Hrid = Request.QueryString["Search_Hrid"];
                    users = users.Where(u => u.HR_EMP_ID == searchModel.Search_Hrid);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Fullname"]))
                {
                    searchModel.Search_Fullname = Request.QueryString["Search_Fullname"];
                    users = users.Where(u => u.USER_FULL_NAME.ToUpper().Contains(searchModel.Search_Fullname.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Mobile"]))
                {
                    searchModel.Search_Mobile = Request.QueryString["Search_Mobile"];
                    users = users.Where(u => u.USER_MOBILE.ToUpper().Contains(searchModel.Search_Mobile.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Email"]))
                {
                    searchModel.Search_Email = Request.QueryString["Search_Email"];
                    users = users.Where(u => u.USER_EMAIL.ToUpper().Contains(searchModel.Search_Email.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Username"]))
                {
                    searchModel.Search_Username = Request.QueryString["Search_Username"];
                    users = users.Where(u => u.USER_NAME.ToUpper().Contains(searchModel.Search_Username.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Active"])
                    && Request.QueryString["Search_Active"] != "null")
                {
                    searchModel.Search_Active = bool.Parse(Request.QueryString["Search_Active"]);
                    decimal active = searchModel.Search_Active == true ? 1 : 0;
                    users = users.Where(u => u.IS_ACTIVE == active);
                }

                Session["User_Search_Model"] = searchModel;
            }

            if (!Request.QueryString.HasKeys() && !page.HasValue)
            {
                Session["User_Search_Model"] = null;
            }

            if (Session["User_Search_Model"] == null)
            {
                users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.GeneralUser);
            }
            else
            {
                searchModel = (AllUserSearchViewModel)Session["User_Search_Model"];
                if (searchModel.Search_User_Type.HasValue)
                {
                    users = users.Where(u => u.USER_TYPE_NO == searchModel.Search_User_Type);
                }
                else
                {
                    users = users.Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.GeneralUser);
                }
                if (searchModel.Search_Company.HasValue)
                {
                    users = users.Where(u => u.COMP_NO == searchModel.Search_Company);
                }
                if (searchModel.Search_Department.HasValue)
                {
                    users = users.Where(u => u.DEPT_NO == searchModel.Search_Department);
                }
                if (searchModel.Search_Designation.HasValue)
                {
                    users = users.Where(u => u.DESIG_NO == searchModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Hrid))
                {
                    users = users.Where(u => u.HR_EMP_ID == searchModel.Search_Hrid);
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Fullname))
                {
                    users = users.Where(u => u.USER_FULL_NAME.ToUpper().Contains(searchModel.Search_Fullname.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Mobile))
                {
                    users = users.Where(u => u.USER_MOBILE.ToUpper().Contains(searchModel.Search_Mobile.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Email))
                {
                    users = users.Where(u => u.USER_EMAIL.ToUpper().Contains(searchModel.Search_Email.ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.Search_Username))
                {
                    users = users.Where(u => u.USER_NAME.ToUpper().Contains(searchModel.Search_Username.ToUpper()));
                }
                if (searchModel.Search_Active.HasValue)
                {
                    decimal active = searchModel.Search_Active == true ? 1 : 0;
                    users = users.Where(u => u.IS_ACTIVE == active);
                }
            }

            sec_users = users.OrderBy(m => m.INSERT_TIME)
                            .ToPagedList(pageIndex, pageSize);

            ViewBag.Search_User_Model = (AllUserSearchViewModel)Session["User_Search_Model"];

            if (searchModel != null && searchModel.Search_User_Type.HasValue)
            {
                ViewBag.Search_User_Type = new SelectList(db.GEN_USER_TYPES
                        .Where(t => t.USER_TYPE_NO == (decimal)EUserTypes.GeneralUser), "USER_TYPE_NO", "USER_TYPE",
                        searchModel.Search_User_Type);
            }
            else
            {
                ViewBag.Search_User_Type = new SelectList(db.GEN_USER_TYPES
                        .Where(t => t.USER_TYPE_NO == (decimal)EUserTypes.GeneralUser), "USER_TYPE_NO", "USER_TYPE");
            }

            if (searchModel != null && searchModel.Search_Company.HasValue)
            {
                ViewBag.Search_Company = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME",
                    searchModel.Search_Company);
            }
            else
            {
                ViewBag.Search_Company = new SelectList(db.SET_COMPANY, "COMP_NO", "COMP_NAME");
            }

            if (searchModel != null && searchModel.Search_Department.HasValue)
            {
                if (searchModel.Search_Company.HasValue)
                {
                    ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                        .Where(c => c.COMP_NO == searchModel.Search_Company), "DEPT_NO", "DEPT_NAME",
                        searchModel.Search_Department);
                }
                else
                {
                    ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                    .Where(c => c.COMP_NO == 1), "DEPT_NO", "DEPT_NAME", searchModel.Search_Department);
                }
            }
            else
            {
                ViewBag.Search_Department = new SelectList(db.SET_DEPARTMENT
                    .Where(c => c.COMP_NO == 1), "DEPT_NO", "DEPT_NAME");
            }

            if (searchModel != null && searchModel.Search_Designation.HasValue)
            {
                ViewBag.Search_Designation = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME",
                    searchModel.Search_Designation);
            }
            else
            {
                ViewBag.Search_Designation = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME");
            }
            return View(sec_users);
        }

        public ActionResult GeneralPermissions(long id = 0)
        {
            SEC_USERS user = db.SEC_USERS.Where(a => a.USER_NO == id).FirstOrDefault();
            ViewBag.USER_NAME = user.USER_NAME;

            List<USER_CONTROLLER_ACTION_GET_Result> controllerList = db.USER_CONTROLLER_ACTION_GET(id).ToList();

            List<SET_USER_ACTION> permit_list = db.SET_USER_ACTION.Where(a => a.USER_NO == id).ToList();


            foreach (var item in controllerList)
            {

                SET_USER_ACTION actionPerm = (from p in permit_list
                                              where p.ACTION_NO == item.ACTION_NO
                                              select p).FirstOrDefault();
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
            ViewBag.userNo = id;
            TempData["USER_ID"] = id;
            TempData.Keep();

            return View();
        }

        [HttpPost]
        public ActionResult GeneralPermissions(SET_USER_ACTION[] permissions)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            SET_USER_ACTION permission = permissions.FirstOrDefault();

            if (permission != null)
            {
                List<GEN_CONTROLLER_ACTION> allowedControllerList = db.GEN_CONTROLLER_ACTION
                    .Where(a => a.IS_ACTIVE == 1 && (a.IS_AUTO_INCLUDE == 1 || a.IS_PUBLIC == 1)).ToList();

                foreach (var allowed in allowedControllerList)
                {
                    SET_USER_ACTION allowedRecord = db.SET_USER_ACTION.
                        Where(a => a.ACTION_NO == allowed.ACTION_NO && a.USER_NO == permission.USER_NO).
                        FirstOrDefault();

                    if (allowedRecord == null)
                    {
                        db.SET_USER_ACTION_INSERT(USER_NO, LOGON_NO, permission.USER_NO,
                            allowed.ACTION_NO, 1, "Automatically Allowed");
                    }
                }
            }

            foreach (var perm in permissions)
            {

                SET_USER_ACTION record = db.SET_USER_ACTION.
                    Where(a => a.ACTION_NO == perm.ACTION_NO && a.USER_NO == perm.USER_NO).
                    FirstOrDefault();
                if (record == null)
                {
                    SET_USER_ACTION rolac = new SET_USER_ACTION();

                    rolac.ACTION_NO = perm.ACTION_NO;
                    rolac.USER_NO = perm.USER_NO;
                    rolac.IS_ACTIVE = perm.IS_ACTIVE;

                    //db.GEN_USERS_PERMISSIONS.Add(gen_user_permission);
                    db.SET_USER_ACTION_INSERT(USER_NO, LOGON_NO, rolac.USER_NO, rolac.ACTION_NO,
                        rolac.IS_ACTIVE, rolac.DETAILS);
                }
                else
                {
                    record.IS_ACTIVE = perm.IS_ACTIVE;
                    //db.Entry(record).State = EntityState.Modified;
                    db.SET_USER_ACTION_UPDATE(record.USER_ACTION_NO, USER_NO, LOGON_NO, record.USER_NO,
                        record.ACTION_NO, record.IS_ACTIVE, record.DETAILS);
                }

                List<GEN_CONTROLLER_ACTION> childActions = db.GEN_CONTROLLER_ACTION
                    .Where(a => a.PARENT_ACTION_NO == perm.ACTION_NO).ToList();

                foreach (var child in childActions)
                {
                    SET_USER_ACTION childRecord = db.SET_USER_ACTION.
                        Where(a => a.ACTION_NO == child.ACTION_NO && a.USER_NO == perm.USER_NO).
                        FirstOrDefault();

                    if (childRecord == null)
                    {
                        db.SET_USER_ACTION_INSERT(USER_NO, LOGON_NO, perm.USER_NO, child.ACTION_NO,
                            perm.IS_ACTIVE, "Child Perms");
                    }
                    else
                    {
                        db.SET_USER_ACTION_UPDATE(childRecord.USER_ACTION_NO, USER_NO, LOGON_NO, perm.USER_NO,
                            childRecord.ACTION_NO, perm.IS_ACTIVE, "Child Perms");
                    }
                }
            }

            ViewBag.userId = TempData.Peek("USER_ID");
            decimal userId = ViewBag.userId;

            SEC_USERS User = db.SEC_USERS.Where(a => a.USER_NO == userId).FirstOrDefault();
            ViewBag.USER_NAME = User.USER_NAME;

            //db.SaveChanges();

            List<USER_CONTROLLER_ACTION_GET_Result> controllerList = db.USER_CONTROLLER_ACTION_GET(userId).ToList();

            ViewBag.controllerList = controllerList;


            return View();
        }

        public ActionResult GetReportingAuthorityForm()
        {
            ViewBag.Zonal_Dept = new SelectList(db.SET_DEPARTMENT, "DEPT_NO", "DEPT_NAME");
            ViewBag.Zonal_Desig = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME");

            ViewBag.Division_No = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.Zilla_No = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0),
                "ZILLA_NO", "ZILLA_NAME");
            ViewBag.Zonal_Thana = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0),
                "THANA_NO", "THANA_NAME");

            SEC_USERS_LOGIN_Result1 user = Session["sess_sec_users"] as SEC_USERS_LOGIN_Result1;

            ViewBag.Zonal_Parent_No = user.USER_NO;


            return PartialView("_GetReportingAuthorityPartial");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}