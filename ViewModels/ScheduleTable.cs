using System.Collections.Generic;
using System.Linq;

namespace DevSumScheduler.ViewModels
{
    public class ScheduleTable
    {
        public ScheduleTable()
        {
            Rows = new Dictionary<string, IEnumerable<ScheduleItem>>();
        }

        public string Title { get; set; }

        public IEnumerable<string> Headers { get; set; }

        public Dictionary<string, IEnumerable<ScheduleItem>> Rows { get; set; }

        public int GetColumnCount()
        {
            var first = Rows.Values.FirstOrDefault();
            if (first == null || !first.Any())
            {
                return 0;
            }

            return first.Count();
        }
    }
}