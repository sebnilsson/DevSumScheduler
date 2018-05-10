using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DevSumScheduler
{
    [DebuggerDisplay("{Title} - Timeslots: {Timeslots.Count}. Sessions: {Sessions.Count}. Locations: {Locations.Count}.")]
    public class Day
    {
        public Day(IEnumerable<Session> sessions, IEnumerable<string> locations = null, string title = null)
        {
            if (sessions == null) throw new ArgumentNullException(nameof(sessions));

            Sessions = new List<Session>(sessions);
            Locations = new List<string>(locations ?? Enumerable.Empty<string>());
            Title = title;

            Timeslots = GetTimeslots(Sessions).ToList();
        }

        public string Title { get; }

        public IReadOnlyList<Session> Sessions { get; }

        public IReadOnlyList<string> Locations { get; }

        public IReadOnlyList<Timeslot> Timeslots { get; }

        private static IEnumerable<Timeslot> GetTimeslots(IEnumerable<Session> sessions)
        {
            return (from session in sessions
                group session by new {session.StartsAt, session.EndsAt}
                into g
                select new Timeslot(g.Key.StartsAt, g.Key.EndsAt, g)).ToList();
        }
    }
}