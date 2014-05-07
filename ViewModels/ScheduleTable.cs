using System.Collections.Generic;
using System.Linq;
using System.Web;

using HtmlAgilityPack;

namespace DevSumScheduler.ViewModels
{
    public class ScheduleTable
    {
        public ScheduleTable(string title, ICollection<ScheduleRow> rows)
        {
            this.Title = title;
            this.Rows = rows;

            var multipleItemRows = this.Rows.Where(x => x.HasMultipleItems).ToList();

            this.Headers = multipleItemRows.SelectMany(x => x.Items).Select(x => x.Location).Distinct().ToList();
        }

        public string Title { get; private set; }

        public ICollection<string> Headers { get; private set; }

        public ICollection<ScheduleRow> Rows { get; private set; }
        
        public string GetRowId(ScheduleRow row)
        {
            string key = string.Concat(this.Title, "-", row.TimeText).ToLowerInvariant();
            return HttpUtility.HtmlAttributeEncode(key);
        }

        public static IEnumerable<ScheduleTable> ParseHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                yield break;
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var scheduleEls = htmlDocument.DocumentNode.SelectNodes("//dl[@class='gk-schedule']/dt | //dl[@class='gk-schedule']/dd");

            var scheduleDays = GetDays(scheduleEls).ToList();

            foreach(var day in scheduleDays)
            {
                yield return FromHtmlNodes(day.Key, day.Value);
            }
        }

        private static IEnumerable<KeyValuePair<HtmlNode, ICollection<HtmlNode>>> GetDays(IList<HtmlNode> scheduleEls)
        {
            var headerEls = scheduleEls.Where(x => x.Name == "dt").ToList();

            foreach (var headerEl in headerEls)
            {
                int startIndex = scheduleEls.IndexOf(headerEl);

                var nextHeaderEl = scheduleEls.Skip(startIndex + 1).FirstOrDefault(x => x.Name == "dt");
                int endIndex = (nextHeaderEl != null) ? scheduleEls.IndexOf(nextHeaderEl) : scheduleEls.Count;

                var items = scheduleEls.Skip(startIndex + 1).Take(endIndex - startIndex - 1).ToList();

                yield return new KeyValuePair<HtmlNode, ICollection<HtmlNode>>(headerEl, items);
            }
        }

        private static ScheduleTable FromHtmlNodes(HtmlNode headerNode, ICollection<HtmlNode> rowNodes)
        {
            string title = headerNode.InnerText;

            var items = ScheduleRow.FromHtmlNodes(rowNodes).ToList();

            return new ScheduleTable(title, items);
        }
    }
}