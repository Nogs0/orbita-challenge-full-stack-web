namespace TurmaMaisA.Middlewares.ExceptionHandling.Dtos
{
    public class ExceptionHandlingDto
    {
        public required string Message { get; set; }
        public int StatusCode { get; set; }
        public string? Details { get; set; }
    }
}
