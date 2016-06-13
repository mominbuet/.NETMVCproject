using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using OG_SLN.Controllers.WebApi;

namespace OG_SLN.Controllers
{
    public class WS_FileUploadsController : Controller
    {
        //
        // GET: /WS_FileUploads/
        public string test()
        {
            return "haas";
        }
        /*[HttpPost]
        public string UploadCrashReports2(HttpPostedFileBase file )
        {
            string file_path = ConfigurationManager.AppSettings["APP_LOG"];
            string full_error_path = System.Web.HttpContext.Current.Server.MapPath(file_path + "_Crash_" +
                DateTime.Today.ToString("yyyy_MM_dd") + ".txt");

            file.SaveAs(full_error_path);
            return "false";
        }*/
        [HttpPost]
        public string UploadDB()
        {
            string file_path = ConfigurationManager.AppSettings["DB_UPLOAD_URL"];
            try
            {

                if (!System.IO.Directory.Exists(file_path))
                {
                    System.IO.Directory.CreateDirectory(file_path);
                }
                HttpPostedFileBase file = Request.Files["file"];
                /*foreach (HttpPostedFile file in Request.Files)
                {*/
                //MobileLogger.logDefault("File length "+file.ContentLength+" name "+file.FileName, "dbuploaded");
                var fileName = Path.GetFileName(file.FileName);

                //var path = Path.Combine(file_path, fileName);
                file.SaveAs(file_path + fileName);
                //}
                return "was here";
            }
            catch (Exception errEx)
            {
                MobileLogger.logDefault("dbuploaded err " + errEx.Message, "dbuploaded");

            }

            return "false";

        }
        [HttpPost]
        public string UploadPrefs()
        {
            string file_path = ConfigurationManager.AppSettings["DB_UPLOAD_URL"];
            try
            {

                if (!System.IO.Directory.Exists(file_path))
                {
                    System.IO.Directory.CreateDirectory(file_path);
                }
                HttpPostedFileBase file = Request.Files["file"];
                /*foreach (HttpPostedFile file in Request.Files)
                {*/
                //MobileLogger.logDefault("File length "+file.ContentLength+" name "+file.FileName, "dbuploaded");
                var fileName = Path.GetFileName(file.FileName);

                //var path = Path.Combine(file_path, fileName);
                file.SaveAs(file_path + fileName);
                //}
                return "was here";
            }
            catch (Exception errEx)
            {
                MobileLogger.logDefault("dbuploaded err " + errEx.Message, "dbuploaded");

            }

            return "false";

        }
        [HttpPost]
        public void UplaodCrashAcra()
        {
            if (Request.HttpMethod == "POST")
            {
                string file_path = ConfigurationManager.AppSettings["APP_LOG"];
                string file_name = "_Crash_" + DateTime.Today.ToString("yyyy_MM_dd") + ".txt";
                string full_error_path = string.Empty;
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    full_error_path = System.Web.HttpContext.Current.Server.MapPath(file_path + file_name);

                    if (!System.IO.File.Exists(full_error_path))
                    {
                        fs = System.IO.File.Create(full_error_path);
                        fs.Close();
                    }
                    sw = System.IO.File.AppendText(full_error_path);
                    sw.WriteLine("\n***************************************\n");
                    foreach (string key in Request.Form)
                    {
                        
                        sw.WriteLine(key + ":" + Request.Form[key]);
                    }
                }
                catch (Exception errEx)
                {
                    MobileLogger.logDefault("crash called " +errEx.Message, "crasttest");
                }
                finally
                {
                    sw.Close();

                }
            }
        }
        [HttpPost]
        public string UploadCrashReports2()
        {
            // MobileLogger.logDefault("crash called", "crasttest");
            if (Request.ContentLength > 0)
            {
                string res = "";
                //var str = docbinaryarray.;
                int strLen = Convert.ToInt32(Request.ContentLength);
                Stream stream = Request.InputStream;
                //byte[] strArr =stream.;

                //str.Read(strArr, 0, strLen);
                //for (int i = 0; i < strLen; i++)
                byte[] buf;  // byte array

                buf = new byte[stream.Length];  //declare arraysize
                stream.Read(buf, 0, buf.Length);
                /*for (int i = 0; i < Request.ContentLength;i++ )
                    res += buf[i].ToString();*/
                buf = buf.Where(x => x != 0x00).ToArray(); // not sure this is OK with your requirements 
                res = System.Text.Encoding.ASCII.GetString(buf).Trim();
                //res = filename;
                //MobileLogger.logDefault("crash called","crasttest");

                string file_path = ConfigurationManager.AppSettings["APP_LOG"];
                string file_name = "_Crash_" + DateTime.Today.ToString("yyyy_MM_dd") + ".txt";
                string full_error_path = string.Empty;
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    full_error_path = System.Web.HttpContext.Current.Server.MapPath(file_path + file_name);

                    if (!System.IO.File.Exists(full_error_path))
                    {
                        fs = System.IO.File.Create(full_error_path);
                        fs.Close();
                    }

                    sw = System.IO.File.AppendText(full_error_path);
                    sw.WriteLine("\n***************************************\n" + res + "\n\n");


                }
                catch (Exception errEx)
                {
                    MobileLogger.logDefault("crash called", "crasttest");
                }
                finally
                {
                    sw.Close();

                }
                return "was here";
            }
            return "false";
        }
        [HttpPost]
        public string UploadCrashReports(HttpPostedFileBase filestream)
        {
            // MobileLogger.logDefault("crash called", "crasttest");
            if (filestream.ContentLength > 0)
            {
                string res = "";
                //var str = docbinaryarray.;
                int strLen = Convert.ToInt32(Request.ContentLength);
                //InputStrea
                byte[] strArr = null;
                using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                {
                    strArr = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                }

                //str.Read(strArr, 0, strLen);
                for (int i = 0; i < strLen; i++)
                    res += strArr[i].ToString();
                //res = filename;
                //MobileLogger.logDefault("crash called","crasttest");

                string file_path = ConfigurationManager.AppSettings["APP_LOG"];
                string file_name = "_Crash_" + DateTime.Today.ToString("yyyy_MM_dd") + ".txt";
                string full_error_path = string.Empty;
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    full_error_path = System.Web.HttpContext.Current.Server.MapPath(file_path + file_name);

                    if (!System.IO.File.Exists(full_error_path))
                    {
                        fs = System.IO.File.Create(full_error_path);
                        fs.Close();
                    }

                    sw = System.IO.File.AppendText(full_error_path);
                    sw.WriteLine("\n***************************************\n" + res + "\n\n");


                }
                catch (Exception errEx)
                {
                    MobileLogger.logDefault("crash called", "crasttest");
                }
                finally
                {
                    sw.Close();

                }
                return "was here";
            }
            return "false";
        }

    }
}
