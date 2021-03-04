using System.Threading.Tasks;
using TBCBanking.Domain.Repositories;
using System.IO;

namespace TBCBanking.Infrastructure.Repositories
{
    public class FileStorageRepository : IFileStorageRepository
    {
        public async Task SaveFile(byte[] data, string filePath)
        {
            await File.WriteAllBytesAsync(filePath, data);
        }
    }
}