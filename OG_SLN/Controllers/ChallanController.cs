using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.IO;
using System.Text;
using OG_SLN.Models;
using Microsoft.Office.Interop;
using PagedList;

namespace OG_SLN.Controllers
{
    public class ChallanController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        private List<SelectListItem> upload_types = new List<SelectListItem> { 
                new SelectListItem{Text = "Challan", Value="1"},
                new SelectListItem{Text = "Return", Value="2"}};

        private string challan_upload_url = System.Configuration.ConfigurationManager.AppSettings["CHALLAN_UPLOAD_URL"];

        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            ViewBag.CurrentSort = sortOrder;

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "CHALLAN_DATE" : sortOrder;

            IPagedList<TRN_CHALLAN_UPLOAD> uploads = db.TRN_CHALLAN_UPLOAD.OrderByDescending(c => c.INSERT_TIME)
                                                        .ToPagedList(pageIndex, pageSize);

            return View(uploads);
        }

        //
        // GET: /Challan/Details/5

        public ActionResult Details(decimal id = 0)
        {
            TRN_CHALLAN_UPLOAD upload = db.TRN_CHALLAN_UPLOAD.Single(t => t.UPLOAD_NO == id);
            if (upload == null)
            {
                return HttpNotFound();
            }

            List<TRN_CHALLAN_DET> challans = null;
            List<TRN_RETURN_DET> returns = null;

            if (upload.UPLOAD_TYPE_NUM == 1)
            {
                challans = db.TRN_CHALLAN_DET.Where(c => c.UPLOAD_NO == id).ToList();
                return View("Challans", challans);
            }
            else if (upload.UPLOAD_TYPE_NUM == 2)
            {
                returns = db.TRN_RETURN_DET.Where(r => r.UPLOAD_NO == id).ToList();
                return View("Returns", returns);
            }

            return HttpNotFound();
        }

        //
        // GET: /Challan/Create

        public ActionResult Create()
        {
            ViewBag.UPLOAD_TYPE_NUM = upload_types;

            return View();
        }

        //
        // POST: /Challan/Create

        [HttpPost]
        public ActionResult Create(TRN_CHALLAN_UPLOAD upload)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            ViewBag.UPLOAD_TYPE_NUM = upload_types;

            if (upload.uploadExcel.ContentLength > 0)
            {
                int length = upload.uploadExcel.ContentLength;

                upload.FILE_NAME = upload.uploadExcel.FileName;
                upload.FILE_TYPE = upload.uploadExcel.ContentType;
                upload.FILE_EXT = Path.GetExtension(upload.uploadExcel.FileName);

                if (!System.IO.Directory.Exists(challan_upload_url))
                {
                    System.IO.Directory.CreateDirectory(challan_upload_url);
                }
                
                string fileLocation = challan_upload_url + Path.GetFileNameWithoutExtension(upload.FILE_NAME) 
                                        + DateTime.Now.ToString("_yyyy_MM_dd_hh_mm_ss_") + DateTime.Now.Millisecond
                                        + Path.GetExtension(upload.FILE_NAME);
                while (System.IO.File.Exists(fileLocation))
                {
                    fileLocation = challan_upload_url + Path.GetFileNameWithoutExtension(upload.FILE_NAME)
                                        + DateTime.Now.ToString("_yyyy_MM_dd_hh_mm_ss_") + DateTime.Now.Millisecond
                                        + Path.GetExtension(upload.FILE_NAME);
                }
                upload.uploadExcel.SaveAs(fileLocation);
                upload.FILE_LOCATION = fileLocation;

                
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fileLocation);
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                bool fields_valid = true;

                if (upload.UPLOAD_TYPE_NUM == (decimal)EChallanTypes.Challan)
                {
                    List<TRN_CHALLAN_DET> challans = new List<TRN_CHALLAN_DET>();

                    List<string> HrId_List = new List<string>();
                    List<string> Book_List = new List<string>();
                    string HrId_List_String = null;
                    string Book_List_String = null;

                    string challan_no = xlRange.Cells[1, 1].Value2.ToString();
                    string challan_date = xlRange.Cells[1, 2].Value2.ToString();

                    if (challan_no != "Chalan_No")
                    {
                        ModelState.AddModelError("ERROR_MSG", "Chalan_No doesn't exist for Challan Type Upload");
                        fields_valid = false;
                    }
                    if (challan_date != "Chalan_Date")
                    {
                        ModelState.AddModelError("ERROR_MSG", "Chalan_Date doesn't exist for Challan Type Upload");
                        fields_valid = false;
                    }

                    if (fields_valid)
                    {
                        for (int i = 2; i <= rowCount; i++)
                        {
                            TRN_CHALLAN_DET newChallan = new TRN_CHALLAN_DET();
                            newChallan.CHALLAN_NO = xlRange.Cells[i, 1].Value2.ToString();
                            newChallan.CHALLAN_DATE = DateTime.FromOADate(Double.Parse(xlRange.Cells[i, 2].Value2.ToString()));
                            newChallan.BB_LIBRARY_CODE = xlRange.Cells[i, 3].Value2.ToString();
                            newChallan.EMP_HR_ID = xlRange.Cells[i, 4].Value2.ToString();
                            newChallan.BB_BOOK_CODE = xlRange.Cells[i, 5].Value2.ToString();
                            newChallan.BB_UNIQUE_CODE = xlRange.Cells[i, 6].Value2.ToString();
                            newChallan.SUPPLY_QTY = decimal.Parse(xlRange.Cells[i, 7].Value2.ToString());
                            newChallan.DIST_MAX_DAY = decimal.Parse(xlRange.Cells[i, 8].Value2.ToString());

                            HrId_List.Add(newChallan.EMP_HR_ID);
                            Book_List.Add(newChallan.BB_UNIQUE_CODE);

                            challans.Add(newChallan);
                        }

                        HrId_List = HrId_List.Distinct<string>().ToList();
                        Book_List = Book_List.Distinct<string>().ToList();

                        HrId_List_String = string.Join(",", HrId_List.ToArray());
                        Book_List_String = string.Join(",", Book_List.ToArray());

                        CHALLAN_VALIDITY_CHECK_Result non_existent =
                            db.CHALLAN_VALIDITY_CHECK(HrId_List_String, Book_List_String).ToList().FirstOrDefault();
                        if (non_existent.V_HR_COUNT > 0)
                        {
                            ModelState.AddModelError("ERROR_MSG", "Employee HR Id is not valid " + non_existent.V_HR_LIST);
                        }
                        if (non_existent.V_BOOK_COUNT > 0)
                        {
                            ModelState.AddModelError("ERROR_MSG", "Book Unique Code is not valid " + non_existent.V_BOOK_LIST);
                        }

                        if (non_existent.V_HR_LIST == null) non_existent.V_HR_LIST = string.Empty;
                        if (non_existent.V_BOOK_LIST == null) non_existent.V_BOOK_LIST = string.Empty;

                        if (ModelState.IsValid)
                        {

                            decimal? UPLOAD_NO = db.TRN_CHALLAN_UPLOAD_INSERT(USER_NO, LOGON_NO, upload.FILE_NAME, upload.FILE_LOCATION,
                                upload.FILE_TYPE, upload.FILE_EXT, (decimal)(rowCount - 1), (decimal)HrId_List.Count,
                                (decimal)Book_List.Count, (decimal)non_existent.V_HR_COUNT, (decimal)non_existent.V_BOOK_COUNT,
                                non_existent.V_HR_LIST, non_existent.V_BOOK_LIST, upload.UPLOAD_TYPE_NUM).FirstOrDefault().UPLOAD_NO;
                            if (UPLOAD_NO.HasValue)
                            {
                                foreach (TRN_CHALLAN_DET challan in challans)
                                {
                                    try
                                    {
                                        db.TRN_CHALLAN_DET_INSERT(USER_NO, LOGON_NO, UPLOAD_NO, challan.CHALLAN_NO,
                                        challan.CHALLAN_DATE, challan.BB_LIBRARY_CODE, challan.EMP_HR_ID, challan.BB_BOOK_CODE,
                                        challan.BB_UNIQUE_CODE, challan.SUPPLY_QTY, challan.DIST_MAX_DAY);
                                    }
                                    catch (Exception e)
                                    {
                                        ModelState.AddModelError("ERROR_MSG", e.InnerException.Message);
                                        return View(upload);
                                    }
                                }
                            }

                            return RedirectToAction("Index");
                        }
                    }
                }
                else if (upload.UPLOAD_TYPE_NUM == (decimal)EChallanTypes.Return)
                {

                    List<TRN_RETURN_DET> returns = new List<TRN_RETURN_DET>();

                    List<string> HrId_List = new List<string>();
                    List<string> Book_List = new List<string>();
                    string HrId_List_String = null;
                    string Book_List_String = null;

                    string return_no = xlRange.Cells[1, 1].Value2.ToString();
                    string return_date = xlRange.Cells[1, 2].Value2.ToString();

                    if (return_no != "Return_Slip_No")
                    {
                        ModelState.AddModelError("ERROR_MSG", "Return_Slip_No doesn't exist for Return Type Upload");
                        fields_valid = false;
                    }
                    if (return_date != "Return_Date")
                    {
                        ModelState.AddModelError("ERROR_MSG", "Return_Date doesn't exist for Return Type Upload");
                        fields_valid = false;
                    }

                    if (fields_valid)
                    {
                        for (int i = 2; i <= rowCount; i++)
                        {
                            TRN_RETURN_DET newReturn = new TRN_RETURN_DET();
                            newReturn.RETURN_SLIP_NO = xlRange.Cells[i, 1].Value2.ToString(); ;
                            newReturn.RETURN_DATE = DateTime.FromOADate(Double.Parse(xlRange.Cells[i, 2].Value2.ToString()));
                            newReturn.BB_LIBRARY_CODE = xlRange.Cells[i, 3].Value2.ToString();
                            newReturn.EMP_HR_ID = xlRange.Cells[i, 4].Value2.ToString();
                            newReturn.BB_BOOK_CODE = xlRange.Cells[i, 5].Value2.ToString();
                            newReturn.BB_UNIQUE_CODE = xlRange.Cells[i, 6].Value2.ToString();
                            newReturn.RETURN_QTY = decimal.Parse(xlRange.Cells[i, 7].Value2.ToString());

                            HrId_List.Add(newReturn.EMP_HR_ID);
                            Book_List.Add(newReturn.BB_UNIQUE_CODE);

                            returns.Add(newReturn);
                        }

                        HrId_List = HrId_List.Distinct<string>().ToList();
                        Book_List = Book_List.Distinct<string>().ToList();

                        HrId_List_String = string.Join(",", HrId_List.ToArray());
                        Book_List_String = string.Join(",", Book_List.ToArray());

                        CHALLAN_VALIDITY_CHECK_Result non_existent =
                            db.CHALLAN_VALIDITY_CHECK(HrId_List_String, Book_List_String).ToList().FirstOrDefault();
                        if (non_existent.V_HR_COUNT > 0)
                        {
                            ModelState.AddModelError("ERROR_MSG", "Employee HR Id is not valid " + non_existent.V_HR_LIST);
                        }
                        if (non_existent.V_BOOK_COUNT > 0)
                        {
                            ModelState.AddModelError("ERROR_MSG", "Book Unique Code is not valid " + non_existent.V_BOOK_LIST);
                        }

                        if (non_existent.V_HR_LIST == null) non_existent.V_HR_LIST = string.Empty;
                        if (non_existent.V_BOOK_LIST == null) non_existent.V_BOOK_LIST = string.Empty;

                        if (ModelState.IsValid)
                        {

                            decimal? UPLOAD_NO = db.TRN_CHALLAN_UPLOAD_INSERT(USER_NO, LOGON_NO, upload.FILE_NAME, upload.FILE_LOCATION,
                                upload.FILE_TYPE, upload.FILE_EXT, (decimal)(rowCount - 1), (decimal)HrId_List.Count,
                                (decimal)Book_List.Count, (decimal)non_existent.V_HR_COUNT, (decimal)non_existent.V_BOOK_COUNT,
                                non_existent.V_HR_LIST, non_existent.V_BOOK_LIST, upload.UPLOAD_TYPE_NUM).FirstOrDefault().UPLOAD_NO;

                            if (UPLOAD_NO.HasValue)
                            {
                                foreach (TRN_RETURN_DET ret in returns)
                                {
                                    try
                                    {
                                        db.TRN_RETURN_DET_INSERT(USER_NO, LOGON_NO, UPLOAD_NO, ret.RETURN_SLIP_NO,
                                        ret.RETURN_DATE, ret.BB_LIBRARY_CODE, ret.EMP_HR_ID, ret.BB_BOOK_CODE,
                                        ret.BB_UNIQUE_CODE, ret.RETURN_QTY);
                                    }
                                    catch (Exception e)
                                    {
                                        ModelState.AddModelError("ERROR_MSG", e.InnerException.Message);
                                        return View(upload);
                                    }
                                }
                            }

                            return RedirectToAction("Index");
                        }
                    }
                }
            }

            return View(upload);
        }

        //
        // GET: /Challan/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            TRN_CHALLAN_UPLOAD trn_challan_upload = db.TRN_CHALLAN_UPLOAD.Single(t => t.UPLOAD_NO == id);
            if (trn_challan_upload == null)
            {
                return HttpNotFound();
            }
            ViewBag.UPLOAD_TYPE_NUM = upload_types;

            return View(trn_challan_upload);
        }

        //
        // POST: /Challan/Edit/5

        [HttpPost]
        public ActionResult Edit(TRN_CHALLAN_UPLOAD trn_challan_upload)
        {
            ViewBag.UPLOAD_TYPE_NUM = upload_types;

            if (ModelState.IsValid)
            {
                db.TRN_CHALLAN_UPLOAD.Attach(trn_challan_upload);
                db.ObjectStateManager.ChangeObjectState(trn_challan_upload, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trn_challan_upload);
        }

        //
        // GET: /Challan/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            TRN_CHALLAN_UPLOAD trn_challan_upload = db.TRN_CHALLAN_UPLOAD.Single(t => t.UPLOAD_NO == id);
            if (trn_challan_upload == null)
            {
                return HttpNotFound();
            }
            return View(trn_challan_upload);
        }

        //
        // POST: /Challan/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            TRN_CHALLAN_UPLOAD trn_challan_upload = db.TRN_CHALLAN_UPLOAD.Single(t => t.UPLOAD_NO == id);
            db.TRN_CHALLAN_UPLOAD_DELETE(trn_challan_upload.UPLOAD_NO, USER_NO, LOGON_NO);

            return RedirectToAction("Index");
        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}