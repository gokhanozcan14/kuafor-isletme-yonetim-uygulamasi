namespace BerberApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public int PackageId { get; set; }
        public virtual Package Package { get; set; }
        public int StaffId { get; set; }
        public virtual Staff? Staff { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}