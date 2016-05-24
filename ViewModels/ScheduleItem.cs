using System;
using System.Globalization;

using CsQuery;

namespace DevSumScheduler.ViewModels
{
    public class ScheduleItem
    {
        public ScheduleItem(string title, string speaker, string speakerUrl, TimeSpan startTime, TimeSpan endTime)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            this.Speaker = speaker;
            this.SpeakerUrl = speakerUrl;
            this.Title = title;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public TimeSpan EndTime { get; }

        public string Speaker { get; }

        public string SpeakerUrl { get; }

        public TimeSpan StartTime { get; }

        public string Title { get; }

        public static ScheduleItem Parse(CQ element)
        {
            var speaker = element.Find(".before_hour_text").Text();
            string speakerUrl = !string.IsNullOrWhiteSpace(speaker) ? element.Find(".event_header").Attr("href") : null;
            string title = element.Find(".event_header").Text();

            string startTimeText = element.Find(".top_hour span").First().Text();
            string endTimeText = element.Find(".bottom_hour span").First().Text();

            var startTime = TimeSpan.ParseExact(startTimeText, "g", CultureInfo.InvariantCulture);
            var endTime = TimeSpan.ParseExact(endTimeText, "g", CultureInfo.InvariantCulture);

            return new ScheduleItem(title, speaker, speakerUrl, startTime, endTime);
        }
    }
}