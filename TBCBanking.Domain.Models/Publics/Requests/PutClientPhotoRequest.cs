namespace TBCBanking.Domain.Models.Publics.Requests
{
    public class PutClientPhotoRequest
    {
        public int ClientId { get; set; }
        public string PhotoName { get; set; }
        public byte[] PhotoData { get; set; }
    }
}