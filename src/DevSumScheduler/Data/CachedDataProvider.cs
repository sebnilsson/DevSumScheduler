using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeyLocks;
using Microsoft.Extensions.Caching.Memory;

namespace DevSumScheduler.Data
{
    public class CachedDataProvider : IDataProvider
    {
        private readonly IMemoryCache _cache;
        private readonly IDataProvider _dataProvider;
        private readonly NameLock _keyLock = new NameLock();

        public CachedDataProvider(IMemoryCache cache, IDataProvider dataProvider)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public async Task<IReadOnlyList<string>> GetData()
        {
            var key = $"{nameof(CachedDataProvider)}.{nameof(GetData)}";

            return await _keyLock.RunWithLock(key,
                () => _cache.GetOrCreateAsync("GetData", _ => _dataProvider.GetData()));
        }
    }
}