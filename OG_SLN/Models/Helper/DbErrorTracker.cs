using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using System.Text;



namespace OG_SLN.Models {
    public class DbErrorTracker {
        static int errorCount=0;
        /*
        public string GetErrorMessage(Exception ex, bool includeStackTrace) {
            StringBuilder msg = new StringBuilder();
            BuildErrorMessage(ex, ref msg);
            if (includeStackTrace) {
                msg.Append("\n");
                msg.Append(ex.StackTrace);
            }
            return msg.ToString();
        }

        private void BuildErrorMessage(Exception ex, ref StringBuilder msg) {
            if (ex != null) {
                msg.Append(ex.Message);
                msg.Append("\n");
                if (ex.InnerException != null) {
                    BuildErrorMessage(ex.InnerException, ref msg);
                }
            }
        }

        public void WriteErrorLog(string err_msg, Exception ex) {
            string file_path = ConfigurationManager.AppSettings["DB_ERROR_PATH"];
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
        */


        public string GetErrorMessage(Exception ex, bool includeStackTrace) {
            StringBuilder msg = new StringBuilder();
            BuildErrorMessage(ex, ref msg);
            if (includeStackTrace) {
                msg.Append("\n");
                msg.Append(ex.StackTrace);
            }
            return msg.ToString();
        }

        private void BuildErrorMessage(Exception ex, ref StringBuilder msg) {
            if (ex != null) {
                msg.Append(ex.Message);
                msg.Append("\n");
                if (ex.InnerException != null) {
                    BuildErrorMessage(ex.InnerException, ref msg);
                }
            }
        }
        /*public void WriteErrorLog(string err_msg, Exception ex)
        {
            string file_path = ConfigurationManager.AppSettings["DB_ERROR_PATH"];
            string file_name = DateTime.Today.ToString("yyyy_MM_dd") + ".txt";
            string full_error_path = string.Empty;

            string controllerName = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller").Trim();
            string actionName = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action").Trim();

            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                full_error_path = HttpContext.Current.Server.MapPath(file_path + file_name);

                if (!File.Exists(full_error_path))
                {
                    fs = File.Create(full_error_path);
                    fs.Close();
                }

                sw = File.AppendText(full_error_path);
                sw.WriteLine("\n");
                sw.WriteLine(" \n");
                sw.WriteLine("******************************************************************");

                sw.WriteLine("\nDATE: " + System.DateTime.Now);

                if (controllerName != null)
                {
                    sw.WriteLine("\nCONTROLLER: " + controllerName);
                }
                if (actionName != null)
                {
                    sw.WriteLine("\nACTION: " + actionName);
                }
                
                if (HttpContext.Current.Session["sess_USER_NO"] != null)
                {
                    sw.WriteLine("\nUSER_NO: " + HttpContext.Current.Session["sess_USER_NO"]);
                }
                if (HttpContext.Current.Session["sess_LOGON_NO"] != null)
                {
                    sw.WriteLine("\nLOGON_HIS_NO: " + HttpContext.Current.Session["sess_LOGON_NO"]);
                }
                if (HttpContext.Current.Session["sess_sec_users"] != null)
                {
                    SEC_USERS user = HttpContext.Current.Session["sess_sec_users"] as SEC_USERS;
                    if ((user != null) && (!string.IsNullOrEmpty(user.USER_NAME)) && (!string.IsNullOrWhiteSpace(user.USER_NAME)))
                    {
                        sw.WriteLine("\nLOGON_NAME: " + user.USER_NAME); // + HttpContext.Current.Session["SEES_LOGON_NAME"]);
                    }
                }

                sw.WriteLine("\nMESSAGE: " + ex.Message);

                sw.WriteLine("\nSOURCE: " + ex.Source);
                sw.WriteLine("\nINSTANCE: " + ex.InnerException);

                sw.WriteLine("\nDATA: " + ex.Data);
                sw.WriteLine("\nTARGETSITE: " + ex.TargetSite);

                sw.WriteLine("\nSTACKTRACE: " + ex.StackTrace + "\n");
                sw.WriteLine("\nERROR DETAILS: " + err_msg + "\n");

                sw.WriteLine("\n******************************************************************");
                //sw.Close();

            }
            catch (Exception errEx)
            {

            }
            finally
            {
                sw.Close();
            }
        }
        */
        public void WriteErrorLog(string err_msg, Exception ex,String userName) {
            string file_path = ConfigurationManager.AppSettings["DB_ERROR_PATH"];
            string file_name = DateTime.Today.ToString("yyyy_MM_dd") + ".txt";
            string full_error_path = string.Empty;

            string controllerName = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller").Trim();
            string actionName = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action").Trim();

            FileStream fs = null;
            StreamWriter sw = null;

            try {
                full_error_path = HttpContext.Current.Server.MapPath(file_path + file_name);

                if (!File.Exists(full_error_path)) {
                    fs = File.Create(full_error_path);
                    fs.Close();
                }

                sw = File.AppendText(full_error_path);
                sw.WriteLine(" \n");
                sw.WriteLine("*********************************" + ++errorCount + "*********************************");

                sw.WriteLine("\nDATE: " + System.DateTime.Now);

                if (controllerName != null) {
                    sw.WriteLine("\nCONTROLLER: " + controllerName);
                }
                if (actionName != null) {
                    sw.WriteLine("\nACTION: " + actionName);
                }
                if (userName != null)
                {
                    sw.WriteLine("\nUSer: " + userName);
                }

                if (HttpContext.Current.Session["sess_USER_NO"] != null) {
                    sw.WriteLine("\nUSER_NO: " + HttpContext.Current.Session["sess_USER_NO"]);
                }
                if (HttpContext.Current.Session["sess_LOGON_NO"] != null) {
                    sw.WriteLine("\nLOGON_HIS_NO: " + HttpContext.Current.Session["sess_LOGON_NO"]);
                }
                if (HttpContext.Current.Session["sess_sec_users"] != null) {
                    SEC_USERS user = HttpContext.Current.Session["sess_sec_users"] as SEC_USERS;
                    if ((user != null) && (!string.IsNullOrEmpty(user.USER_NAME)) && (!string.IsNullOrWhiteSpace(user.USER_NAME))) {
                        sw.WriteLine("\nLOGON_NAME: " + user.USER_NAME); // + HttpContext.Current.Session["SEES_LOGON_NAME"]);
                    }
                }

                sw.WriteLine("\nMESSAGE: " + ex.Message);

                sw.WriteLine("\nSOURCE: " + ex.Source);
                sw.WriteLine("\nInnerException: " + ex.InnerException);

                sw.WriteLine("\nDATA: " + ex.Data);
                sw.WriteLine("\nTARGETSITE: " + ex.TargetSite);

                sw.WriteLine("\nSTACKTRACE: " + ex.StackTrace + "\n");
                sw.WriteLine("\nERROR DETAILS: " + err_msg + "\n");

                sw.WriteLine("\n******************************************************************");
                //sw.Close();

            } catch (Exception errEx) {

            } finally {
                sw.Close();
            }
        }

    }
}