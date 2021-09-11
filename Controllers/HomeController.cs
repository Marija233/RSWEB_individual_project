using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project2.Areas.Identity.Data;
using Project2.Data;
using Project2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Project2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Project2Context _context;
        private readonly UserManager<Project2User> userManager;

        public HomeController(ILogger<HomeController> logger, Project2Context context, UserManager<Project2User> usrMgr)
        {
            _logger = logger;
            _context = context;
            userManager = usrMgr;

        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Departments");
            }
            else if (User.IsInRole("Doctor"))
            {
               
                var userID = userManager.GetUserId(User);
                Project2User user = await userManager.FindByIdAsync(userID);
                return RedirectToAction("MyPatients", "Doctors", new { id = user.DoctorId });
            }
            else if (User.IsInRole("Patient"))
            {
                var userID = userManager.GetUserId(User);
                Project2User user = await userManager.FindByIdAsync(userID);
                return RedirectToAction("MyDoctors", "Patients", new { id = user.PatientId });
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
