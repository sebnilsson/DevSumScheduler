using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevSumScheduler.Data
{
    public class DayDataService
    {
        private readonly IDataProvider _dataProvider;

        public DayDataService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public async Task<Result> GetResult()
        {
            var daysData = await _dataProvider.GetData();

            var days = daysData.Select(HtmlDataParser.GetDay).ToList();

            return new Result(days);
        }

        public async Task<Day> GetDay(int index)
        {
            var result = await GetResult();

            return index < result.Days.Count ? result.Days[index] : null;
        }
    }
}