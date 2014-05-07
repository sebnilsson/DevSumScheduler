using System.Collections.Generic;

using HtmlAgilityPack;

namespace DevSumScheduler.ViewModels
{
    public class ScheduleItem
    {
        public string Location { get; private set; }

        public string Speaker { get; private set; }

        public string SpeakerUrl { get; private set; }

        public string Title { get; private set; }

        public static IEnumerable<ScheduleItem> FromHtmlNode(HtmlNode htmlNode)
        {
            var itemEls = htmlNode.SelectNodes("div/div");
            foreach (var itemEl in itemEls)
            {
                yield return GetScheduleItem(itemEl);
            }
        }

        private static ScheduleItem GetScheduleItem(HtmlNode htmlNode)
        {
            var locationEl = htmlNode.SelectSingleNode("p/strong");
            string location = (locationEl != null) ? (locationEl.InnerText ?? string.Empty).Trim() : null;

            var infoEl = htmlNode.SelectSingleNode("p[position()>1]");
            var titleEl = (infoEl != null) ? infoEl.SelectSingleNode("a") : null;
            var speakerEl = (infoEl != null) ? infoEl.SelectSingleNode("small") : null;

            string title = (titleEl != null) ? (titleEl.InnerText ?? string.Empty).Trim() : null;
            string speaker = (speakerEl != null) ? (speakerEl.InnerText ?? string.Empty).Trim() : null;
            string speakerUrl = (titleEl != null) ? titleEl.GetAttributeValue("href", string.Empty).Trim() : null;
            speakerUrl = (speakerUrl != "#") ? speakerUrl : null;

            return new ScheduleItem { Location = location, Speaker = speaker, SpeakerUrl = speakerUrl, Title = title };
        }
    }
}