using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project2.Areas.Identity.Data;
using Project2.Data;
using Project2.Models;
using Project2.ViewModels;

namespace Project2.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly Project2Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly UserManager<Project2User> userManager;

        public DoctorsController(Project2Context context, IWebHostEnvironment webHostEnvironment, UserManager<Project2User> usrMgr)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            userManager = usrMgr;
        }

        // GET: Doctors
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchString, string doctorSpecialty)
        {


            var doctors = from d in _context.Doctor
                              select d;


          
          


            if (!string.IsNullOrEmpty(doctorSpecialty))
            {
                doctors = doctors.Where(s => s.Specialty.Contains(doctorSpecialty));
            }
              ViewData["FilterSpecialties"] = doctorSpecialty;
          

            if (!string.IsNullOrEmpty(searchString))
            {
                doctors = doctors.Where(s => (s.FirstName + " " + s.LastName).ToLower().Contains(searchString.ToLower()));
                
            }

            ViewData["FilterNames"] = searchString;


          

            return View(await doctors.AsNoTracking().ToListAsync());


        }
        [Authorize(Roles = "Admin")]

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor
                .Include(d => d.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }
        [Authorize(Roles = "Admin")]

        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name");
           
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(DoctorFormVM Vmodel)
        {
          
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(Vmodel);

                Doctor doctor = new Doctor
                {
                    ProfilePicture = uniqueFileName,

                    FirstName = Vmodel.FirstName,
                    LastName = Vmodel.LastName,
                    Specialty = Vmodel.Specialty,
                    Subspecialty = Vmodel.Subspecialty,
                    DepartmentId = Vmodel.DepartmentId,
                    HireDate = Vmodel.HireDate,
                
                  Membership = Vmodel.Membership,
                    Patients = Vmodel.Patients
                   
                };
                ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", doctor.DepartmentId);
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          
            return View();
        }

        private string UploadedFile(DoctorFormVM model)
        {
            string uniqueFileName = null;

            if (model.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfilePicture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilePicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Doctors/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            DoctorFormVM Vmodel = new DoctorFormVM
            {

                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialty = doctor.Specialty,
                Subspecialty = doctor.Subspecialty,
                DepartmentId = doctor.DepartmentId,
                Department = doctor.Department,
                HireDate = doctor.HireDate,
                Membership = doctor.Membership,
               
               
                Patients = doctor.Patients
            };


              ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", doctor.DepartmentId);
            return View(Vmodel);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, DoctorFormVM Vmodel)
        {
            if (id != Vmodel.Id)
            {
                return NotFound();
            }
           
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadedFile(Vmodel);

                    Doctor doctor = new Doctor
                    {
                        Id = Vmodel.Id,
                        ProfilePicture = uniqueFileName,

                        FirstName = Vmodel.FirstName,
                        LastName = Vmodel.LastName,
                        Specialty = Vmodel.Specialty,
                        Subspecialty =Vmodel.Subspecialty,
                        DepartmentId = Vmodel.DepartmentId,
                        Department = Vmodel.Department,
                        HireDate = Vmodel.HireDate,
                        Membership = Vmodel.Membership,

                       
                        Patients = Vmodel.Patients
                    };
                    ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", doctor.DepartmentId);
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(Vmodel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
           
            return View(Vmodel);
        }


        [Authorize(Roles = "Admin")]
        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor
                .Include(d => d.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctor.FindAsync(id);
         
            _context.Doctor.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        private bool DoctorExists(int id)
        {
            return _context.Doctor.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> MyPatients(long? id)
        {
            IQueryable<Patient> patients = _context.Patient.Include(d => d.Department).AsQueryable();

            IQueryable<Appointment> appointments = _context.Appointment.AsQueryable();

            appointments = appointments.Where(s => s.DoctorId == id);
            
            IEnumerable<int> appointmentsIDS = appointments.Select(e => e.PatientId).Distinct();

            patients = patients.Where(s => appointmentsIDS.Contains(s.Id));

            patients = patients.Include(c => c.Doctors).ThenInclude(c => c.Doctor);

            ViewData["DoctorName"] = _context.Doctor.Where(t => t.Id == id).Select(t => t.FullName).FirstOrDefault();
            ViewData["doctorId"] = id;

            

            return View(patients);
        }

       



    }
}
