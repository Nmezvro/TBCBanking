using System.Linq;
using System.Threading.Tasks;
using TBCBanking.Domain.Models.Publics.Common;
using TBCBanking.Domain.Models.Publics.Responses;
using TBCBanking.Domain.Repositories;
using TBCBanking.Domain.Services;

namespace TBCBanking.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;

        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<Report1Response> Report1()
        {
            System.Collections.Generic.IEnumerable<Domain.Models.DbEntities.Report1Entity> dbEntity = await _repository.Report1();
            return new Report1Response
            {
                Data = dbEntity.GroupBy(g => new { g.ClientId, g.PersonalNumber }).Select(s => new Report1
                {
                    ClientId = s.Key.ClientId,
                    PersonalNumber = s.Key.PersonalNumber,
                    Data = s.Select(d => new Report1Data { Type = (RelatedClientType)d.TypeId, RelativeCount = d.RelativeCount })
                })
            };
        }
    }
}