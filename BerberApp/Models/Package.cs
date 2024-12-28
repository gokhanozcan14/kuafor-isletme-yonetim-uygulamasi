namespace BerberApp.Models
{
    public class Package
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
    }
}
