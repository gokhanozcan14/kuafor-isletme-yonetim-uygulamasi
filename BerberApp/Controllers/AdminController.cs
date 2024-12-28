using BerberApp.Models;
using BerberApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BerberApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly StaffService _staffService;
        private readonly IAppointmentService _appointmentService;
        private readonly UserService _userService;
        private readonly PackageService _packageService;

        public AdminController(StaffService staffService, 
            IAppointmentService appointmentService,
            UserService userService, PackageService packageService)
        {
            _staffService = staffService;
            _appointmentService = appointmentService;
            _userService = userService;
            _packageService = packageService;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userService.GetUserByUsernameAndPasswordAsync(username, password);
            if (user != null && user.Role == "Admin")
            {
                // Kullanıcı doğrulandı, admin sayfasına yönlendir
                return RedirectToAction("Dashboard");
            }

            // Kullanıcı doğrulanamadı, hata mesajı göster
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View("Index");
        }
        
        public IActionResult Dashboard()
        {
            return View();
        }
        
        public async Task<IActionResult> EditStaff(int id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            ViewBag.ExpertiseAreas = Enum.GetValues(typeof(ExpertiseArea))
                .Cast<ExpertiseArea>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.GetDescription(),
                    Selected = staff.ExpertiseAreas.Contains(e)
                }).ToList();

            return View(staff);
        }
        
        public async Task<IActionResult> Staff()
        {
            // Tüm personel listesini alıyoruz
            var staffList = await _staffService.GetAllStaffAsync();
            return View(staffList);
        }
        
        public async Task<IActionResult> Package()
        {
            var packageList = await _packageService.GetAllPackagesAsync();
            return View(packageList);
        }
        
        public async Task<IActionResult> EditPackage(int id)
        {
            var package = await _packageService.GetPackageByIdAsync(id);
            return View(package);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePackage(Package package, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var directoryPath = Path.Combine("wwwroot", "images");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, ImageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                package.ImageUrl = $"/images/{ImageFile.FileName}";
            }
            else
            {
                package.ImageUrl = "/images/default.png";
            }

            if (ModelState.IsValid)
            {
                if (package.Id > 0)
                {
                    await _packageService.UpdatePackageAsync(package);
                }
                else
                {
                    await _packageService.AddPackageAsync(package);
                }
                return RedirectToAction("Package");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View("EditPackage", package);
        }      
        public async Task<IActionResult> Appointment()
        {
            var allAppointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(allAppointments);
        }
        
        public IActionResult SavePackage()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff)
        {
            if (ModelState.IsValid)
            {
                await _staffService.AddStaffAsync(staff);
                return RedirectToAction("Staff");
            }

            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveStaff(Staff staff)
        {
            if (ModelState.IsValid)
            {
                if (staff.Id > 0)
                {
                    // Update existing staff
                    await _staffService.UpdateStaffAsync(staff);
                }
                else
                {
                    // Create new staff
                    await _staffService.AddStaffAsync(staff);
                }
                return RedirectToAction("Staff");
            }

            ViewBag.ExpertiseAreas = Enum.GetValues(typeof(ExpertiseArea))
                .Cast<ExpertiseArea>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.GetDescription(),
                    Selected = staff.ExpertiseAreas.Contains(e)
                }).ToList();

            return View("EditStaff", staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            if (staff != null)
            {
                await _staffService.DeleteStaffAsync(id);
                return RedirectToAction("Staff");
            }

            return NotFound();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var package = await _packageService.GetPackageByIdAsync(id);
            if (package != null)
            {
                await _packageService.DeletePackageAsync(id);
                return RedirectToAction("Package");
            }

            return NotFound();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment != null)
            {
                appointment.IsApproved = true;
                await _appointmentService.UpdateAppointmentAsync(appointment);
            }
            return RedirectToAction("Appointment");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectAppointment(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment != null)
            {
                appointment.IsRejected = true;
                await _appointmentService.UpdateAppointmentAsync(appointment);
            }
            return RedirectToAction("Appointment");
        }
    }
}
