using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Caching;
using System.Web.Mvc;

using DevSumScheduler.ViewModels;

namespace DevSumScheduler.Controllers
{
    public class HomeController : Controller
    {
        private const string DevSumScheduleUrl = "http://devsum.se/schema/";

        private const string ScheduleTablesCacheKey = "DevSumScheduler.Controllers.HomeController.GetScheduleTables";

        public ActionResult Index()
        {
            var scheduleTables = GetScheduleTables();
            if (scheduleTables == null)
            {
                return View("NoData");
            }

            return View(scheduleTables);
        }

        private IList<ScheduleTable> GetScheduleTables()
        {
            var cachedScheduleTables = HttpContext.Cache[ScheduleTablesCacheKey] as IList<ScheduleTable>;

            if (cachedScheduleTables == null)
            {
                cachedScheduleTables = (ParseScheduleTables() ?? Enumerable.Empty<ScheduleTable>()).ToList();
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
                using (var client = new WebClient { Encoding = System.Text.Encoding.UTF8 })
                {
                    string html = client.DownloadString(DevSumScheduleUrl);
                    return ScheduleTable.ParseHtml(html);
                }
            }
            catch (Exception)
            {
                return Enumerable.Empty<ScheduleTable>();
            }
        }

        public ActionResult About()
        {
            ViewBag.Title = "About";
            return View();
        }
    }
}