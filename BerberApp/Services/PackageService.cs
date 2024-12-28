using BerberApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BerberApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BerberApp.Services
{
    public class PackageService
    {
        private readonly ApplicationDbContext _context;

        public PackageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Package>> GetAllPackagesAsync()
        {
            return await _context.Packages.ToListAsync();
        }

        public async Task<Package> GetPackageByIdAsync(int id)
        {
            return await _context.Packages.FindAsync(id);
        }

        public async Task AddPackageAsync(Package package)
        {
            _context.Packages.Add(package);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePackageAsync(Package package)
        {
            _context.Packages.Update(package);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePackageAsync(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package != null)
            {
                _context.Packages.Remove(package);
                await _context.SaveChangesAsync();
            }
        }
    }
}