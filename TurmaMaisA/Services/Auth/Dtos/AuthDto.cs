namespace TurmaMaisA.Services.Auth.Dtos
{
    public class AuthDto
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
