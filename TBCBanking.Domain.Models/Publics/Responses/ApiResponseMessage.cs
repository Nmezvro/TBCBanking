using System.Collections.Generic;

namespace TBCBanking.Domain.Models.Publics.Responses
{
    public class ApiResponseMessage
    {
        public bool Successful { get; set; }
        public IEnumerable<ErrorMessage> Errors { get; set; }
    }

    public class ErrorMessage
    {
        public ApiErrorCode Code { get; set; }
        public string Message { get; set; }
    }

    public enum ApiErrorCode
    {
        Validation = 400,
        Fatal = 500,
    }
}