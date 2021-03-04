using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using TBCBanking.Domain.Models.Publics.Responses;

namespace TBCBanking.API.ApiConfigurations
{
    public class InputValidationActionFilter : IAsyncActionFilter
    {
        public InputValidationActionFilter()
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                BasicApiResponse responseObj =
                    new BasicApiResponse(false,
                    context.ModelState.Values.SelectMany(v => v.Errors.Select(e => new ErrorMessage() { Code = ApiErrorCode.Validation, Message = e.ErrorMessage })).ToArray());
                context.Result = new JsonResult(responseObj);
                return;
            }

            await next();
        }
    }
}