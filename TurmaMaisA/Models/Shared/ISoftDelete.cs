namespace TurmaMaisA.Models.Shared
{
    public interface ISoftDelete
    {
        public DateTime? DeletedAt { get; set; }
    }
}
