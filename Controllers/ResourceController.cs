using System.Text;
using System.Web.Mvc;

namespace DevSumScheduler.Controllers {
    public class ResourceController : Controller {
        [OutputCache(VaryByParam="fileName", Duration=43200)]
        public ActionResult File(string fileName, string fileExtension) {
            string filePath = Server.MapPath(string.Format("~/Resources/{0}.{1}", fileName, fileExtension));
            var fileContent = System.IO.File.ReadAllText(filePath, Encoding.UTF8);

            return Content(fileContent, "text/cache-manifest");
        }
    }
}
