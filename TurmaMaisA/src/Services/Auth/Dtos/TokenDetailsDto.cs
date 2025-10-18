namespace TurmaMaisA.Services.Auth.Dtos
{
    public class TokenDetailsDto
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
