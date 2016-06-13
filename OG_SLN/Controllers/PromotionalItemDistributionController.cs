using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using OG_SLN.Models;
namespace OG_SLN.Controllers
{
    public class PromotionalItemDistributionController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /PromotionalItemDistribution/

        public Tuple<PromotionalItemSearch, IQueryable<TRN_USER_PROMO_ITEM>> setSearch(IQueryable<TRN_USER_PROMO_ITEM> specimens, PromotionalItemSearch specsearch,bool search)
        {
            specsearch = (specsearch.dirty) ? specsearch :
                new PromotionalItemSearch();
            if (!string.IsNullOrEmpty(Request.QueryString["USER_NO"]) || search)
            {
                specsearch.userNo = (!search) ? decimal.Parse(Request.QueryString["USER_NO"].ToString()) : specsearch.userNo;
                if (!string.IsNullOrEmpty(specsearch.userNo.ToString()))
                    specimens = specimens.Where(s => s.SEC_USERS.USER_NO == specsearch.userNo);
            }
            else
                specsearch.userNo = null;
            if (!string.IsNullOrEmpty(Request.QueryString["user_name"]))
            {
                specsearch.user_name = Request.QueryString["user_name"];
                specimens = specimens.Where(s => s.SEC_USERS.USER_NAME.Contains(specsearch.user_name));
            }
            else
                specsearch.user_name = "";
            if (!string.IsNullOrEmpty(Request.QueryString["userno"]))
            {
                specsearch.userno = decimal.Parse(Request.QueryString["userno"]);
                if (specsearch.userno != 0)
                    specimens = specimens.Where(s => s.USER_NO == specsearch.userno);
            }
            else
                specsearch.userno = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["AssignDateFrom"]))
            {
                specsearch.AssignDateFrom = DateTime.Parse(Request.QueryString["AssignDateFrom"]);
                specimens = specimens.Where(s => s.ASSIGN_DATE >= specsearch.AssignDateFrom);
            }
            else
                specsearch.AssignDateFrom = null;
            if (!string.IsNullOrEmpty(Request.QueryString["AssignDateTo"]))
            {
                specsearch.AssignDateTo = DateTime.Parse(Request.QueryString["AssignDateTo"]);
                specimens = specimens.Where(s => s.ASSIGN_DATE == specsearch.AssignDateTo);
            }
            else
                specsearch.AssignDateTo = null;
            specsearch.isactive = (!string.IsNullOrEmpty(Request.QueryString["Search_Active"])) ?
                Request.QueryString["Search_Active"] : specsearch.isactive;
            if (specsearch.isactive != "all")
            {
                specimens = specimens.Where(s => s.IS_ACTIVE == ((specsearch.isactive == "active") ? 1 : 0));

            }
            if (!string.IsNullOrEmpty(Request.QueryString["promotional_item_no"]))
            {
                specsearch.promotional_item_no = decimal.Parse(Request.QueryString["promotional_item_no"]);
                if (specsearch.promotional_item_no != 0)
                {
                    specimens = specimens.Where(s => db.TRN_USER_PROMO_DET
                        .Where(det => det.PROMO_ITEM_NO == specsearch.promotional_item_no)
                        .Select(det => det.USER_PROMO_NO)
                        .Contains(s.USER_PROMO_NO)
                        );
                }
            }
            else
                specsearch.promotional_item_no = 0;
            specsearch.dirty = true;
            return new Tuple<PromotionalItemSearch, IQueryable<TRN_USER_PROMO_ITEM>>(specsearch, specimens);
        }
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 5;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            //sortOrder = String.IsNullOrEmpty(sortOrder) ? "USER_NO" : sortOrder;
            IPagedList<TRN_USER_PROMO_ITEM> pagedSpecimens = null;
            IQueryable<TRN_USER_PROMO_ITEM> specimens = db.TRN_USER_PROMO_ITEM.AsQueryable();

            ViewBag.ZonalUsers = db.SEC_USERS.Where(u => u.USER_TYPE_NO == 5);
            ViewBag.specs = db.SET_PROMO_ITEM.Where(s => s.IS_ACTIVE == 1);
            ViewBag.currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.futureDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd");
            PromotionalItemSearch specsearch = (Session["promotion_search"] == null) ? new PromotionalItemSearch()
                : (PromotionalItemSearch)Session["promotion_search"];
            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                Tuple<PromotionalItemSearch, IQueryable<TRN_USER_PROMO_ITEM>> tmp = setSearch(specimens, specsearch,false);
                specimens = tmp.Item2;
                specsearch = tmp.Item1;
            }
            else if (page.HasValue && Session["promotion_search"] != null)
            {
                specsearch = (PromotionalItemSearch)Session["promotion_search"];
                Tuple<PromotionalItemSearch, IQueryable<TRN_USER_PROMO_ITEM>> tmp = setSearch(specimens, specsearch,true);
                specimens = tmp.Item2;
                specsearch = tmp.Item1;
            }
            else if (page.HasValue && Session["promotion_search"] == null)
            {
                specsearch = new PromotionalItemSearch();
                specsearch.isactive = "all";
            }
            else
                specsearch.isactive = "all";
            Session["promotion_search"] = specsearch;
            pagedSpecimens = specimens.OrderByDescending(m => m.INSERT_TIME).ToPagedList(pageIndex, pageSize);
            ViewBag.specSearch = specsearch;
            ViewBag.USER_NO = new SelectList(db.SEC_USERS.Where(a => a.USER_TYPE_NO == 
                (decimal)EUserTypes.ZonalManager)
                .ToList(), "USER_NO", "USER_FULL_NAME");
            return View(pagedSpecimens);
        }


        //
        // GET: /PromotionalItemDistribution/Details/5

        public ActionResult Details(decimal id = 0)
        {
            TRN_USER_PROMO_ITEM trn_user_promo_item = db.TRN_USER_PROMO_ITEM.Single(t => t.USER_PROMO_NO == id);
            if (trn_user_promo_item == null)
            {
                return HttpNotFound();
            }
            ViewBag.dets = trn_user_promo_item.TRN_USER_PROMO_DET.ToList();
            return View(trn_user_promo_item);
        }

        //
        // GET: /PromotionalItemDistribution/Create

        public ActionResult Create(decimal? id)
        {
            if (Session["sess_USER_NO"] != null)
            {
                List<string> lstTmp = new List<string>();
                foreach (string crntSession in Session)
                    if (crntSession.Contains("promotion"))
                        lstTmp.Add(crntSession);
                foreach (string tmp in lstTmp)
                    Session.Remove(tmp);
                ViewBag.ZonalUsers = db.SEC_USERS.Where(u => u.USER_TYPE_NO == 5);

                ViewBag.jsmsg = (id != null) ? "<script type='text/javascript' language='javascript'>setforEdit(" + id + "); </script>" : "";

                ViewBag.specimens = db.SET_PROMO_ITEM.Where(s => s.IS_ACTIVE == 1);

                ViewBag.currentDate = (id != null) ? DateTime.Now.ToString("yyyy-MM-dd") : "";
                ViewBag.futureDate = (id != null) ? DateTime.Now.AddDays(60).ToString("yyyy-MM-dd") : "";
                IList<TRN_USER_PROMO_ITEM> usr_spec_list = db.TRN_USER_PROMO_ITEM.OrderByDescending(t => t.LAST_ACTION_TIME).ToList();
                List<temp_TRN_USER_PROMO_ITEM> tmpPromoList = new List<temp_TRN_USER_PROMO_ITEM>();
                foreach (TRN_USER_PROMO_ITEM data in usr_spec_list)
                    tmpPromoList.Add(new temp_TRN_USER_PROMO_ITEM(data.USER_PROMO_NO, data.IS_ACTIVE, data.ASSIGN_DATE, data.TARGET_DATE_FROM, data.TARGET_DATE_TO, data.USER_NO));
                ViewBag.PrevDataList = tmpPromoList;
                //db.SET_SPECIMEN.First(s=>s.SPECIMEN_NO=3).SPECIMEN_NAME
                return View();
            }
            else {
                return RedirectToAction("Login", "Login");
            }
        }

        //
        // POST: /PromotionalItemDistribution/Create

        [HttpPost]
        public ActionResult Create(TRN_USER_PROMO_ITEM trn_user_promo_item)
        {
            //TRN_USER_SPECIMEN trn_user_specimen = new TRN_USER_SPECIMEN();
            if (ModelState.IsValid)
            {
                //TryUpdateModel(trn_user_specimen, frm);
                //trn_user_specimen.IS_ACTIVE = bool.Parse(frm["IS_ACTIVE"].ToString()) == true ? 1 : 0;
                //db.TRN_USER_SPECIMEN.AddObject(trn_user_specimen);
                /*db.TRN_USER_SPECIMEN.AddObject(trn_user_specimen);
                db.SaveChanges();*/
                if (trn_user_promo_item.edit_ID == 0)
                    trn_user_promo_item.USER_PROMO_NO = db.TRN_USER_PROMO_ITEM_INSERT(
                        decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                        trn_user_promo_item.USER_NO,
                        trn_user_promo_item.ASSIGN_DATE,
                        trn_user_promo_item.TARGET_DATE_FROM,
                        trn_user_promo_item.TARGET_DATE_TO,
                        trn_user_promo_item.IS_ACTIVE).First().Value;
                else
                {
                    db.TRN_USER_PROMO_ITEM_UPDATE(trn_user_promo_item.edit_ID,
                        decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                        trn_user_promo_item.USER_NO,
                        trn_user_promo_item.ASSIGN_DATE,
                        trn_user_promo_item.TARGET_DATE_FROM,
                        trn_user_promo_item.TARGET_DATE_TO,
                        trn_user_promo_item.IS_ACTIVE);
                }
                List<string> lstTmp = new List<string>();
                foreach (string crntSession in Session)
                {
                    if (crntSession.Contains("promotion"))
                    {
                        lstTmp.Add(crntSession);
                        decimal tmp = decimal.Parse(crntSession.Replace("promotion", ""));
                        temp_TRN_USER_PROMO_ITEM_DET temp_sess_det = (temp_TRN_USER_PROMO_ITEM_DET)Session[crntSession];
                        if (!temp_sess_det.deleted)
                        {
                            TRN_USER_PROMO_DET tmp_det = db.TRN_USER_PROMO_DET.FirstOrDefault(t => t.PROMO_ITEM_NO == tmp
                                && t.USER_PROMO_NO == ((trn_user_promo_item.edit_ID == 0) ? trn_user_promo_item.USER_PROMO_NO :
                                            trn_user_promo_item.edit_ID));
                            TRN_USER_PROMO_DET trn_user_promotion_det = (tmp_det == null) ? new TRN_USER_PROMO_DET() : tmp_det;
                            trn_user_promotion_det.QTY = temp_sess_det.qty;
                            trn_user_promotion_det.IS_ACTIVE = (temp_sess_det.active) ? 1 : 0;
                            trn_user_promotion_det.SET_PROMO_ITEM = db.SET_PROMO_ITEM.Single(t => t.PROMO_ITEM_NO == tmp);

                            if (trn_user_promotion_det.USER_PROMO_DET_NO != 0)
                                db.TRN_USER_PROMO_DET_UPDATE(trn_user_promotion_det.USER_PROMO_DET_NO,
                                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                                    decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                                     (trn_user_promo_item.edit_ID == 0) ? trn_user_promo_item.USER_PROMO_NO :
                                            trn_user_promo_item.edit_ID,
                                    trn_user_promotion_det.PROMO_ITEM_NO,
                                    trn_user_promotion_det.QTY,
                                    trn_user_promotion_det.IS_ACTIVE);
                            else
                                db.TRN_USER_PROMO_DET_INSERT(decimal.Parse(Session["sess_USER_NO"].ToString()),
                                    decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                                    (trn_user_promo_item.edit_ID == 0) ? trn_user_promo_item.USER_PROMO_NO :
                                            trn_user_promo_item.edit_ID,
                                    trn_user_promotion_det.PROMO_ITEM_NO,
                                    trn_user_promotion_det.QTY,
                                    trn_user_promotion_det.IS_ACTIVE);
                        }
                        else
                        {
                            TRN_USER_PROMO_DET trn_tmp = db.TRN_USER_PROMO_DET.FirstOrDefault(t => t.PROMO_ITEM_NO == temp_sess_det.specimenid &&
                               t.USER_PROMO_NO == trn_user_promo_item.edit_ID);
                            if (trn_tmp != null)
                            {
                                db.TRN_USER_PROMO_DET_DELETE(trn_tmp.USER_PROMO_DET_NO,
                                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                                    decimal.Parse(Session["sess_LOGON_NO"].ToString()));
                            }
                        }
                    }
                }
                foreach (string tmp in lstTmp)
                    Session.Remove(tmp);
                //int count = db.SaveChanges();
                return RedirectToAction("Index");
            }
            //else
            //return View();
            return View(trn_user_promo_item);
        }

        //
        // GET: /PromotionalItemDistribution/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            TRN_USER_PROMO_ITEM trn_user_promo_item = db.TRN_USER_PROMO_ITEM.Single(t => t.USER_PROMO_NO == id);
            if (trn_user_promo_item == null)
            {
                return HttpNotFound();
            }
            ViewBag.USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_user_promo_item.USER_NO);
            return View(trn_user_promo_item);
        }

        //
        // POST: /PromotionalItemDistribution/Edit/5

        [HttpPost]
        public ActionResult Edit(TRN_USER_PROMO_ITEM trn_user_promo_item)
        {
            if (ModelState.IsValid)
            {
                db.TRN_USER_PROMO_ITEM.Attach(trn_user_promo_item);
                db.ObjectStateManager.ChangeObjectState(trn_user_promo_item, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_user_promo_item.USER_NO);
            return View(trn_user_promo_item);
        }

        //
        // GET: /PromotionalItemDistribution/Delete/5
        public JsonResult SaveSpecimenOnSession(int id, int qty, string status)
        {
            /*if (Session["specimen" + id] == null)
                Session.Add("specimen" + id, qty);
            else*/

            Session["promotion" + id] = new temp_TRN_USER_PROMO_ITEM_DET(id, (status == "Active") ? true : false, qty);
            //qty = 0;
            return Json(new { dt = id });
        }
        public JsonResult getOnePrevData(int? id)
        {
            TRN_USER_PROMO_ITEM trn_user_promotion = db.TRN_USER_PROMO_ITEM.Single(t => t.USER_PROMO_NO == id);
            /*JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(trn_user_specimen);*/
            return Json(new
            {
                specid = trn_user_promotion.USER_PROMO_NO,
                userno = trn_user_promotion.USER_NO,
                userName = db.SEC_USERS.Single(t => t.USER_NO == trn_user_promotion.USER_NO).USER_NAME,
                assign_date = trn_user_promotion.ASSIGN_DATE.ToString("yyyy-MM-dd"),
                target_date_from = DateTime.Parse(trn_user_promotion.TARGET_DATE_FROM.ToString()).ToString("yyyy-MM-dd"),
                target_date_to = DateTime.Parse(trn_user_promotion.TARGET_DATE_TO.ToString()).ToString("yyyy-MM-dd"),
                active = trn_user_promotion.IS_ACTIVE
            }, JsonRequestBehavior.AllowGet);
        }
        public string getPrevDetails(int? id)
        {
            IList<TRN_USER_PROMO_DET> trn_user_specimen_det = db.TRN_USER_PROMO_DET.Where(t => t.USER_PROMO_NO == id).ToList();
            /*JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(trn_user_specimen);*/
            string ret = "";
            List<string> toberemoved = new List<string>();
            foreach (string crntSession in Session)
                if (crntSession.Contains("promotion"))
                    toberemoved.Add(crntSession);

            foreach (string tmp in toberemoved)
                Session.Remove(tmp);
            foreach (TRN_USER_PROMO_DET tmp_det in trn_user_specimen_det)
            {
                SET_PROMO_ITEM specimen = db.SET_PROMO_ITEM.Single(t => t.PROMO_ITEM_NO == tmp_det.PROMO_ITEM_NO);
                //Session.Add("specimen" + tmp_det.SPECIMEN_NO, tmp_det.QTY);
                this.SaveSpecimenOnSession(int.Parse(tmp_det.PROMO_ITEM_NO.ToString()), int.Parse(tmp_det.QTY.ToString()), (tmp_det.IS_ACTIVE == 1) ? "Active" : "Inactive");
                if (specimen != null)
                    ret += "<tr id=" + tmp_det.PROMO_ITEM_NO + " tmptag=" + tmp_det.USER_PROMO_DET_NO + "><td>" + specimen.PROMO_ITEM_CODE + "</td><td>" + specimen.PROMO_ITEM_NAME +
                        "</td><td>" + tmp_det.QTY + "</td><td>" + ((tmp_det.ACTIVE_SPEC) ? "Active" : "Inactive") + "</td><td><input class=\"btn btn-small btn-info\" onclick=\"setEdit($(this).closest('tr').children(':eq(0)').text(),$(this).closest('tr').children(':eq(2)').text())\" value=\"Edit\" type=\"button\">" +
                        "<input class=\"btn btn-small btn-danger\" onclick=\"removeSpecimen( $(this).closest(\'tr\').attr('id'));$(this).closest(\'tr\').remove();\" value=\"Remove\" type=\"button\"></td><td></td></tr>";
            }
            return ret;
        }
        
        public JsonResult getPreviousData()
        {
            IList<TRN_USER_PROMO_ITEM> allData = db.TRN_USER_PROMO_ITEM.ToList();
            var tmparr = new { };

            //Session.Remove("specimen" + name);
            var spec = from data in db.TRN_USER_PROMO_ITEM
                       select new
                       {
                           specID = data.USER_PROMO_NO,
                           userno = db.SEC_USERS.FirstOrDefault(t => t.USER_NO == data.USER_NO).USER_NAME,
                           isactive = data.IS_ACTIVE,
                           assignDate = ((DateTime)data.ASSIGN_DATE).ToShortDateString()
                       };

            /*var tmp = db.TRN_USER_SPECIMEN_DET.Select(a => new
            {
                assigndate = a.UPDATE_TIME
            });*/

            return Json(spec, JsonRequestBehavior.AllowGet);
        }
        public void RemoveSpecimenFromDB(int? specid, int? edit_id)
        {
            /*TRN_USER_SPECIMEN_DET trn_tmp = db.TRN_USER_SPECIMEN_DET.FirstOrDefault(t => t.SPECIMEN_NO == specid &&
                t.USER_SPECIMEN_NO == edit_id);
            if (trn_tmp != null)
            {
                db.TRN_USER_SPECIMEN_DET_DELETE(trn_tmp.SPECIMEN_DET_NO,
                    trn_tmp.LAST_ACTION_USER_NO,
                    trn_tmp.LAST_ACTION_LOGON_NO);*/
            temp_TRN_USER_PROMO_ITEM_DET from_sess = (temp_TRN_USER_PROMO_ITEM_DET)Session["promotion" + specid];
            from_sess.set_deleted();
            Session["promotion" + specid] = from_sess;
            /*return "Specimen successfully removed.";
        }
        else
            return "Data not found";*/
        }
        
        public ActionResult Delete(decimal id = 0)
        {
           /* if (Session["sess_USER_NO"] != null)
            {*/
                TRN_USER_PROMO_ITEM trn_user_promo_item = db.TRN_USER_PROMO_ITEM.Single(t => t.USER_PROMO_NO == id);
                if (trn_user_promo_item == null)
                {
                    return HttpNotFound();
                }
                ViewBag.dets = trn_user_promo_item.TRN_USER_PROMO_DET.ToList();
                return View(trn_user_promo_item);
            /*}
            else
            {
                return RedirectToAction("Login", "Login");
            }*/
        }

        //
        // POST: /PromotionalItemDistribution/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            TRN_USER_PROMO_ITEM trn_user_promo = db.TRN_USER_PROMO_ITEM.Single(t => t.USER_PROMO_NO == id);
            
            IQueryable<TRN_USER_PROMO_DET> dets = db.TRN_USER_PROMO_DET
               .Where(x => x.USER_PROMO_NO == id).AsQueryable();
            foreach (TRN_USER_PROMO_DET det in dets)
                db.TRN_USER_PROMO_DET_DELETE(det.USER_PROMO_DET_NO, decimal.Parse(Session["sess_USER_NO"].ToString()), decimal.Parse(Session["sess_LOGON_NO"].ToString()));
            db.TRN_USER_PROMO_ITEM_DELETE(trn_user_promo.USER_PROMO_NO, decimal.Parse(Session["sess_USER_NO"].ToString()), decimal.Parse(Session["sess_LOGON_NO"].ToString()));

            return RedirectToAction("Index");
        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        } 
    }
}