using System.Collections.Generic;
using System.Threading.Tasks;
using TBCBanking.Domain.Models.DbEntities;

namespace TBCBanking.Domain.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report1Entity>> Report1();
    }
}