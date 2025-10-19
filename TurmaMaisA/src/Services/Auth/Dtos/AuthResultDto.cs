namespace TurmaMaisA.Services.Auth.Dtos
{
    public class AuthResultDto
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public string? UserFullName { get; set; }
        public string? OrganizationName { get; set; }
    }
}
