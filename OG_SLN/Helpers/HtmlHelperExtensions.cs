using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString InstituteDialogFormLink(this HtmlHelper helper, string linkText, 
            string dialogContentUrl, string dialogTitle)
        {
            string output = String.Empty;

            
            TagBuilder input = new TagBuilder("input");

            input.Attributes.Add("id", "Selected_Institute");
            input.Attributes.Add("name", "Selected_Institute");
            
            TagBuilder builder = new TagBuilder("a");

            builder.SetInnerText(linkText);

            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);

            builder.AddCssClass("dialogLink btn btn-primary");

            output = input.ToString() + builder.ToString();

            return new MvcHtmlString(output);
        }

        public static MvcHtmlString InstituteAssignedDialogFormLink(this HtmlHelper helper, string linkText,
            string dialogContentUrl, string dialogTitle)
        {
            string output = String.Empty;


            TagBuilder input = new TagBuilder("input");

            input.Attributes.Add("id", "Selected_Institute");
            input.Attributes.Add("name", "Selected_Institute");

            TagBuilder builder = new TagBuilder("a");

            builder.SetInnerText(linkText);

            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);

            builder.AddCssClass("dialogLink btn btn-primary");

            output = input.ToString() + builder.ToString();

            return new MvcHtmlString(output);
        }

        public static MvcHtmlString ZonalManagerDialogFormLink(this HtmlHelper helper, string linkText,
            string dialogContentUrl, string dialogTitle)
        { 
            string output = String.Empty;


            TagBuilder input = new TagBuilder("input");

            input.Attributes.Add("id", "Selected_Zonal");
            input.Attributes.Add("name", "Selected_Zonal");
            input.Attributes.Add("class", "form-control");
            TagBuilder builder = new TagBuilder("a");

            builder.SetInnerText(linkText);

            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);

            builder.AddCssClass("btn btn-primary dialogLink ");

            output = input.ToString() + builder.ToString();

            return new MvcHtmlString(output);
        }

        public static MvcHtmlString ReportingAuthorityDialogFormLink(this HtmlHelper helper, string linkText,
            string dialogContentUrl, string dialogTitle)
        {
            string output = String.Empty;


            TagBuilder input = new TagBuilder("input");

            input.Attributes.Add("id", "Selected_Authority");
            input.Attributes.Add("name", "Selected_Authority");
            input.Attributes.Add("class", "form-control");
            TagBuilder builder = new TagBuilder("a");

            builder.SetInnerText(linkText);

            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);

            builder.AddCssClass("dialogLink btn btn-primary");

            output = input.ToString() + builder.ToString();

            return new MvcHtmlString(output);
        }

        public static MvcHtmlString ThousandConvert(this HtmlHelper helper, long amount)
        {
            string amount_str = String.Format("{0:###,##,##,##0.##}", amount);

            return new MvcHtmlString(amount_str);
        }

        public static IDisposable BeginCollectionItem<TModel>(this HtmlHelper<TModel> html, 
            string collectionName, string id)
        {
            //string itemIndex = Guid.NewGuid().ToString();
            string itemIndex = id;
            string collectionItemName = String.Format("{0}[{1}]", collectionName, itemIndex);

            TagBuilder indexField = new TagBuilder("input");
            indexField.MergeAttributes(new Dictionary<string, string>() { 
                {"name", String.Format("{0}.Index", collectionName)},
                {"value", itemIndex},
                {"type", "hidden"},
                {"autocomplete", "off"}
            });

            html.ViewContext.Writer.WriteLine(indexField.ToString(TagRenderMode.SelfClosing));
            return new CollectionItemNamePrefixScope(html.ViewData.TemplateInfo, collectionItemName);
        }

        private class CollectionItemNamePrefixScope : IDisposable
        {
            private readonly TemplateInfo _templateInfo;
            private readonly string _previousPrefix;

            public CollectionItemNamePrefixScope(TemplateInfo templateInfo, string collectionItemName)
            {
                this._templateInfo = templateInfo;

                _previousPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = collectionItemName;
            }

            public void Dispose()
            {
                _templateInfo.HtmlFieldPrefix = _previousPrefix;
            }
        }
    }
}