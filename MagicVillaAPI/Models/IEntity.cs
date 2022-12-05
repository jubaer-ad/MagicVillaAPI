namespace MagicVillaAPI.Models
{
    public interface IEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        //public DateTime getCreatedAt();
    }
}
