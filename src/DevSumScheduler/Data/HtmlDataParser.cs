using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using CsQuery;
using CsQuery.ExtensionMethods;

namespace DevSumScheduler.Data
{
    internal static class HtmlDataParser
    {
        private static readonly Regex SlugRegex =
            new Regex(@"https?:\/\/(?:www.)?devsum.se\/speaker\/([^\/]+)\/?",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static Day GetDay(string data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var dom = CQ.CreateDocument(data);

            var title = ParseTitle(dom);
            var locations = ParseLocations(dom).ToList();
            var sessions = ParseSessions(dom, locations).ToList();

            return new Day(sessions, locations, title);
        }

        private static IEnumerable<string> ParseLocations(CQ dom)
        {
            var headers = dom["#all-events table.tt_timetable thead tr th"];

            return headers.Elements.Skip(1).Select(x => x.InnerText).ToList();
        }

        private static IEnumerable<Session> ParseSessions(CQ dom, IReadOnlyCollection<string> locations)
        {
            var items = dom["#all-events table.tt_timetable tbody .event_container"];

            foreach (var item in items.Elements)
            {
                var cell = item.NodeName.Equals("td", StringComparison.InvariantCultureIgnoreCase)
                    ? item
                    : item.ParentNode;
                var parent = cell.ParentNode;
                var colIndex = parent.ChildElements.IndexOf(cell) - 1;

                yield return ParseSession(item, colIndex, locations);
            }
        }

        private static Session ParseSession(IDomObject item, int colIndex, IReadOnlyCollection<string> locations)
        {
            var itemElement = CQ.Create(item);

            var location = locations.Any() && colIndex >= 0 ? locations.ElementAtOrDefault(colIndex) : null;

            var start = itemElement[".top_hour .hours"].Text();
            var end = itemElement[".bottom_hour .hours"].Text();

            var startsAt = DateTime.ParseExact(start, "HH.mm", CultureInfo.InvariantCulture).TimeOfDay;
            var endsAt = DateTime.ParseExact(end, "HH.mm", CultureInfo.InvariantCulture).TimeOfDay;

            var header = itemElement[".event_header"];

            var title = header.Text();

            var speakerTitle = itemElement[".before_hour_text"].Text();
            var speakerSlug = GetSpeakerSlug(header.Attr("href"));

            return new Session(title, location, startsAt, endsAt, speakerTitle, speakerSlug);
        }

        private static string GetSpeakerSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var match = SlugRegex.Match(text);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private static string ParseTitle(CQ dom)
        {
            return dom["meta[itemprop='name']"].Attr("content");
        }
    }
}