using BerberApp.Data;
using BerberApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BerberApp.Services
{
    public class StaffService
    {
        private readonly ApplicationDbContext _context;

        public StaffService(ApplicationDbContext context)
        {
            _context = context; // DbContext'in bir örneğini alıyoruz
        }

        // Personel ekleme işlemi
        public async Task AddStaffAsync(Staff staff)
        {
            _context.Staffes.Add(staff); // DbSet üzerinden ekleme işlemi
            await _context.SaveChangesAsync(); // Veritabanına değişiklikleri kaydet
        }

        // Personel listesi alma işlemi
        public async Task<List<Staff>> GetAllStaffAsync()
        {
            return await _context.Staffes.ToListAsync(); // Tüm personel kayıtlarını al
        }

        // Belirli bir personel bulma işlemi
        public async Task<Staff?> GetStaffByIdAsync(int id)
        {
            return await _context.Staffes.FindAsync(id); // ID'ye göre personeli bul
        }

        // Personel güncelleme işlemi
        public async Task UpdateStaffAsync(Staff staff)
        {
            _context.Staffes.Update(staff); // Güncelleme işlemi
            await _context.SaveChangesAsync(); // Veritabanına değişiklikleri kaydet
        }

        // Personel silme işlemi
        public async Task DeleteStaffAsync(int id)
        {
            var staff = await GetStaffByIdAsync(id); // ID'ye göre personeli bul
            if (staff != null)
            {
                _context.Staffes.Remove(staff); // Silme işlemi
                await _context.SaveChangesAsync(); // Veritabanına değişiklikleri kaydet
            }
        }
    }

}
