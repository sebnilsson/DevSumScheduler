using System;
using System.Threading.Tasks;
using KeyLocks;
using Microsoft.Extensions.Caching.Memory;

namespace DevSumScheduler.Data
{
    public class CachedSpeakerDataProvider : ISpeakerDataProvider
    {
        private readonly IMemoryCache _cache;
        private readonly ISpeakerDataProvider _dataProvider;
        private readonly NameLock _keyLock = new NameLock();

        public CachedSpeakerDataProvider(IMemoryCache cache, ISpeakerDataProvider dataProvider)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public async Task<string> GetData(string slug)
        {
            var key = $"{nameof(CachedSpeakerDataProvider)}.{nameof(GetData)}.{slug}";

            try
            {
                return await _keyLock.RunWithLock(key,
                    () => _cache.GetOrCreateAsync(key, _ => _dataProvider.GetData(slug)));
            }
            catch (InvalidCastException)
            {
                _cache.Remove(key);

                return await _keyLock.RunWithLock(key,
                    () => _cache.GetOrCreateAsync(key, _ => _dataProvider.GetData(slug)));
            }
        }
    }
}