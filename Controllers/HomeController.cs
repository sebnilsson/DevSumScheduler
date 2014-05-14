using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;

using DevSumScheduler.ViewModels;

namespace DevSumScheduler.Controllers
{
    [Route("{action=index}", Name = "Home")]
    public class HomeController : Controller
    {
        private const string DevSumScheduleUrl = "http://devsum.se/schema/";

        private const string ScheduleTablesCacheKey = "DevSumScheduler.Controllers.HomeController.GetScheduleTables";

        public ActionResult Index()
        {
            var scheduleTables = this.GetScheduleTables();
            if (scheduleTables == null)
            {
                return View("NoData");
            }

            return View(scheduleTables);
        }

        public ActionResult About()
        {
            ViewBag.Title = "About";
            return View();
        }

        private IList<ScheduleTable> GetScheduleTables()
        {
            var cachedScheduleTables = this.HttpContext.Cache[ScheduleTablesCacheKey] as IList<ScheduleTable>;

            if (cachedScheduleTables == null)
            {
                var parseScheduleTablesTask = ParseScheduleTables();

                cachedScheduleTables = (parseScheduleTablesTask ?? Enumerable.Empty<ScheduleTable>()).ToList();
                if (!cachedScheduleTables.Any())
                {
                    return null;
                }

                HttpContext.Cache.Add(
                    ScheduleTablesCacheKey,
                    cachedScheduleTables,
                    null,
                    DateTime.Now.AddHours(1),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.High,
                    null);
            }

            return cachedScheduleTables;
        }

        private static IEnumerable<ScheduleTable> ParseScheduleTables()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var responseTask = client.GetAsync(DevSumScheduleUrl);
                    responseTask.Wait();

                    var response = responseTask.Result;

                    var contentTask = response.Content.ReadAsStringAsync();
                    contentTask.Wait();

                    string html = contentTask.Result;

                    return ScheduleTable.ParseHtml(html);
                }
            }
            catch (Exception)
            {
                return Enumerable.Empty<ScheduleTable>();
            }
        }
    }
}