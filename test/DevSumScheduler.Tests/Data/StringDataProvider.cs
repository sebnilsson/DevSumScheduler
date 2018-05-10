using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevSumScheduler.Data;

namespace DevSumScheduler.Tests.Data
{
    public class StringDataProvider : IDataProvider
    {
        private readonly IEnumerable<string> _data;

        public StringDataProvider(params string[] data) : this(data.AsEnumerable())
        {
        }

        public StringDataProvider(IEnumerable<string> data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public Task<IReadOnlyList<string>> GetData()
        {
            return Task.FromResult(new List<string>(_data) as IReadOnlyList<string>);
        }
    }
}