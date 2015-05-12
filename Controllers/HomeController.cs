using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Caching;
using System.Web.Mvc;

using DevSumScheduler.ViewModels;

namespace DevSumScheduler.Controllers
{
    [Route("{action=index}", Name = "Home")]
    public class HomeController : Controller
    {
        private static readonly IEnumerable<string> ScheduleTableUrls = new[]
                                                                            {
                                                                                "http://www.devsum.se/dag-1-25-maj/",
                                                                                "http://www.devsum.se/dag-2-26-maj/"
                                                                            };

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

        private static readonly object ScheduleTablesLock = new object();

        private ICollection<ScheduleTable> GetScheduleTables()
        {
            var cachedScheduleTables = this.GetCachedScheduleTables();

            if (cachedScheduleTables == null)
            {
                lock (ScheduleTablesLock)
                {
                    cachedScheduleTables = this.GetCachedScheduleTables();

                    if (cachedScheduleTables == null)
                    {
                        cachedScheduleTables = ParseScheduleTables();

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
                }
            }

            return cachedScheduleTables;
        }

        private ICollection<ScheduleTable> GetCachedScheduleTables()
        {
            var cachedScheduleTables = this.HttpContext.Cache[ScheduleTablesCacheKey] as ICollection<ScheduleTable>;
            return cachedScheduleTables;
        }

        private static ICollection<ScheduleTable> ParseScheduleTables()
        {
            try
            {
                var scheduleTables = ParseScheduleTablesInternal();
                return scheduleTables.ToList();
            }
            catch (Exception)
            {
                return new ScheduleTable[0];
            }
        }

        private static IEnumerable<ScheduleTable> ParseScheduleTablesInternal()
        {
            foreach (var scheduleTableUrl in ScheduleTableUrls)
            {
                string html = GetScheduleTableHtml(scheduleTableUrl);

                var scheduleTables = ScheduleTable.ParseHtml(html);
                foreach (var scheduleTable in scheduleTables)
                {
                    yield return scheduleTable;
                }
            }
        }

        private static string GetScheduleTableHtml(string url)
        {
            using (var client = new HttpClient())
            {
                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var response = responseTask.Result;

                var contentTask = response.Content.ReadAsStringAsync();
                contentTask.Wait();

                string html = contentTask.Result;
                return html;
            }
        }
    }
}