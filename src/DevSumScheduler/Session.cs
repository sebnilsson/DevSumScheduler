using System;
using System.Diagnostics;

namespace DevSumScheduler
{
    [DebuggerDisplay("{StartsAt}-{EndsAt} - {Title} ({SpeakerTitle}) [{SpeakerSlug}] - {Location}")]
    public class Session
    {
        public Session(
            string title,
            string location,
            TimeSpan startsAt,
            TimeSpan endsAt,
            string speakerTitle = null,
            string speakerSlug = null)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Location = location ?? throw new ArgumentNullException(nameof(location));
            StartsAt = startsAt;
            EndsAt = endsAt;
            SpeakerTitle = speakerTitle;
            SpeakerSlug = speakerSlug;
        }

        public string Title { get; }

        public string SpeakerTitle { get; }

        public string SpeakerSlug { get; }

        public TimeSpan StartsAt { get; }

        public TimeSpan EndsAt { get; }

        public string Location { get; }
    }
}