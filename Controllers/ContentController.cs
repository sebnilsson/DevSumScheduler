using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

using SquishIt.Framework;

namespace DevSumScheduler.Controllers
{
    public class ContentController : Controller
    {
        private const string CssContentType = "text/css";

        private const string JsContentType = "application/javascript";

        [Route("content/min/{hash}/{filename}.css")]
        public ActionResult Css(string filename)
        {
            return this.GetWrappedBundleResult(
                () =>
                    {
                        var rendered = Bundle.Css().RenderCached(filename);

                        this.Response.ContentType = CssContentType;
                        return this.Content(rendered);
                    });
        }

        [Route("content/min/{hash?}/{filename}.js")]
        public ActionResult Js(string filename)
        {
            return this.GetWrappedBundleResult(
                () =>
                    {
                        var rendered = Bundle.JavaScript().RenderCached(filename);

                        this.Response.ContentType = JsContentType;
                        return this.Content(rendered);
                    });
        }

        [OutputCache(VaryByParam = "fileName", Duration = 43200)]
        public ActionResult Resource(string fileName, string fileExtension)
        {
            string filePath = Server.MapPath(string.Format("~/Resources/{0}.{1}", fileName, fileExtension));
            var fileContent = System.IO.File.ReadAllText(filePath, Encoding.UTF8);

            return Content(fileContent, "text/cache-manifest");
        }

        private ActionResult GetWrappedBundleResult(Func<ActionResult> actionFactory)
        {
            try
            {
                return actionFactory();
            }
            catch (KeyNotFoundException)
            {
                return this.HttpNotFound();
            }
        }
    }
}