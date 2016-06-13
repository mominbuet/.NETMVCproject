using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

namespace OG_SLN.Controllers.WebApi
{
    public class MobileLogger
    {
        static int logcount = 0;
        public static void logDefault(String inp, string filename)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                string file_path = ConfigurationManager.AppSettings["APP_LOG"];
                string file_name = filename + "_" + DateTime.Today.ToString("yyyy_MM_dd") + ".txt";
                string full_error_path = string.Empty;



                full_error_path = HttpContext.Current.Server.MapPath(file_path + file_name);

                if (!File.Exists(full_error_path))
                {
                    fs = File.Create(full_error_path);
                    fs.Close();
                }

                sw = File.AppendText(full_error_path);
                sw.WriteLine("\n " + DateTime.Now.ToString("hh:mm:ss") + "  " + inp);


            }
            catch (Exception errEx)
            {

            }
            finally
            {
                sw.Close();
            }
        }
        public static void logLogin(SEC_USERS sec_users, decimal? LAST_PK_VAL)
        {
            string file_path = ConfigurationManager.AppSettings["APP_LOG"];
            string file_name = "APP_" + DateTime.Today.ToString("yyyy_MM_dd") + ".txt";
            string full_error_path = string.Empty;

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
                sw.WriteLine(" \n");
                sw.WriteLine("*********************************" + ++logcount + "*********************************");

                sw.WriteLine("\nDATE: " + System.DateTime.Now);
                sw.WriteLine("\nLogging User no: " + sec_users.USER_NO);
                sw.WriteLine("\t User Name: " + sec_users.USER_NAME);

                sw.WriteLine("\n " + DateTime.Now.ToShortTimeString() + " " + ((LAST_PK_VAL == null) ? "PK val not found" : "PK val " + LAST_PK_VAL.ToString()));


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
    }
}