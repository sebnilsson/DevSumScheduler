using System.Threading.Tasks;

namespace DevSumScheduler.Data
{
    public interface ISpeakerDataProvider
    {
        Task<string> GetData(string slug);
    }
}