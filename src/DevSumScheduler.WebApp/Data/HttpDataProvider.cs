using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DevSumScheduler.Data;

namespace DevSumScheduler.WebApp.Data
{
    public class HttpDataProvider : IDataProvider
    {
        private const string Day1Url = "http://www.devsum.se/day-1/";
        private const string Day2Url = "http://www.devsum.se/day-2/";

        public async Task<IReadOnlyList<string>> GetData()
        {
            var http = new HttpClient();

            var day1Task = http.GetStringAsync(Day1Url);
            var day2Task = http.GetStringAsync(Day2Url);

            await Task.WhenAll(day1Task, day2Task);

            return new List<string> {day1Task.Result, day2Task.Result};
        }
    }
}