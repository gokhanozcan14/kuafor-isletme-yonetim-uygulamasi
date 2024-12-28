using System.Diagnostics;
using BerberApp.Filters;
using BerberApp.Models;
using BerberApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BerberApp.Controllers
{
    [ServiceFilter(typeof(RequireLoginFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StaffService _staffService;
        private readonly IAppointmentService _appointmentService;
        private readonly UserService _userService;
        private readonly PackageService _packageService;

        public HomeController(ILogger<HomeController> logger, StaffService staffService,
            IAppointmentService appointmentService,
            UserService userService, PackageService packageService)
        {
            _logger = logger;
            _staffService = staffService;
            _appointmentService = appointmentService;
            _userService = userService;
            _packageService = packageService;
        }
        
        
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
    
            var user = await _userService.GetUserByUsernameAsync(User.Identity.Name);
            if (user != null && user.Role == "User")
            {
                var packageList = await _packageService.GetAllPackagesAsync();
                var staffList = await _staffService.GetAllStaffAsync();
                var viewModel = new PackageStaffViewModel
                {
                    Packages = packageList,
                    Staff = staffList,
                    UserId = user.Id // Pass the UserId to the view model
                };
                return View(viewModel);
            } 
            else if (user != null && user.Role == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return View();
        }

        public async Task<IActionResult> MyAppointments()
        {
            var user = await _userService.GetUserByUsernameAsync(User.Identity.Name);
            var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(user.Id);
            return View(appointments);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(AppointmentDto appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.StartDate = appointment.StartDate.ToUniversalTime();
                appointment.EndDate = appointment.StartDate.AddHours(1).ToUniversalTime();
                
                var existingAppointment = await _appointmentService.GetAppointmentByDateAsync(appointment.StartDate, appointment.StaffId);
                if (existingAppointment != null)
                {
                    ModelState.AddModelError("", "The selected time slot is already booked.");
                }
                else
                {
                    var user = await _userService.GetUserByUsernameAsync(User.Identity.Name);

                    var newAppointment = new Appointment
                    {
                        UserId = user.Id,
                        PackageId = appointment.PackageId,
                        StaffId = appointment.StaffId,
                        StartDate = appointment.StartDate,
                        EndDate = appointment.EndDate
                    };
                    await _appointmentService.CreateAppointmentAsync(newAppointment);
                    return RedirectToAction("Index");
                }
            }

            var packageList = await _packageService.GetAllPackagesAsync();
            var staffList = await _staffService.GetAllStaffAsync();
            var viewModel = new PackageStaffViewModel
            {
                Packages = packageList,
                Staff = staffList
            };

            return View("Index", viewModel);
        }
    }
}
