using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OG_SLN.Models;
using PagedList;

namespace OG_SLN.Controllers
{
    public class ClientEntryController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /ClientEntry/

        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            IPagedList<SET_CLIENT_INFO> client_info = null;
            ClientSearchViewModel clientModel = null;

            var clients = db.SET_CLIENT_INFO.AsQueryable();

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                clientModel = new ClientSearchViewModel();

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Name"]))
                {
                    clientModel.Search_Name = Request.QueryString["Search_Name"];
                    clients = clients.Where(u => u.CLIENT_NAME.ToLower()
                        .Contains(clientModel.Search_Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Mobile"]))
                {
                    clientModel.Search_Mobile = Request.QueryString["Search_Mobile"];
                    clients = clients.Where(u => u.CLIENT_MOBILE.ToLower()
                        .Contains(clientModel.Search_Mobile.ToLower()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Division"]))
                {
                    clientModel.Search_Division = decimal.Parse(Request.QueryString["Search_Division"]);
                    clients = clients.Where(u => u.DIVISION_NO == clientModel.Search_Division);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Zilla"]))
                {
                    clientModel.Search_Zilla = decimal.Parse(Request.QueryString["Search_Zilla"]);
                    clients = clients.Where(u => u.ZILLA_NO == clientModel.Search_Zilla);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Thana"]))
                {
                    clientModel.Search_Thana = decimal.Parse(Request.QueryString["Search_Thana"]);
                    clients = clients.Where(u => u.THANA_NO == clientModel.Search_Thana);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Active"])
                    && Request.QueryString["Search_Active"] != "null")
                {
                    clientModel.Search_Active = bool.Parse(Request.QueryString["Search_Active"]);
                    decimal active = clientModel.Search_Active == true ? 1 : 0;
                    clients = clients.Where(u => u.IS_ACTIVE == active);
                }

                Session["Client_Search_Model"] = clientModel;
            }

            if (!Request.QueryString.HasKeys() && !page.HasValue)
            {
                Session["Client_Search_Model"] = null;
            }

            if (Session["Client_Search_Model"] != null)
            {
                clientModel = (ClientSearchViewModel)Session["Client_Search_Model"];
                if (!string.IsNullOrEmpty(clientModel.Search_Name))
                {
                    clients = clients.Where(u => u.CLIENT_NAME.ToLower()
                        .Contains(clientModel.Search_Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(clientModel.Search_Mobile))
                {
                    clients = clients.Where(u => u.CLIENT_MOBILE.ToLower()
                        .Contains(clientModel.Search_Mobile.ToLower()));
                }
                if (clientModel.Search_Division.HasValue)
                {
                    clients = clients.Where(u => u.DIVISION_NO == clientModel.Search_Division);
                }
                if (clientModel.Search_Zilla.HasValue)
                {
                    clients = clients.Where(u => u.ZILLA_NO == clientModel.Search_Zilla);
                }
                if (clientModel.Search_Thana.HasValue)
                {
                    clients = clients.Where(u => u.THANA_NO == clientModel.Search_Thana);
                }

                if (clientModel.Search_Active.HasValue)
                {
                    decimal active = clientModel.Search_Active == true ? 1 : 0;
                    clients = clients.Where(u => u.IS_ACTIVE == active);
                }
            }

            client_info = clients.OrderBy(m => m.CLIENT_NAME)
                            .ToPagedList(pageIndex, pageSize);

            ViewBag.Search_Client_Model = (ClientSearchViewModel)Session["Client_Search_Model"];

            if (clientModel != null && clientModel.Search_Division.HasValue)
            {
                ViewBag.Search_Division = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME",
                    clientModel.Search_Division);
            }
            else
            {
                ViewBag.Search_Division = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            }

            if (clientModel != null && clientModel.Search_Zilla.HasValue)
            {
                ViewBag.Search_Zilla = new SelectList(db.SET_ZILLA, "ZILLA_NO", "ZILLA_NAME",
                    clientModel.Search_Zilla);
            }
            else
            {
                if (clientModel != null && clientModel.Search_Division.HasValue)
                {
                    ViewBag.Search_Zilla = new SelectList(db.SET_ZILLA
                        .Where(z => z.DIVISION_NO == clientModel.Search_Division), "ZILLA_NO", "ZILLA_NAME");
                }
                else
                {
                    ViewBag.Search_Zilla = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0));
                }
            }

            if (clientModel != null && clientModel.Search_Thana.HasValue)
            {
                ViewBag.Search_Thana = new SelectList(db.SET_THANA, "THANA_NO", "THANA_NAME",
                    clientModel.Search_Thana);
            }
            else
            {
                if (clientModel != null && clientModel.Search_Zilla.HasValue)
                {
                    ViewBag.Search_Thana = new SelectList(db.SET_THANA
                        .Where(t => t.ZILLA_NO == clientModel.Search_Zilla), "THANA_NO", "THANA_NAME");
                }
                else
                {
                    ViewBag.Search_Thana = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0));
                }
            }
            /*
            var set_client_info = db.SET_CLIENT_INFO.Include("SET_DIVISION")
                                    .Include("SET_ZILLA").Include("SET_THANA");*/

            return View(client_info);
        }

        //
        // GET: /ClientEntry/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_CLIENT_INFO set_client_info = db.SET_CLIENT_INFO.Single(s => s.CLIENT_NO == id);
            if (set_client_info == null)
            {
                return HttpNotFound();
            }
            return View(set_client_info);
        }

        //
        // GET: /ClientEntry/Create

        public ActionResult Create()
        {
            ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            //ViewBag.ZONE_NO = new SelectList(db.SET_ZONE, "ZONE_NO", "LAST_ACTION");
            ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0), "ZILLA_NO", "ZILLA_NAME");
            ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0), "THANA_NO", "THANA_NAME");
            return View();
        }

        //
        // POST: /ClientEntry/Create

        [HttpPost]
        public ActionResult Create(SET_CLIENT_INFO client)
        {
            if (ModelState.IsValid)
            {
                //db.SET_CLIENT_INFO.AddObject(set_client_info);
                //db.SaveChanges();

                decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
                decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

                db.SET_CLIENT_INFO_INSERT(USER_NO, LOGON_NO, client.CLIENT_NAME, client.CLIENT_NAME_BNG, client.CLIENT_NICK_NAME,
                    client.CLIENT_MOBILE, client.CLIENT_DOB, client.CLIENT_MARRIAGE_DATE, client.CLIENT_ADDR, client.CLIENT_COMENTS, client.DIVISION_NO,
                    client.ZONE_NO, client.ZILLA_NO, client.THANA_NO, client.IS_OFFLINE_ENTRY, client.OFFLINE_ENTRY_TIME,
                    client.OFFLINE_ENTRY_SYNC, client.IS_ACTIVE, client.ACTIVE_FROM, client.ACTIVE_TO, client.SL_NUM);

                return RedirectToAction("Index");
            }

            ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME", client.DIVISION_NO);
            //ViewBag.ZONE_NO = new SelectList(db.SET_ZONE, "ZONE_NO", "LAST_ACTION", client.ZONE_NO);
            ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0), "ZILLA_NO", "ZILLA_NAME");
            ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0), "THANA_NO", "THANA_NAME");
            return View(client);
        }

        //
        // GET: /ClientEntry/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_CLIENT_INFO client = db.SET_CLIENT_INFO.Single(s => s.CLIENT_NO == id);
            if (client == null)
            {
                return HttpNotFound();
            }

            ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME", client.DIVISION_NO);
            //ViewBag.ZONE_NO = new SelectList(db.SET_ZONE, "ZONE_NO", "LAST_ACTION", client.ZONE_NO);
            if (client.DIVISION_NO.HasValue)
            {
                ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.DIVISION_NO == client.DIVISION_NO),
                    "ZILLA_NO", "ZILLA_NAME", client.ZILLA_NO);
            }
            else
            {
                ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0),
                    "ZILLA_NO", "ZILLA_NAME", client.ZILLA_NO);
            }

            if (client.ZILLA_NO.HasValue)
            {
                ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.ZILLA_NO == client.ZILLA_NO),
                    "THANA_NO", "THANA_NAME", client.THANA_NO);
            }
            else
            {
                ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0),
                    "THANA_NO", "THANA_NAME", client.THANA_NO);
            }
            return View(client);
        }

        //
        // POST: /ClientEntry/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_CLIENT_INFO client)
        {
            if (ModelState.IsValid)
            {
                //db.SET_CLIENT_INFO.Attach(set_client_info);
                //db.ObjectStateManager.ChangeObjectState(set_client_info, EntityState.Modified);
                //db.SaveChanges();

                decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
                decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

                db.SET_CLIENT_INFO_UPDATE(client.CLIENT_NO, USER_NO, LOGON_NO, client.CLIENT_NAME, client.CLIENT_NAME_BNG,
                    client.CLIENT_NICK_NAME, client.CLIENT_MOBILE, client.CLIENT_DOB, client.CLIENT_MARRIAGE_DATE, client.CLIENT_ADDR,
                    client.CLIENT_COMENTS, client.DIVISION_NO, client.ZONE_NO, client.ZILLA_NO, client.THANA_NO,
                    client.IS_OFFLINE_ENTRY, client.OFFLINE_ENTRY_TIME, client.OFFLINE_ENTRY_SYNC, client.IS_ACTIVE,
                    client.ACTIVE_FROM, client.ACTIVE_TO, client.SL_NUM);

                return RedirectToAction("Index");
            }
            ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME", client.DIVISION_NO);
            //ViewBag.ZONE_NO = new SelectList(db.SET_ZONE, "ZONE_NO", "LAST_ACTION", client.ZONE_NO);
            if (client.DIVISION_NO.HasValue)
            {
                ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.DIVISION_NO == client.DIVISION_NO),
                    "ZILLA_NO", "ZILLA_NAME", client.ZILLA_NO);
            }
            else
            {
                ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0),
                    "ZILLA_NO", "ZILLA_NAME", client.ZILLA_NO);
            }

            if (client.ZILLA_NO.HasValue)
            {
                ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.ZILLA_NO == client.ZILLA_NO),
                    "THANA_NO", "THANA_NAME", client.THANA_NO);
            }
            else
            {
                ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0),
                    "THANA_NO", "THANA_NAME", client.THANA_NO);
            }
            return View(client);
        }

        //
        // GET: /ClientEntry/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_CLIENT_INFO set_client_info = db.SET_CLIENT_INFO.Single(s => s.CLIENT_NO == id);
            if (set_client_info == null)
            {
                return HttpNotFound();
            }
            return View(set_client_info);
        }

        //
        // POST: /ClientEntry/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            /*
            SET_CLIENT_INFO set_client_info = db.SET_CLIENT_INFO.Single(s => s.CLIENT_NO == id);
            db.SET_CLIENT_INFO.DeleteObject(set_client_info);
            db.SaveChanges();
            */

            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            db.SET_CLIENT_INFO_DELETE(id, USER_NO, LOGON_NO);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}