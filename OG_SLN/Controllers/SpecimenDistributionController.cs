using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.Script.Serialization;
using PagedList;
using OG_SLN.Models;
namespace OG_SLN.Controllers
{
    public class SpecimenDistributionController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /SpecimenDistribution/
        public Tuple<SpecimenDistSearch, IQueryable<TRN_USER_SPECIMEN>> setSearch(IQueryable<TRN_USER_SPECIMEN> specimens, SpecimenDistSearch specsearch, bool search)
        {
            specsearch = (specsearch.dirty) ? specsearch :
                new SpecimenDistSearch();
            if (!string.IsNullOrEmpty(Request.QueryString["USER_NO"]) || search)
            {
                specsearch.USER_NO = (!search) ? decimal.Parse(Request.QueryString["USER_NO"].ToString()) : specsearch.USER_NO;
                if (!string.IsNullOrEmpty(specsearch.USER_NO.ToString()))
                    specimens = specimens.Where(s => s.SEC_USERS.USER_NO==(specsearch.USER_NO));
            }
            else
                specsearch.USER_NO = null;
            if (!string.IsNullOrEmpty(Request.QueryString["userno"]) || search)
            {
                specsearch.userno =  (!search) ?decimal.Parse(Request.QueryString["userno"]):specsearch.userno;
                if (specsearch.userno != null && specsearch.userno != 0)
                    specimens = specimens.Where(s => s.USER_NO == specsearch.userno);
            }
            else
                specsearch.userno = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["AssignDateFrom"]) || search)
            {
                specsearch.AssignDateFrom = (!search) ?DateTime.Parse(Request.QueryString["AssignDateFrom"]):specsearch.AssignDateFrom;
                if (specsearch.AssignDateFrom != null )
                    specimens = specimens.Where(s => s.ASSIGN_DATE >= specsearch.AssignDateFrom);
            }
            else
                specsearch.AssignDateFrom = null;
            if (!string.IsNullOrEmpty(Request.QueryString["AssignDateTo"]) || search)
            {
                specsearch.AssignDateTo = (!search) ? DateTime.Parse(Request.QueryString["AssignDateTo"]) : specsearch.AssignDateFrom;
                if (specsearch.AssignDateTo != null) 
                    specimens = specimens.Where(s => s.ASSIGN_DATE <= specsearch.AssignDateTo);
            }
            else
                specsearch.AssignDateTo = null;

            specsearch.isactive = (!string.IsNullOrEmpty(Request.QueryString["Search_Active"])) ?
                Request.QueryString["Search_Active"] : specsearch.isactive;
            if (specsearch.isactive != "all")
            {
                specimens = specimens.Where(s => s.IS_ACTIVE == ((specsearch.isactive == "active") ? 1 : 0));

            }
            if (!string.IsNullOrEmpty(Request.QueryString["specimen_no"]) || search)
            {
                specsearch.specimen_no = (!search)?decimal.Parse(Request.QueryString["specimen_no"]):specsearch.specimen_no;
                if (specsearch.specimen_no != 0 && specsearch.specimen_no!=null)
                {
                    specimens = specimens.Where(s => db.TRN_USER_SPECIMEN_DET
                        .Where(det => det.SPECIMEN_NO == specsearch.specimen_no)
                        .Select(det => det.USER_SPECIMEN_NO)
                        .Contains(s.USER_SPECIMEN_NO)
                        );
                }
            }
            else
                specsearch.specimen_no = 0;
            specsearch.dirty = true;
            return new Tuple<SpecimenDistSearch, IQueryable<TRN_USER_SPECIMEN>>(specsearch, specimens);
        }
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            //sortOrder = String.IsNullOrEmpty(sortOrder) ? "USER_NO" : sortOrder;
            IPagedList<TRN_USER_SPECIMEN> pagedSpecimens = null;
            IQueryable<TRN_USER_SPECIMEN> specimens = db.TRN_USER_SPECIMEN.AsQueryable();

            ViewBag.ZonalUsers = db.SEC_USERS.Where(u => u.GEN_USER_TYPES.SHORT_NAME == "ZM");
            ViewBag.specs = db.SET_SPECIMEN.Where(s => s.IS_ACTIVE == 1);
            ViewBag.currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.futureDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd");
            SpecimenDistSearch specsearch = (Session["Specimen_search"] == null) ? new SpecimenDistSearch()
                : (SpecimenDistSearch)Session["Specimen_search"];
            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                Tuple<SpecimenDistSearch, IQueryable<TRN_USER_SPECIMEN>> tmp = setSearch(specimens, specsearch,false);
                specimens = tmp.Item2;
                specsearch = tmp.Item1;
            }
            else if (page.HasValue && specsearch.dirty)
            {
                specsearch = (SpecimenDistSearch)Session["Specimen_search"];
                Tuple<SpecimenDistSearch, IQueryable<TRN_USER_SPECIMEN>> tmp = setSearch(specimens, specsearch,true);
                specimens = tmp.Item2;
                specsearch = tmp.Item1;
            }
            else if (page.HasValue && Session["Specimen_search"] == null)
            {
                specsearch = new SpecimenDistSearch();
                specsearch.isactive = "all";
            }
            else
                specsearch.isactive = "all";
            Session["Specimen_search"] = specsearch;
            pagedSpecimens = specimens.OrderByDescending(m => m.INSERT_TIME).ToPagedList(pageIndex, pageSize);
            ViewBag.specSearch = specsearch;
            ViewBag.USER_NO = new SelectList(db.SEC_USERS.Where(a => a.USER_TYPE_NO ==
               (decimal)EUserTypes.ZonalManager)
               .ToList(), "USER_NO", "USER_FULL_NAME");
            return View(pagedSpecimens);
        }

        //
        // GET: /SpecimenDistribution/Details/5

        public ActionResult Details(decimal id = 0)
        {
            TRN_USER_SPECIMEN trn_user_specimen = db.TRN_USER_SPECIMEN.Single(t => t.USER_SPECIMEN_NO == id);
            if (trn_user_specimen == null)
            {
                return HttpNotFound();
            }
            ViewBag.dets = trn_user_specimen.TRN_USER_SPECIMEN_DET.ToList();
            return View(trn_user_specimen);
        }
        //
        // GET: /SpecimenDistribution/Create

        public ActionResult Create(decimal? id)
        {
            List<string> lstTmp = new List<string>();
            foreach (string crntSession in Session)
                if (crntSession.Contains("specimen"))
                    lstTmp.Add(crntSession);
            foreach (string tmp in lstTmp)
                Session.Remove(tmp);
            ViewBag.ZonalUsers = db.SEC_USERS.Where(u => u.GEN_USER_TYPES.SHORT_NAME == "ZM");

            ViewBag.jsmsg = (id != null) ? "<script type='text/javascript' language='javascript'>setforEdit(" + id + "); </script>" : "";

            ViewBag.specimens = db.SET_SPECIMEN.Where(s => s.IS_ACTIVE == 1);

            ViewBag.currentDate = (id != null) ? DateTime.Now.ToString("yyyy-MM-dd") : "";
            ViewBag.futureDate = (id != null) ? DateTime.Now.AddDays(60).ToString("yyyy-MM-dd") : "";
            IList<TRN_USER_SPECIMEN> usr_spec_list = db.TRN_USER_SPECIMEN.OrderByDescending(t => t.LAST_ACTION_TIME).ToList();
            List<temp_TRN_USER_SPECIMEN> tmpSpecList = new List<temp_TRN_USER_SPECIMEN>();
            foreach (TRN_USER_SPECIMEN data in usr_spec_list)
                tmpSpecList.Add(new temp_TRN_USER_SPECIMEN(data.USER_SPECIMEN_NO, data.IS_ACTIVE, data.ASSIGN_DATE, data.TARGET_DATE_FROM, data.TARGET_DATE_TO, data.USER_NO));
            ViewBag.PrevDataList = tmpSpecList;

            //db.SET_SPECIMEN.First(s=>s.SPECIMEN_NO=3).SPECIMEN_NAME
            return View();
        }

        //
        // POST: /SpecimenDistribution/Create

        [HttpPost]
        public ActionResult Create(TRN_USER_SPECIMEN trn_user_specimen)
        {
            //TRN_USER_SPECIMEN trn_user_specimen = new TRN_USER_SPECIMEN();
            if (ModelState.IsValid)
            {
                //TryUpdateModel(trn_user_specimen, frm);
                //trn_user_specimen.IS_ACTIVE = bool.Parse(frm["IS_ACTIVE"].ToString()) == true ? 1 : 0;
                //db.TRN_USER_SPECIMEN.AddObject(trn_user_specimen);
                /*db.TRN_USER_SPECIMEN.AddObject(trn_user_specimen);
                db.SaveChanges();*/
                if (trn_user_specimen.edit_ID == 0)
                    trn_user_specimen.USER_SPECIMEN_NO = db.TRN_USER_SPECIMEN_INSERT(
                        decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                        trn_user_specimen.USER_NO,
                        trn_user_specimen.ASSIGN_DATE,
                        trn_user_specimen.TARGET_DATE_FROM,
                        trn_user_specimen.TARGET_DATE_TO,
                        trn_user_specimen.IS_ACTIVE).First().Value;
                else
                {
                    db.TRN_USER_SPECIMEN_UPDATE(trn_user_specimen.edit_ID,
                        decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                        trn_user_specimen.USER_NO,
                        trn_user_specimen.ASSIGN_DATE,
                        trn_user_specimen.TARGET_DATE_FROM,
                        trn_user_specimen.TARGET_DATE_TO,
                        trn_user_specimen.IS_ACTIVE);
                }
                List<string> lstTmp = new List<string>();
                foreach (string crntSession in Session)
                {
                    if (crntSession.Contains("specimen"))
                    {
                        lstTmp.Add(crntSession);
                        decimal tmp = decimal.Parse(crntSession.Replace("specimen", ""));
                        temp_TRN_USER_SPECIMEN_DET temp_sess_det = (temp_TRN_USER_SPECIMEN_DET)Session[crntSession];
                        if (!temp_sess_det.deleted)
                        {
                            TRN_USER_SPECIMEN_DET tmp_det = db.TRN_USER_SPECIMEN_DET.FirstOrDefault(t => t.SPECIMEN_NO == tmp
                                && t.USER_SPECIMEN_NO == ((trn_user_specimen.edit_ID == 0) ? trn_user_specimen.USER_SPECIMEN_NO :
                                            trn_user_specimen.edit_ID));
                            TRN_USER_SPECIMEN_DET trn_user_specimen_det = (tmp_det == null) ? new TRN_USER_SPECIMEN_DET() : tmp_det;
                            trn_user_specimen_det.QTY = temp_sess_det.qty;
                            trn_user_specimen_det.IS_ACTIVE = (temp_sess_det.active) ? 1 : 0;
                            trn_user_specimen_det.SET_SPECIMEN = db.SET_SPECIMEN.Single(t => t.SPECIMEN_NO == tmp);

                            if (trn_user_specimen_det.SPECIMEN_DET_NO != 0)
                                db.TRN_USER_SPECIMEN_DET_UPDATE(trn_user_specimen_det.SPECIMEN_DET_NO,
                                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                                    decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                                     (trn_user_specimen.edit_ID == 0) ? trn_user_specimen.USER_SPECIMEN_NO :
                                            trn_user_specimen.edit_ID,
                                     trn_user_specimen_det.SPECIMEN_NO,
                                    trn_user_specimen_det.QTY,
                                    trn_user_specimen_det.IS_ACTIVE);
                            else
                                db.TRN_USER_SPECIMEN_DET_INSERT(decimal.Parse(Session["sess_USER_NO"].ToString()),
                                    decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                                    (trn_user_specimen.edit_ID == 0) ? trn_user_specimen.USER_SPECIMEN_NO :
                                            trn_user_specimen.edit_ID,
                                    trn_user_specimen_det.SPECIMEN_NO,
                                    trn_user_specimen_det.QTY,
                                    trn_user_specimen_det.IS_ACTIVE);
                        }
                        else
                        {
                            TRN_USER_SPECIMEN_DET trn_tmp = db.TRN_USER_SPECIMEN_DET.FirstOrDefault(t => t.SPECIMEN_NO == temp_sess_det.specimenid &&
                               t.USER_SPECIMEN_NO == trn_user_specimen.edit_ID);
                            if (trn_tmp != null)
                            {
                                db.TRN_USER_SPECIMEN_DET_DELETE(trn_tmp.SPECIMEN_DET_NO,
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
            return View(trn_user_specimen);
        }

        //
        // GET: /SpecimenDistribution/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            TRN_USER_SPECIMEN trn_user_specimen = db.TRN_USER_SPECIMEN.Single(t => t.USER_SPECIMEN_NO == id);
            if (trn_user_specimen == null)
            {
                return HttpNotFound();
            }
            return View(trn_user_specimen);
        }

        //
        // POST: /SpecimenDistribution/Edit/5

        [HttpPost]
        public ActionResult Edit(TRN_USER_SPECIMEN trn_user_specimen)
        {
            if (ModelState.IsValid)
            {
                db.TRN_USER_SPECIMEN.Attach(trn_user_specimen);
                db.ObjectStateManager.ChangeObjectState(trn_user_specimen, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trn_user_specimen);
        }

        //
        // GET: /SpecimenDistribution/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            TRN_USER_SPECIMEN trn_user_specimen = db.TRN_USER_SPECIMEN.Single(t => t.USER_SPECIMEN_NO == id);
            if (trn_user_specimen == null)
            {
                return HttpNotFound();
            }
            ViewBag.dets = trn_user_specimen.TRN_USER_SPECIMEN_DET.ToList();
            return View(trn_user_specimen);
        }
        public JsonResult getOnePrevData(int? id)
        {
            TRN_USER_SPECIMEN trn_user_specimen = db.TRN_USER_SPECIMEN.Single(t => t.USER_SPECIMEN_NO == id);
            /*JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(trn_user_specimen);*/
            return Json(new
            {
                specid = trn_user_specimen.USER_SPECIMEN_NO,
                userno = trn_user_specimen.USER_NO,
                userName = db.SEC_USERS.Single(t => t.USER_NO == trn_user_specimen.USER_NO).USER_NAME,
                assign_date = trn_user_specimen.ASSIGN_DATE.ToString("yyyy-MM-dd"),
                target_date_from = DateTime.Parse(trn_user_specimen.TARGET_DATE_FROM.ToString()).ToString("yyyy-MM-dd"),
                target_date_to = DateTime.Parse(trn_user_specimen.TARGET_DATE_TO.ToString()).ToString("yyyy-MM-dd"),
                active = trn_user_specimen.IS_ACTIVE
            }, JsonRequestBehavior.AllowGet);
        }
        public string getPrevDetails(int? id)
        {
            IList<TRN_USER_SPECIMEN_DET> trn_user_specimen_det = db.TRN_USER_SPECIMEN_DET.Where(t => t.USER_SPECIMEN_NO == id).ToList();
            /*JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(trn_user_specimen);*/
            string ret = "";
            List<string> toberemoved = new List<string>();
            foreach (string crntSession in Session)
                if (crntSession.Contains("specimen"))
                    toberemoved.Add(crntSession);

            foreach (string tmp in toberemoved)
                Session.Remove(tmp);
            foreach (TRN_USER_SPECIMEN_DET tmp_det in trn_user_specimen_det)
            {
                SET_SPECIMEN specimen = db.SET_SPECIMEN.Single(t => t.SPECIMEN_NO == tmp_det.SPECIMEN_NO);
                //Session.Add("specimen" + tmp_det.SPECIMEN_NO, tmp_det.QTY);
                this.SaveSpecimenOnSession(int.Parse(tmp_det.SPECIMEN_NO.ToString()), int.Parse(tmp_det.QTY.ToString()), (tmp_det.IS_ACTIVE == 1) ? "Active" : "Inactive");
                if (specimen != null)
                    ret += "<tr id=" + tmp_det.SPECIMEN_NO + " tmptag=" + tmp_det.SPECIMEN_DET_NO + "><td>" + specimen.SPECIMEN_CODE + "</td><td>" + specimen.SPECIMEN_NAME +
                        "</td><td>" + tmp_det.QTY + "</td><td>" + ((tmp_det.ACTIVE_SPEC) ? "Active" : "Inactive") + "</td><td><input class=\"btn btn-small btn-info\" onclick=\"setEdit($(this).closest('tr').children(':eq(0)').text(),$(this).closest('tr').children(':eq(2)').text())\" value=\"Edit\" type=\"button\">" +
                        "<input class=\"btn btn-small btn-danger\" onclick=\"removeSpecimen( $(this).closest(\'tr\').attr('id'));$(this).closest(\'tr\').remove();\" value=\"Remove\" type=\"button\"></td><td></td></tr>";
            }
            return ret;


        }
        
        /*public JsonResult editSpecimen(int id, int qty)
        { 
            if (Session["specimen" + id] != null)
                Session["specimen" + id]
        }*/
        public JsonResult SaveSpecimenOnSession(int id, int qty, string status)
        {
            /*if (Session["specimen" + id] == null)
                Session.Add("specimen" + id, qty);
            else*/

            Session["specimen" + id] = new temp_TRN_USER_SPECIMEN_DET(id, (status == "Active") ? true : false, qty);
            //qty = 0;
            return Json(new { dt = id });
        }
        /*public JsonResult RemoveSpecimenOnSession(int? name)
        {
            Session.Remove("specimen" + name);
            return Json(new { dt = name });
        }*/
        public JsonResult getPreviousData()
        {
            IList<TRN_USER_SPECIMEN> allData = db.TRN_USER_SPECIMEN.ToList();
            var tmparr = new { };

            //Session.Remove("specimen" + name);
            var spec = from data in db.TRN_USER_SPECIMEN
                       select new
                       {
                           specID = data.USER_SPECIMEN_NO,
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
            temp_TRN_USER_SPECIMEN_DET from_sess = (temp_TRN_USER_SPECIMEN_DET)Session["specimen" + specid];
            from_sess.set_deleted();
            Session["specimen" + specid] = from_sess;
            /*return "Specimen successfully removed.";
        }
        else
            return "Data not found";*/
        }
        //
        // POST: /SpecimenDistribution/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            TRN_USER_SPECIMEN trn_user_specimen = db.TRN_USER_SPECIMEN.Single(t => t.USER_SPECIMEN_NO == id);
            /*db.TRN_USER_SPECIMEN.DeleteObject(trn_user_specimen);
            db.SaveChanges();*/
            IQueryable<TRN_USER_SPECIMEN_DET> dets = db.TRN_USER_SPECIMEN_DET
                .Where(x => x.USER_SPECIMEN_NO == id).AsQueryable();
            foreach (TRN_USER_SPECIMEN_DET det in dets)
                db.TRN_USER_SPECIMEN_DET_DELETE(det.SPECIMEN_DET_NO, decimal.Parse(Session["sess_USER_NO"].ToString()), decimal.Parse(Session["sess_LOGON_NO"].ToString()));
            db.TRN_USER_SPECIMEN_DELETE(trn_user_specimen.USER_SPECIMEN_NO, decimal.Parse(Session["sess_USER_NO"].ToString()), decimal.Parse(Session["sess_LOGON_NO"].ToString()));

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}