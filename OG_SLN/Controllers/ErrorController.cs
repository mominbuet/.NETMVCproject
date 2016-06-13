using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using System.Text;

namespace OG_SLN.Controllers
{
    public class ErrorController : Controller {

        private static string GetErrorMessage(Exception ex, bool includeStackTrace) {
            StringBuilder msg = new StringBuilder();
            BuildErrorMessage(ex, ref msg);
            if (includeStackTrace) {
                msg.Append("\n");
                msg.Append(ex.StackTrace);
            }
            return msg.ToString();
        }

        private static void BuildErrorMessage(Exception ex, ref StringBuilder msg) {
            if (ex != null) {
                msg.Append(ex.Message);
                msg.Append("\n");
                if (ex.InnerException != null) {
                    BuildErrorMessage(ex.InnerException, ref msg);
                }
            }
        }

        private void WriteErrorLog(string err_msg, Exception ex) {
            string file_path = ConfigurationManager.AppSettings["ERROR_APTH"];
            string file_name = DateTime.Today.ToString("yyyy_MM_dd");
            string full_error_path = string.Empty;

            try {
                full_error_path = file_path + file_name;

                if (!System.IO.File.Exists(full_error_path)) {
                    FileStream fs = System.IO.File.Create(full_error_path);
                    fs.Close();
                }

                StreamWriter sw = System.IO.File.AppendText(full_error_path);
                sw.WriteLine("\n");
                sw.WriteLine(" \n");
                sw.WriteLine("******************************************************************");

                sw.WriteLine("\nDATE: " + System.DateTime.Now);
                sw.WriteLine("\nMESSAGE: " + ex.Message);

                sw.WriteLine("\nSOURCE: " + ex.Source);
                sw.WriteLine("\nINSTANCE: " + ex.InnerException);

                sw.WriteLine("\nDATA: " + ex.Data);
                sw.WriteLine("\nTARGETSITE: " + ex.TargetSite);

                sw.WriteLine("\nSTACKTRACE: " + ex.StackTrace + "\n");
                sw.WriteLine("\nERROR DETAILS: " + err_msg + "\n");

                sw.WriteLine("\n******************************************************************");
                sw.Close();

            } catch (Exception errEx) {

            } finally {

            }
        }


        public ActionResult Index() {
            /*
            Exception ex = Server.GetLastError();
            HttpException httpException = ex as HttpException;

            this.WriteErrorLog(GetErrorMessage(ex, true), ex);
            */
            return View("Error");
        }
        public ViewResult NotFound() {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("NotFound");
        }

        public ViewResult ServerBusy() {
            Response.StatusCode = 503;  //you may want to set this to 200
            return View("ServerBusy");
        }

        /*
        public ActionResult RaiseError() {
            Exception myEx = new Exception("1st Error");
            myEx.Source = "RaiseError";

            throw myEx;
        }
        */

    }
}
