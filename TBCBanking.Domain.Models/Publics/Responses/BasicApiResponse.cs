namespace TBCBanking.Domain.Models.Publics.Responses
{
    public class BasicApiResponse : IApiResponse
    {
        public BasicApiResponse()
        {

        }

        public BasicApiResponse(bool successful, params ErrorMessage[] messages)
        {
            ApiMessage = new ApiResponseMessage() { Successful = successful, Errors = messages };
        }

        public ApiResponseMessage ApiMessage { get; set; }
    }
}