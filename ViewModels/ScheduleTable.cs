using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using CsQuery;

namespace DevSumScheduler.ViewModels
{
    public class ScheduleTable
    {
        public ScheduleTable(string title, IEnumerable<string> headers)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            this.Title = GetShortTitle(title);

            this.Headers = headers.ToList();
            this.Rows = new List<ScheduleRow>();
        }

        public string Title { get; }

        public ICollection<string> Headers { get; }

        public ICollection<ScheduleRow> Rows { get; }

        public static IEnumerable<ScheduleTable> ParseHtml(string pageHtml)
        {
            if (string.IsNullOrWhiteSpace(pageHtml))
            {
                yield break;
            }

            var pageQuery = CQ.Create(pageHtml);

            var scheduleDays = GetScheduleTableDays(pageQuery).ToList();

            foreach (var scheduleDay in scheduleDays)
            {
                var scheduleTable = FromScheduleDay(scheduleDay);
                yield return scheduleTable;
            }
        }

        private static IEnumerable<ScheduleTableDay> GetScheduleTableDays(CQ pageQuery)
        {
            var titleElement = pageQuery.Find(".gdlr-item-title");

            var tableElement = pageQuery.Find("#all-events table.tt_timetable");

            yield return new ScheduleTableDay { TitleElement = titleElement, TableElement = tableElement };
        }

        private static string GetShortTitle(string title)
        {
            title = title ?? string.Empty;

            var separatorIndex = title.LastIndexOf(",", StringComparison.OrdinalIgnoreCase);

            string shortTitle = (separatorIndex > 0) ? title.Substring(0, separatorIndex) : title;
            return shortTitle;
        }

        private static ScheduleTable FromScheduleDay(ScheduleTableDay scheduleTableDay)
        {
            string title = scheduleTableDay.TitleElement.Text();

            var headers = GetHeaders(scheduleTableDay.TableElement).ToList();

            var scheduleTable = new ScheduleTable(title, headers);

            var rows = ScheduleRow.FromTable(scheduleTable, scheduleTableDay.TableElement).ToList();
            foreach (var row in rows)
            {
                scheduleTable.Rows.Add(row);
            }

            return scheduleTable;
        }

        private static IEnumerable<string> GetHeaders(CQ tableElement)
        {
            var headers = tableElement.Find("thead tr th").Skip(1).ToList();

            var headerTexts = headers.Select(x => WebUtility.HtmlDecode(x.InnerText ?? string.Empty).Trim());

            return headerTexts;
        }

        private class ScheduleTableDay
        {
            public CQ TitleElement { get; set; }

            public CQ TableElement { get; set; }
        }
    }
}