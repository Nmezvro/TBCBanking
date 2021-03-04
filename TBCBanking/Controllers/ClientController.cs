using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TBCBanking.API.ApiConfigurations;
using TBCBanking.Domain.Models.Publics.Requests;
using TBCBanking.Domain.Models.Publics.Responses;
using TBCBanking.Domain.Services;

namespace TBCBanking.API.Controllers
{
    [ServiceFilter(typeof(InputValidationActionFilter))]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// ფიზიკური პირის დამატება
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/client")]
        public async Task<IActionResult> RegisterClient([FromBody] RegisterClientRequest request)
        {
            int id = await _clientService.RegisterClient(request);
            return Ok(new PutClientResponse(id));
        }

        /// <summary>
        /// ფიზიკური პირის ძირითადი ინფორმაციის ცვლილება, რომელიც მოიცავს შემდეგ მონაცემებს: სახელი, გვარი, სქესი, პირადი ნომერი, დაბადების თარიღი, ქალაქი, ტელეფონის ნომრები
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("api/client")]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientRequest request)
        {
            int id = await _clientService.UpdateClient(request);
            return Ok(new PutClientResponse(id));
        }

        /// <summary>
        /// ფიზიკური პირის წაშლა
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("api/client")]
        public async Task<IActionResult> DeactivateClient([FromBody] DeactivateClientRequest request)
        {
            await _clientService.DeactivateClient(request);
            return Ok();
        }

        /// <summary>
        /// ფიზიკური პირის შესახებ სრული ინფორმაციის მიღება იდენტიფიკატორის მეშვეობით (დაკავშირებული ფიზიკური პირების და სურათის ჩათვლით)
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("api/client")]
        public async Task<IActionResult> GetClient(int clientId)
        {
            return Ok(await _clientService.GetClient(clientId));
        }

        /// <summary>
        /// ფიზიკური პირის სურათის ატვირთვა/ცვლილება (სურათი შევინახოთ ფაილურ სისტემაში)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("api/client/photo")]
        public async Task<IActionResult> PutClientPhoto([FromBody] PutClientPhotoRequest request)
        {
            await _clientService.PutClientPhoto(request);
            return Ok();
        }

        /// <summary>
        /// ფიზიკური პირის დაკავშირებული ფიზიკური პირის დამატება
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/client/relatives")]
        public async Task<IActionResult> AddClientRelatives([FromBody] AddClientRelativesRequest request)
        {
            await _clientService.AddClientRelatives(request);
            return Ok();
        }

        /// <summary>
        /// ფიზიკური პირის დაკავშირებული ფიზიკური პირის წაშლა
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("api/client/relatives")]
        public async Task<IActionResult> DeleteClientRelatives([FromBody] DeleteClientRelativesRequest request)
        {
            await _clientService.DeleteClientRelatives(request);
            return Ok();
        }

        /// <summary>
        /// ფიზიკური პირების სიის მიღება, სწრაფი ძებნის (სახელი, გვარი, პირადი ნომრის მიხედვით (დამთხვევა sql like პრინციპით))
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("api/client/qsearch")]
        public async Task<IActionResult> QuickClientSearch(QuickClientSearchRequest request)
        {
            return Ok(await _clientService.QuickSearchClient(request));
        }

        /// <summary>
        /// დეტალური ძებნის (ყველა ველის მიხედვით) და paging-ის ფუნქციით
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("api/client/search")]
        public async Task<IActionResult> ClientSearch(ClientSearchRequest request)
        {
            return Ok(await _clientService.SearchClient(request));
        }
    }
}