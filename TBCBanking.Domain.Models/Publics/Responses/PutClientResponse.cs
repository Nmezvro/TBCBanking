namespace TBCBanking.Domain.Models.Publics.Responses
{
    public class PutClientResponse
    {
        public PutClientResponse()
        {

        }

        public PutClientResponse(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}