using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Collections;
using System.Text;
using System.Data.Objects.SqlClient;


using OG_SLN.Models;
using OG_SLN.Utils;

namespace OG_SLN.Controllers
{
    public class PublishController : Controller
    {
        private OGDBEntities db = new OGDBEntities();
        private object tr;

        public ActionResult Index()
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            DcrPublishSearchModel searchModel = new DcrPublishSearchModel();


            //searchModel = ViewBag.Search_DcrPublish_Model as DcrPublishSearchModel;


            if (!string.IsNullOrEmpty(Request.QueryString["Search_User"]))
            {
                searchModel.Search_User = decimal.Parse(Request.QueryString["Search_User"]);
            }


            if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_From"]))
            {
                searchModel.Search_Date_From = DateTime.Parse(Request.QueryString["Search_Date_From"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_To"]))
            {
                searchModel.Search_Date_To = DateTime.Parse(Request.QueryString["Search_Date_To"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Search_pubno"]))
            {
                searchModel.Search_pubno = decimal.Parse(Request.QueryString["Search_pubno"]);
            }


            //DcrPublishSearchModel searchModel = ViewBag.Search_DcrPublish_Model;

            // searchModel = ViewBag.Search_DcrPublish_Model; 

            ViewBag.Search_DcrPublish_Model = searchModel;


            List<DCR_PUB_MASTER_GET_Result> publish_list =
            db.DCR_PUB_MASTER_GET(searchModel.Search_User, USER_NO,
            searchModel.Search_pubno, searchModel.Search_Date_From, searchModel.Search_Date_To).ToList();


            // List<TRN_USER_PUBLISH> user_pub = db.TRN_USER_PUBLISH(searchModel.Search_pubno,null,null,null);

            ViewBag.Puball_Count = publish_list.Count;



            return View(publish_list);

        }


        public ActionResult DcrPrint()
        {
            DcrPublishSearchModel searchModel = new DcrPublishSearchModel();
            DcrPublishViewModel dcrPublishModel = new DcrPublishViewModel();

            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;


            //string path = Path.GetTempFileName();// file implementation with temp file in system
            //string path = @"d:\test\MyTest.txt";

            //creating file in a specified directory 

            string publish_export_url = System.Configuration.ConfigurationManager.AppSettings["PUBLISH_EXPORT_URL"];


            string Search_User = Request.QueryString["Search_User"];

            searchModel.Search_pubno = decimal.Parse(Request.QueryString["Search_pubno"]);



            string[] user_dcr = Search_User.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            string directory_path = publish_export_url + "\\" + searchModel.Search_pubno;

            if (user_dcr.Length > 0)
            {

                if (!System.IO.Directory.Exists(directory_path))
                {
                    System.IO.Directory.CreateDirectory(directory_path);
                }
            }

            TRN_USER_PUBLISH user_publish = db.TRN_USER_PUBLISH.Where(q=>q.PUBLISH_NO ==searchModel.Search_pubno).FirstOrDefault();

            searchModel.Search_Date_From = user_publish.TRN_DATE_FROM;

            searchModel.Search_Date_To = user_publish.TRN_DATE_TO;
            

            string content = "";

            foreach (string user in user_dcr)
            {
                decimal user_no = decimal.Parse(user);
                SEC_USERS sec_user = db.SEC_USERS.Where(u => u.USER_NO == user_no).FirstOrDefault();

                string path = directory_path + "\\" + searchModel.Search_pubno + "_" + sec_user.USER_NAME + ".html";

                if (!System.IO.File.Exists(path))
                {
                    List<DCR_PUB_EXP_GET_Result> expense =
                        db.DCR_PUB_EXP_GET(user_no, null,
                            searchModel.Search_Date_From, searchModel.Search_Date_To, 1).ToList();

                    content = "<html><title>DCR Publish Report</title><body>";
                    //content += "<table>";

                    content += "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:13px\"><tr>";
                    content += "<td colspan=\"9\" style=\"text-align:center;  border:solid 1px #e4e4e4;\"> <h4>DCR Top Sheet</h4>";

                    content += "<b>Publish No. : " + searchModel.Search_pubno + "<br />";
                    content += "User Full Name : " + sec_user.USER_FULL_NAME + "<br />";

                    content += "Contact : " + sec_user.USER_MOBILE + "<br />";

                    content += "User Name : " + sec_user.USER_NAME + "<br />";

                    content += "Date From : " + searchModel.Search_Date_From.Value.ToString("yyyy-MM-dd") + " Date To : " + searchModel.Search_Date_To.Value.ToString("yyyy-MM-dd");

                    content += "</b></td></tr>";

                    content += "\n<tr><th style=\"text-align:center; border:solid 1px #e4e4e4;\">Date</th><th style=\"text-align:center; border:solid 1px #e4e4e4;\">T.A.</th><th class=\"right\"> T.Entertainment  </th>  <th class=\"right\">P.B.</th><th class=\"right\"> D.A. </th><th class=\"right\"> Other</th><th style=\"text-align:center; border:solid 1px #e4e4e4;\">Total </th> </tr>";

                    int ta = 0;
                    int te = 0;
                    int pb = 0;
                    int da = 0;
                    int other = 0;
                    int total_exp = 0;

                    int total_final = 0;

                    foreach (var exp in expense)
                    {

                        ta += (int)exp.TA_AMT;
                        te += (int)exp.TE_AMT;
                        pb += (int)exp.PB_AMT;
                        da += (int)exp.DA_AMT;
                        other += (int)exp.OB_AMT;

                        total_exp = (int)exp.TA_AMT + (int)exp.TE_AMT + (int)exp.PB_AMT + (int)exp.DA_AMT + (int)exp.OB_AMT;
                        content += "\n<tr><td  style=\"text-align:center; border:solid 1px #e4e4e4; border-top:none;\">" + exp.DT.Value.ToString("yyyy-MM-dd") + "</td><td style=\"text-align:center; border:solid 1px #e4e4e4;\">" + exp.TA_AMT + " </td><td style=\"text-align:center; border:solid 1px #e4e4e4;\">" + exp.TE_AMT + "</td><td style=\"text-align:center; border:solid 1px #e4e4e4;\">" + exp.PB_AMT + "</td><td style=\"text-align:center; border:solid 1px #e4e4e4;\">" + exp.DA_AMT + "</td><td style=\"text-align:center; border:solid 1px #e4e4e4;\">" + exp.OB_AMT + "</td> <td style=\"text-align:center; border:solid 1px #e4e4e4;\"> " + total_exp + "</td> </tr>";

                    }

                    total_final = ta + te + pb + da + other;
                    content += "<tr style=\"font-weight:bold\"><td style=\"text-align:center; border:solid 1px #e4e4e4; border-top:none;\">Total DCR Bill</td><td style=\" font-weight:bold; text-align:center; border:solid 1px #e4e4e4; border-left:none;\"> " + String.Format("{0:#,###0}", ta) + " <td style=\" font-weight:bold; text-align:center; border:solid 1px #e4e4e4; border-left:none;\">" + String.Format("{0:#,###0}", te) + "</td><td style=\" font-weight:bold; text-align:center; border:solid 1px #e4e4e4; border-left:none;\">" + String.Format("{0:#,###0}", pb) + "</td><td style=\" font-weight:bold; text-align:center; border:solid 1px #e4e4e4; border-left:none;\">" + String.Format("{0:#,###0}", da) + "</td> <td style=\" font-weight:bold; text-align:center; border:solid 1px #e4e4e4; border-left:none;\">" + String.Format("{0:#,###0}", other) + "</td><td style=\" font-weight:bold; text-align:center; border:solid 1px #e4e4e4; border-left:none;\">" + String.Format("{0:#,###0}", total_final) + "</td></tr>";

                    content += "</table>";

                    List<DCR_PUB_SPECIMEN_GET_Result> specimen =
                        db.DCR_PUB_SPECIMEN_GET(user_no, null,
                            searchModel.Search_Date_From, searchModel.Search_Date_To, 1).ToList();


                    content += "<table  class =\"page-break\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:13px; margin-top:20px; border-width:5px;border-color:Black;\">";
                    content += "<td colspan=\"5\"  style=\"text-align:center;  border:solid 1px #e4e4e4;\"><h4> Specimen Report</h4></td>";


                    content += "\n <tr><th>SL No. </th><th> Specimen Code </th><th>Specimen Name (Eng) </th><th>Specimen Name (Bng) </th><th>Quantity</th></tr>";


                    int sl_no = 1;

                    int total = 0;

                    foreach (var spec in specimen)
                    {
                        content += "\n<tr> <td  style=\"text-align:center;  border:solid 1px #e4e4e4;  border-top:none;\">" + sl_no++ + "</td>   <td  style=\"text-align:center;  border:solid 1px #e4e4e4;  border-top:none;\">" + spec.SPECIMEN_CODE + "</td><td  style=\"text-align:center;  border:solid 1px #e4e4e4;  border-top:none;\">" + spec.SPECIMEN_NAME + "</td><td  style=\"text-align:center;  border:solid 1px #e4e4e4;  border-top:none;\">" + spec.SPECIMEN_NAME_BNG + "</td><td  style=\"text-align:center;  border:solid 1px #e4e4e4;  border-top:none;\">" + spec.QTY + "</td></tr>";

                        total += (int)spec.QTY;
                    }

                    content += "<tr style=\"font-weight:bold\">";
                    content += "<td colspan=\"4\" align=\"center\"  style=\"text-align:center;  border:solid 1px #e4e4e4;  border-top:none;\">Grand Total</td>";

                    content += "<td style=\"text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">" + String.Format("{0:#,###0}", total) + "</td>";
                    content += "</tr>";

                    content += "</table>";

                    dcrPublishModel.expenses = new DcrExpenseModel();
                    //dcrPublishModel.expenses.user = user;
                    dcrPublishModel.expenses.date = DateTime.Now;
                    dcrPublishModel.expenses.expense = expense;

                    dcrPublishModel.specimens = new DcrSpecimenModel();
                    //dcrPublishModel.specimens.user = user;
                    dcrPublishModel.specimens.date = DateTime.Now;
                    dcrPublishModel.specimens.specimen = specimen;

                    dcrPublishModel.details = new List<DcrDetailsModel>();

                    dcrPublishModel.expenses.date = searchModel.Search_Date_From;
                    dcrPublishModel.specimens.date = searchModel.Search_Date_From;

                    for (DateTime? date = searchModel.Search_Date_From;
                        date <= searchModel.Search_Date_To; date = date.Value.AddDays(1))
                    {
                        DcrDetailsModel dcrDetails = new DcrDetailsModel();

                        List<DCR_PUB_DCR_DETAIL_GET_Result> detail =
                            db.DCR_PUB_DCR_DETAIL_GET(user_no, null, date, date, 1).ToList();

                        List<TRN_EXPENSE_APPROVAL_GET_Result> detailexpense =
                            db.TRN_EXPENSE_APPROVAL_GET(
                            user_no, decimal.Parse(Session["sess_USER_NO"].ToString()),//parent
                            null, null, null, date, date, 1, 1, null).ToList();

                        if (detail.Count > 0 || detailexpense.Count > 0)
                        {

                            content += "<table><tbody>";
                            if (detail.Count > 0)
                            {
                                content += "<tr style=\"height:0px; font-size:0px;\"></tr><tr style=\"height:0px; font-size:0px;\"><td width=\"5%\">&nbsp;</td><td width=\"18%\">&nbsp;</td><td width=\"18%\">&nbsp;</td><td width=\"5%\">&nbsp;</td><td width=\"5%\">&nbsp;</td><td width=\"25%\">&nbsp;</td><td width=\"8%\">&nbsp;</td><td width=\"4%\">&nbsp;</td><td width=\"4%\">&nbsp;</td><td width=\"4%\">&nbsp;</td><td width=\"3%\">&nbsp;</td><td width=\"3%\">&nbsp;</td></tr>";

                                content += "<tr><td colspan=\"7\" style=\"background:#DDDDDD; text-align:center; border:solid 1px #e4e4e4;\">&nbsp;</td> <td style=\"background:#DDDDDD; text-align:center; border:solid 1px #e4e4e4; border-left:none;\"/>DCR Date</td><td style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none;\">" + date.Value.ToString("yyyy-MM-dd") + " </td></tr>";

                                content += "<tr style=\"font-size:13px; background:#DDDDDD; font-weight:bold;\"><td colspan=\"2\" style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none;\">Area of Work </td><td colspan=\"2\" style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none;\">Time of Work</td><td rowspan=\"2\" style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none;\">Institution<br>Name</td><td rowspan=\"2\" style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none;\">Teacher Number</td><td rowspan=\"2\" style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none;\">P. Work</td><td rowspan=\"2\" style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none;\">T. Type</td><td rowspan=\"2\" style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none;\">Fare</td></tr><tr style=\"font-size:13px; background:#DDDDDD; font-weight:bold;\"><td style=\" text-align:center; border:solid 1px #e4e4e4; border-top:none;\">From</td><td style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">To</td><td style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">From</td><td style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">To</td> </tr>";

                            }
                            int sub_total = 0;

                            foreach (var det in detail)
                            {
                                content += "\n<tr><td style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">" + det.WORK_AREA_FROM_NAME + "</td><td style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">" + det.WORK_AREA_TO_NAME + "</td><td style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">" + det.TIME_FROM + "</td><td style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">" + det.TIME_TO + "</td><td style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">" + det.INSTITUTE_NAME + "</td><td style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">" + det.TEACHER_MOBILE + "</td><td style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">" + det.DCR_TYPE_CODE + "</td><td class=\"transtype\" style=\" text-align:center; border:solid 1px #e4e4e4; border-left:none; border-top:none;\"> " + det.TRANS_TYPE_CODE + "</td><td class=\"fareamt\" style=\" text-align:right; border:solid 1px #e4e4e4; border-left:none; border-top:none;\">" + det.APPROVE_FARE_AMT + "</td></tr>";

                                sub_total += (int)det.APPROVE_FARE_AMT;
                            }

                            if (detail.Count > 0)
                            {
                                content += " <tr> <td colspan=\"6\">&nbsp;</td><td colspan=\"2\" style=\" text-align:center;\"> <b>Sub Total : </b></td><td id=\"dcrSubTotal\" style=\" text-align:center;\" ><b>" + String.Format("{0:#,###0}", sub_total) + "</b></td></tr>";
                            }
                            content += "</tbody></table>";

                            content += " <table id=\"dcr_other\" style=\"font-size:13px; text-align:center; margin-top:20px\" width=\"50%\" border=\"1\" cellpadding=\"5\" cellspacing=\"0\">";
                            content += "<tr style=\"background:green; font-weight:bold;\"></tr>";
                            if (detailexpense.Count > 0)
                            {
                                content += "\n<tr><th>ExpType</th><th> Cost </th><th>  Vendor  </th></tr>";

                            }

                            foreach (var detexp in detailexpense)
                            {
                                content += "\n<tr><td>" + detexp.EXP_TYPE_CODE + "</td><td>" + detexp.APPROVE_EXP_AMT + "</td><td>" + detexp.VENDOR + "</td></tr>";
                            }

                            content += "</table>";
                        }
                    }


                    content += "\n</body></html>";

                    using (FileStream fs = System.IO.File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(content);
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                        fs.Close();
                    }
                }
            }

            //return PartialView("_DcrPublishPrint", dcrPublishModel);

            return null;
        }







        [HttpPost]
        public ActionResult Index(DcrPublishSearchModel searchModel)
        {

            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            List<OG_SLN.DCR_PUB_MASTER_GET_Result> pub_result = db.DCR_PUB_MASTER_GET(searchModel.Search_User, USER_NO,
            searchModel.Search_pubno, searchModel.Search_Date_From, searchModel.Search_Date_To).ToList();


            return PartialView("_PubResult", pub_result);
        }

        public ActionResult SalesPanel()
        {
            PublishSalesSearchModel salesSearchModel = new PublishSalesSearchModel();

            decimal? USER_PARENT_NO = Session["sess_USER_NO"] as decimal?;// saving session at USER_PARENT

            ViewBag.Search_Sales_Model = salesSearchModel;

            if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_From"]))
            {
                salesSearchModel.Search_Date_From = DateTime.Parse(Request.QueryString["Search_Date_From"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_To"]))
            {
                salesSearchModel.Search_Date_To = DateTime.Parse(Request.QueryString["Search_Date_To"]);
            }

            List<DCR_PUB_SALES_GET_Result> published_list =

            db.DCR_PUB_SALES_GET

            (null, USER_PARENT_NO, salesSearchModel.PUBLISH_NO,

             salesSearchModel.Search_Date_From, salesSearchModel.Search_Date_To).ToList();

            return View(published_list);
        }


        [HttpPost]
        public ActionResult SalesPanel(DcrPublishSearchModel searchModel)
        {

            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;


            List<DCR_PUB_SALES_GET_Result> published_sales_list =

            db.DCR_PUB_SALES_GET(searchModel.Search_User, USER_NO, searchModel.Search_pubno,
            searchModel.Search_Date_From, searchModel.Search_Date_To).ToList();

            TRN_USER_PUBLISH publish = db.TRN_USER_PUBLISH
                .Where(p => p.PUBLISH_NO == searchModel.Search_pubno).FirstOrDefault();

            ViewBag.Publish = publish;

            ViewBag.Pub_Count = published_sales_list.Count;

            return PartialView("_SalesPanelResult", published_sales_list);

        }

        [HttpPost]
        public JsonResult DcrDoConfirm(PublishSalesSearchModel model)
        {
            decimal? CONFIRM_USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? CONFIRM_LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            //db.DCR_PUB_DO_PUBLISH(model.User_list,USER_NO,LOGON_NO,model.Search_Date_From,model.Search_Date_To);

            db.DCR_PUB_DO_CONFIRM(model.user_not_confirmed, model.PUBLISH_NO, model.IS_CONFIRM, CONFIRM_USER_NO,
                CONFIRM_LOGON_NO, model.Search_Date_From, model.Search_Date_To, model.Comments);


            return Json(new
            {
                message = "confirmed successfully"
            });

        }

        public JsonResult DcrDoDisburse(PublishDisburseModel model)
        {
            decimal? DISBURSE_USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? DISBURSE_LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            db.DCR_PUB_DO_DISBURSE(model.PUBLISH_NO, 1, DISBURSE_USER_NO, DISBURSE_LOGON_NO, model.Comments);

            return Json(new
            {
                message = "disbursed successfully"
            });
        }

        [HttpPost]
        public ActionResult DcrUserPublish(DcrPublishSearchModel searchModel)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            var publish_list = db.DCR_PUB_PUBLISH_LIST_GET(searchModel.Search_User, USER_NO,
                searchModel.Search_Date_From, searchModel.Search_Date_To, searchModel.Dcr_Date_From,
                searchModel.Dcr_Date_To, searchModel.Filter_Type).ToList();

            ViewBag.Filter_Type = searchModel.Filter_Type;

            return PartialView("_DcrUserPublishList", publish_list);

        }

        public ActionResult AccountsPanel()
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            DcrPublishSearchModel searchModel = new DcrPublishSearchModel();

            if (!string.IsNullOrEmpty(Request.QueryString["Search_User"]))
            {
                searchModel.Search_User = decimal.Parse(Request.QueryString["Search_User"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_From"]))
            {
                searchModel.Search_Date_From = DateTime.Parse(Request.QueryString["Search_Date_From"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_To"]))
            {
                searchModel.Search_Date_To = DateTime.Parse(Request.QueryString["Search_Date_To"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Search_pubno"]))
            {
                searchModel.Search_pubno = decimal.Parse(Request.QueryString["Search_pubno"]);
            }


            ViewBag.Search_DcrPublish_Model = searchModel;


            var accounts_list = db.DCR_PUB_ACCOUNTS_GET(searchModel.Search_User, USER_NO, searchModel.Search_pubno,
                searchModel.Search_Date_From, searchModel.Search_Date_To).ToList();

            return View(accounts_list);
        }

        [HttpPost]
        public ActionResult AccountsPanel(DcrPublishSearchModel searchModel)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            var accounts_list = db.DCR_PUB_ACCOUNTS_GET(searchModel.Search_User, USER_NO, searchModel.Search_pubno,
                searchModel.Search_Date_From, searchModel.Search_Date_To).ToList();

            TRN_USER_PUBLISH publish = db.TRN_USER_PUBLISH
                .Where(p => p.PUBLISH_NO == searchModel.Search_pubno).FirstOrDefault();
            ViewBag.Conf_Count = accounts_list.Count;
            ViewBag.Publish = publish;


            return PartialView("_AccountsPanelResult", accounts_list);
        }



        public ActionResult AccountsExportExcel(int id = 0)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;


            TRN_USER_PUBLISH publish = db.TRN_USER_PUBLISH
                .Where(p => p.PUBLISH_NO == id).FirstOrDefault();

            var accounts_list = db.DCR_PUB_ACCOUNTS_GET(null, USER_NO, id,
                null, null).ToList();


            DataTable accounts_table = new DataTable("test1");

            accounts_table.Columns.Add("User Name", typeof(string));
            accounts_table.Columns.Add("User Full Name", typeof(string));
            accounts_table.Columns.Add("User Mobile", typeof(string));
            accounts_table.Columns.Add("Division Name", typeof(string));
            accounts_table.Columns.Add("Zilla Name", typeof(string));
            accounts_table.Columns.Add("Thana Name", typeof(string));
            accounts_table.Columns.Add("Desig Name", typeof(string));
            accounts_table.Columns.Add("Dept Name", typeof(string));
            accounts_table.Columns.Add("User No", typeof(string));
            accounts_table.Columns.Add("Total DCR", typeof(int));
            accounts_table.Columns.Add("Total DER", typeof(int));

            foreach (var item in accounts_list)
            {
                accounts_table.Rows.Add(item.USER_NAME, item.USER_FULL_NAME, item.USER_MOBILE, item.DIVISION_NAME,
                    item.ZILLA_NAME, item.THANA_NAME, item.DESIG_NAME, item.DEPT_NAME, item.USER_NO, item.DCR_AMT, item.DER_AMT);


            }


            GridView gv = new GridView();
            gv.DataSource = accounts_table;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename="+ publish.PUBLISH_NO +"-PublishedList.xls");
            Response.ContentType = "application/vnd.ms-excel";
            //Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            ViewBag.Conf_Count = accounts_list.Count;
            ViewBag.Publish = publish;

            return PartialView("_AccountsPanelResult", accounts_list);
        }



        public JsonResult ValidateHtmlDownload()
        {
            decimal? user_no = null, pub_no = null;
            if (!string.IsNullOrEmpty(Request.QueryString["Search_User"]))
            {
                user_no = decimal.Parse(Request.QueryString["Search_User"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Search_pubno"]))
            {
                pub_no = decimal.Parse(Request.QueryString["Search_pubno"]);
            }

            if (!user_no.HasValue || !pub_no.HasValue)
            {
                return null;
            }
            SEC_USERS sec_user = db.SEC_USERS.Where(u => u.USER_NO == user_no).FirstOrDefault();


            string path = System.Configuration.ConfigurationManager.AppSettings["PUBLISH_EXPORT_URL"]
               + "\\" + pub_no + "\\" + pub_no + "_" + sec_user.USER_NAME + ".html";

            string status = "false";
            if (System.IO.File.Exists(path))
            {
                status = "true";

            }


            return Json(new { status = status, message = "HTML is not Generated Yet... \nPlease Generate HTML for this Publish" });

        }


        public FileContentResult Download()
        {
            decimal? user_no = null, pub_no = null;
            if (!string.IsNullOrEmpty(Request.QueryString["Search_User"]))
            {
                user_no = decimal.Parse(Request.QueryString["Search_User"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Search_pubno"]))
            {
                pub_no = decimal.Parse(Request.QueryString["Search_pubno"]);
            }

            if (!user_no.HasValue || !pub_no.HasValue)
            {
                return null;
            }
            SEC_USERS sec_user = db.SEC_USERS.Where(u => u.USER_NO == user_no).FirstOrDefault();



            string path = System.Configuration.ConfigurationManager.AppSettings["PUBLISH_EXPORT_URL"]
                + "\\" + pub_no + "\\" + pub_no + "_" + sec_user.USER_NAME + ".html";


            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string fileName = pub_no + "_" + sec_user.USER_NAME + ".html";


            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ZipResult DownloadZip()
        {
            decimal? pub_no = null;

            if (!string.IsNullOrEmpty(Request.QueryString["Search_pubno"]))
            {
                pub_no = decimal.Parse(Request.QueryString["Search_pubno"]);
            }

            if (!pub_no.HasValue)
            {
                return new ZipResult(null);
            }

            string directory_path = System.Configuration.ConfigurationManager
                .AppSettings["PUBLISH_EXPORT_URL"] + "\\" + pub_no;

            string[] files = null;

            List<string> zip = new List<string>();

            if (Directory.Exists(directory_path))
            {
                files = Directory.GetFiles(directory_path);

                foreach (string filename in files)
                {
                    zip.Add(filename);
                }

                return new ZipResult(zip, "Publish-Download-No-" + pub_no);
            }

            return new ZipResult(null);
        }
    }
}
