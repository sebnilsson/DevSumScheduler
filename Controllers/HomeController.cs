using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Caching;
using System.Web.Mvc;

using DevSumScheduler.ViewModels;
using HtmlAgilityPack;

namespace DevSumScheduler.Controllers {
    public class HomeController : Controller {
        private const string devSumUrl = "http://devsum.se/schema/";

        public ActionResult Index() {
            var scheduleTables = GetScheduleTables();

            return View(scheduleTables);
        }

        private IList<ScheduleTable> GetScheduleTables() {
            string cacheKey = "DevSumScheduler.Controllers.HomeController.GetScheduleTables";

            var scheduleTables = HttpContext.Cache[cacheKey] as IList<ScheduleTable>;
            if(scheduleTables == null) {
                scheduleTables = ParseScheduleTables().ToList();

                HttpContext.Cache.Add(cacheKey, scheduleTables, null, DateTime.Now.AddHours(1),
                    Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }
            
            return scheduleTables;
        }

        private IEnumerable<ScheduleTable> ParseScheduleTables() {
            var client = new WebClient() {
                Encoding = System.Text.Encoding.UTF8
            };
            string html = client.DownloadString(devSumUrl);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var schemaTable = htmlDocument.DocumentNode.SelectNodes("//table[@class='schema_table']");

            foreach(var table in schemaTable) {
                var headerItems = table.SelectNodes("thead/tr/td");

                var itemsRows = table.SelectNodes("tbody/tr");

                var items = ParseItem(itemsRows).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                
                yield return new ScheduleTable {
                    Headers = headerItems.Skip(1).Select(item => item.InnerText),
                    Rows = items,
                    Title = headerItems.First().InnerText,
                };
            }
        }

        private IEnumerable<KeyValuePair<string, IEnumerable<ScheduleItem>>> ParseItem(HtmlNodeCollection itemsRows) {
            foreach(var row in itemsRows) {
                var time = row.SelectSingleNode("th").InnerText;
                var tdTables = row.SelectNodes("td/table");

                var result = from tdTable in tdTables //.SelectSingleNode("table").ChildNodes
                             let title = tdTable.SelectSingleNode("tr[@class='top']/td/a").InnerText
                             let speakerNode = tdTable.SelectSingleNode("tr[@class='bottom']/td/a")
                             let speaker = speakerNode.InnerText
                             let speakerUrl = speakerNode.Attributes["href"].Value
                             select new ScheduleItem {
                                 Speaker = speaker,
                                 SpeakerUrl = speakerUrl,
                                 Title = title,
                             };

                yield return new KeyValuePair<string, IEnumerable<ScheduleItem>>(time, result);
            }
        }

        public ActionResult About() {
            ViewBag.Title = "Om";
            return View();
        }
    }
}
