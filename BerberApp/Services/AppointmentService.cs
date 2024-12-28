using BerberApp.Data;
using BerberApp.Models;
using Microsoft.EntityFrameworkCore;

public interface IAppointmentService
{
    Task CreateAppointmentAsync(Appointment appointment);
    Task<Appointment> GetAppointmentByDateAsync(DateTime startDate, int staffId);
    Task<List<Appointment>> GetAllAppointmentsAsync();
    Task<Appointment> GetAppointmentByIdAsync(int id);
    Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId);
    Task UpdateAppointmentAsync(Appointment appointment);
}

public class AppointmentService : IAppointmentService
{
    private readonly ApplicationDbContext _context;

    public AppointmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAppointmentAsync(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task<Appointment> GetAppointmentByDateAsync(DateTime startDate, int staffId)
    {
        return await _context.Appointments
            .Include(a => a.Package)
            .Include(a => a.Staff)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.StartDate == startDate && a.StaffId == staffId);
    }

    public async Task<List<Appointment>> GetAllAppointmentsAsync()
    {
        return await _context.Appointments
            .Include(a => a.Package)
            .Include(a => a.Staff)
            .Include(a => a.User)
            .ToListAsync();
    }

    public async Task<Appointment> GetAppointmentByIdAsync(int id)
    {
        return await _context.Appointments
            .Include(a => a.Package)
            .Include(a => a.Staff)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    
    public async Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId)
    {
        return await _context.Appointments
            .Include(a => a.Package)
            .Include(a => a.Staff)
            .Include(a => a.User)
            .Where(a => a.UserId == userId).ToListAsync();
    }


    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }
}