using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

namespace DevSumScheduler.ViewModels
{
    public class ScheduleRow
    {
        public ScheduleRow(string timeText, ICollection<ScheduleItem> items)
        {
            this.TimeText = timeText;
            this.Items = items;
        }

        public string TimeText { get; private set; }

        public ICollection<ScheduleItem> Items { get; private set; }

        public bool HasMultipleItems
        {
            get
            {
                return this.Items.Count > 1;
            }
        }

        public ScheduleItem GetItemByLocation(string location)
        {
            return this.Items.FirstOrDefault(x => x.Location.Equals(location, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<ScheduleRow> FromHtmlNodes(ICollection<HtmlNode> rowNodes)
        {
            foreach (var htmlNode in rowNodes)
            {
                var timeEl = htmlNode.SelectSingleNode("strong");
                string timeText = (timeEl != null) ? timeEl.InnerText : null;

                var items = ScheduleItem.FromHtmlNode(htmlNode).ToList();

                yield return new ScheduleRow(timeText, items);
            }
        }
    }
}