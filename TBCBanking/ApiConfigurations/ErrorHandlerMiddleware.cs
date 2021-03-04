using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using TBCBanking.Domain.Models.Exceptions;
using TBCBanking.Domain.Models.Publics.Responses;

namespace TBCBanking.API.ApiConfigurations
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ClientNotFoundException ex)
            {
                string result = JsonSerializer.Serialize(new BasicApiResponse(false, new ErrorMessage { Code = ApiErrorCode.Validation, Message = ex.Message }));
                context.Response.StatusCode = 400;
                LogResponseError(context, ex, result);
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                string result = JsonSerializer.Serialize(new BasicApiResponse(false, new ErrorMessage { Code = ApiErrorCode.Fatal, Message = ex.Message }));
                context.Response.StatusCode = 500;
                LogResponseError(context, ex, result);
                await context.Response.WriteAsync(result);
            }
        }

        private void LogResponseError(HttpContext context, Exception ex, string response)
        {
            _logger.LogError(ex, "QueryString: {query} Response: {response}", context.Request.Method + context.Request.Path + context.Request.QueryString, response);
        }
    }
}