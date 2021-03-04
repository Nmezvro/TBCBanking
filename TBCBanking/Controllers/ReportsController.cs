using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TBCBanking.API.ApiConfigurations;
using TBCBanking.Domain.Services;

namespace TBCBanking.API.Controllers
{
    [ServiceFilter(typeof(InputValidationActionFilter))]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// რეპორტი თუ რამდენი დაკავშირებული პირი ჰყავს თითოეულ ფიზიკურ პირს, კავშირის ტიპის მიხედვით
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/reports/RelatedClientsCountByType")]
        public async Task<IActionResult> Report1()
        {
            return Ok(await _reportService.Report1());
        }
    }
}