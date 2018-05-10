using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevSumScheduler.Data
{
    public interface IDataProvider
    {
        Task<IReadOnlyList<string>> GetData();
    }
}