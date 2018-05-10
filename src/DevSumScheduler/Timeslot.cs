using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DevSumScheduler
{
    [DebuggerDisplay("{StartsAt}-{EndsAt} - IsSelectable: {IsSelectable}. Sessions: {Sessions.Count}.")]
    public class Timeslot
    {
        public Timeslot(TimeSpan startsAt, TimeSpan endsAt, IEnumerable<Session> sessions)
        {
            if (sessions == null) throw new ArgumentNullException(nameof(sessions));

            StartsAt = startsAt;
            EndsAt = endsAt;
            Sessions = new List<Session>(sessions);

            IsSelectable = GetIsSelectable();
        }

        public TimeSpan StartsAt { get; }

        public TimeSpan EndsAt { get; }

        public IReadOnlyList<Session> Sessions { get; }

        public bool IsSelectable { get; }

        private bool GetIsSelectable()
        {
            var firstSession = Sessions.FirstOrDefault();
            return firstSession != null && !Sessions.All(x =>
                       x.Title == firstSession.Title && x.SpeakerTitle == firstSession.SpeakerTitle &&
                       x.SpeakerSlug == firstSession.SpeakerSlug);
        }
    }
}