using System.Collections.Generic;

using CsQuery;

namespace DevSumScheduler.ViewModels
{
    public class ScheduleItem
    {
        public string Speaker { get; private set; }

        public string SpeakerUrl { get; private set; }

        public string Title { get; private set; }

        public static IEnumerable<ScheduleItem> FromColumns(CQ columnElements)
        {
            for (int i = 0; i < columnElements.Length; i++)
            {
                var columnElement = columnElements.Eq(i);

                var item = GetScheduleItem(columnElement);
                if (item != null)
                {
                    yield return item;
                }
            }
        }

        private static ScheduleItem GetScheduleItem(CQ htmlNode)
        {
            var eventHeaderEl = htmlNode.Find(".event_header");

            string title = (eventHeaderEl.Text() ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                return null;
            }

            string speaker = (htmlNode.Find(".before_hour_text").Text() ?? string.Empty).Trim();
            speaker = !string.IsNullOrWhiteSpace(speaker) ? speaker : null;

            string speakerUrl = !string.IsNullOrWhiteSpace(speaker) ? (eventHeaderEl.Attr("href")) : null;
            
            return new ScheduleItem
                       {
                           Speaker = speaker,
                           SpeakerUrl = speakerUrl,
                           Title = title
                       };
        }
    }
}