using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ionic.Zip;

namespace OG_SLN.Utils
{
    public class ZipResult : ActionResult
    {
        private IEnumerable<string> _files;
        private string _fileName;

        public string FileName
        {
            get
            {
                return _fileName ?? "uploaded_attachments.zip";
            }
            set
            {
                _fileName = value;
            }
        }

        public ZipResult(params string[] files)
        {
            this._files = files;
        }

        public ZipResult(IEnumerable<string> files, string filename)
        {
            this._fileName = filename + ".zip";
            this._files = files;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            ZipFile zip = new ZipFile();

            if (_files != null)
            {
                zip.AddFiles(_files, false, "");
                context.HttpContext.Response.ContentType = "application/zip";
                context.HttpContext.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                zip.Save(context.HttpContext.Response.OutputStream);
            }
            else
            {
                context.HttpContext.Response.ContentType = "application/zip";
                context.HttpContext.Response.AppendHeader("Content-Disposition", "attachment; filename=" + 
                    "Your-ip-is-not-allowed-to-download.zip");
                zip.Save(context.HttpContext.Response.OutputStream);
            }
        }
    }
}