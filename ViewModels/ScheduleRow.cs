using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CsQuery;

namespace DevSumScheduler.ViewModels
{
    public class ScheduleRow
    {
        public ScheduleRow(ScheduleTable table, string title, IList<ScheduleItem> items)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            this.Table = table;
            this.Label = title;
            this.Items = items;
        }

        public ScheduleTable Table { get; private set; }

        public string Label { get; private set; }

        public IList<ScheduleItem> Items { get; private set; }

        public string GetRowId()
        {
            string tableTitle = this.Table.Title;

            string key = string.Format("{0}-{1}", tableTitle, this.Label).ToLowerInvariant();
            return HttpUtility.HtmlAttributeEncode(key);
        }

        public static IEnumerable<ScheduleRow> FromTable(ScheduleTable table, CQ tableElement)
        {
            var rowElements = tableElement.Find("tbody tr");

            for (int i = 0; i < rowElements.Length; i++)
            {
                var rowElement = rowElements.Eq(i);

                var colElements = rowElement.Find("td");

                if (colElements.Length < 5)
                {
                    continue;
                }

                var firstColEl = colElements.Eq(1);

                string startTime = (firstColEl.Find(".top_hour span").Text() ?? string.Empty).Trim();
                string endTime = (firstColEl.Find(".bottom_hour span").Text() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                {
                    continue;
                }

                string timeText = string.Format("{0} - {1}", startTime, endTime);

                var items = ScheduleItem.FromColumns(colElements).ToList();

                var row = new ScheduleRow(table, timeText, items);
                yield return row;
            }
        }
    }
}