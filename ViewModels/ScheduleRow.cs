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
                throw new ArgumentNullException(nameof(table));
            }
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            this.Table = table;
            this.Title = title;
            this.Items = items;
        }

        public ScheduleTable Table { get; }

        public string Title { get; }

        public IList<ScheduleItem> Items { get; }

        public string GetRowId()
        {
            string tableTitle = this.Table.Title;

            string key = $"{tableTitle}-{this.Title}".ToLowerInvariant();

            string encodedKey = HttpUtility.UrlEncode(key) ?? string.Empty;
            encodedKey = HttpUtility.HtmlAttributeEncode(encodedKey);

            return encodedKey;
        }

        public static IEnumerable<ScheduleRow> FromTable(ScheduleTable table, CQ tableElement)
        {
            var groupedItems = FromTable(tableElement).GroupBy(x => new { x.StartTime, x.EndTime }).ToList();

            foreach (var group in groupedItems)
            {
                var items = group.GroupBy(x => x.Title).Select(x => x.First()).ToList();

                string title = $"{group.Key.StartTime.ToString(@"hh\:mm")} - {group.Key.EndTime.ToString(@"hh\:mm")}";

                yield return new ScheduleRow(table, title, items);
            }
        }

        private static IEnumerable<ScheduleItem> FromTable(CQ tableElement)
        {
            var itemElements = tableElement.Find("tbody tr .event .event_container");

            for (int i = 0; i < itemElements.Length; i++)
            {
                var item = itemElements.Eq(i);

                yield return ScheduleItem.Parse(item);
            }
        }
    }
}