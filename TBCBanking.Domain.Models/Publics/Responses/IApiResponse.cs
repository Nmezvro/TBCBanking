namespace TBCBanking.Domain.Models.Publics.Responses
{
    public interface IApiResponse
    {
        ApiResponseMessage ApiMessage { get; set; }
    }
}