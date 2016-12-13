using System.Threading.Tasks;
using LaunchPal.Enums;

namespace LaunchPal.Interface
{
    public interface IStoreCache
    {
        Task<bool> SaveCache(string objectToStore, CacheType type);

        string LoadSettings(CacheType type);

        string LoadCache(CacheType type);

        Task ClearAllCache();

        Task ClearCache(CacheType type);
    }
}
