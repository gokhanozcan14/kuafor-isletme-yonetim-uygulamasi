namespace BerberApp.Models;

public class PackageStaffViewModel
{
    public IEnumerable<Package> Packages { get; set; }
    public IEnumerable<Staff> Staff { get; set; }
    public int? UserId { get; set; }
    public AppointmentDto? AppointmentDto { get; set; }
    

}

public class AppointmentDto
{
    public int? UserId { get; set; }
    
    public int PackageId { get; set; }
    public int StaffId { get; set; }
    public DateTime StartDate { get; set; }
    public string StartTime { get; set; }
    public DateTime EndDate { get; set; }
}