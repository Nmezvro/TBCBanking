using System.Threading.Tasks;
using TBCBanking.Domain.Models.Publics.Responses;

namespace TBCBanking.Domain.Services
{
    public interface IReportService
    {
        Task<Report1Response> Report1();
    }
}