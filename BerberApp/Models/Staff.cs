namespace BerberApp.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<ExpertiseArea> ExpertiseAreas { get; set; } = new List<ExpertiseArea>();

    }
}
