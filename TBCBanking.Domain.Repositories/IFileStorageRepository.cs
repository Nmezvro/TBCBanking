using System.Threading.Tasks;

namespace TBCBanking.Domain.Repositories
{
    public interface IFileStorageRepository
    {
        Task SaveFile(byte[] data, string filePath);
    }
}