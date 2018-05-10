using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DevSumScheduler
{
    [DebuggerDisplay("Days: {Days.Count}")]
    public class Result
    {
        public Result(IEnumerable<Day> days)
        {
            if (days == null) throw new ArgumentNullException(nameof(days));

            Days = new List<Day>(days);
        }

        public IReadOnlyList<Day> Days { get; set; }
    }
}