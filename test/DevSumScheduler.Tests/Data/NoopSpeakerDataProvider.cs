using System.Threading.Tasks;
using DevSumScheduler.Data;

namespace DevSumScheduler.Tests.Data
{
    public class NoopSpeakerDataProvider : ISpeakerDataProvider
    {
        public Task<string> GetData(string slug)
        {
            return Task.FromResult(string.Empty);
        }
    }
}