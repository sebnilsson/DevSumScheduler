using System;
using System.Threading.Tasks;

namespace DevSumScheduler.Data
{
    public class SpeakerDataService
    {
        private readonly ISpeakerDataProvider _dataProvider;

        public SpeakerDataService(ISpeakerDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public async Task<string> GetSpeaker(string slug)
        {
            return await _dataProvider.GetData(slug);
        }
    }
}