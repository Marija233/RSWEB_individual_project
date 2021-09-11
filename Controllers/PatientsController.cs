using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project2.Data;
using Project2.Models;
using Project2.ViewModels;

namespace Project2.Controllers
{
    public class PatientsController : Controller
    {
        private readonly Project2Context _context;
       
        public PatientsController(Project2Context context)
        {
            _context = context;
          
        }

        // GET: Patients
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index( string SearchString)
        {
            IEnumerable<Patient> patients = _context.Patient.Include(p => p.Department).Include(m => m.Doctors).ThenInclude(m => m.Doctor);
            

           

            if (!string.IsNullOrEmpty(SearchString))
            {
                patients = patients.Where(s => (s.FirstName + " " + s.LastName).ToLower().Contains(SearchString.ToLower())).ToList();
               
            }


            

            var PatientFullName = new PatientFullNameVM
            {
               
                Patients = patients.ToList()
            };

            return View(PatientFullName);
        }

        // GET: Patients/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.Include(p => p.Department).Include(m => m.Doctors).ThenInclude(m => m.Doctor).FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }
        [Authorize(Roles = "Admin")]

        // GET: Patients/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PatientNumber,BirthDate,Gender,PhoneNumber,EmailAddress,MailingAddress,DepartmentId")] Patient patient)
        {
            if (ModelState.IsValid)
            {

               

                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }




            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", patient.DepartmentId);
            return View();
        }


        // GET: Patients/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


           

            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            
        

            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", patient.DepartmentId);
            return View(patient);
            
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PatientNumber,BirthDate,Gender,PhoneNumber,EmailAddress,MailingAddress,DepartmentId")]
        Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", patient.DepartmentId);
            return View(patient);
        }

        // GET: Patients/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.Include(p => p.Department).Include(m => m.Doctors).ThenInclude(m => m.Doctor).FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patient.FindAsync(id);
            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.Id == id);
        }
        [Authorize(Roles = "Admin, Patient")]
        public async Task<IActionResult> MyDoctors(long? id)
        {
            IQueryable<Doctor> doctors = _context.Doctor.Include(c => c.Department).AsQueryable();

            IQueryable<Appointment> appointments = _context.Appointment.AsQueryable();

            appointments = appointments.Where(s => s.PatientId == id);
            IEnumerable<int> appointmentsIDS = appointments.Select(e => e.DoctorId).Distinct();

            doctors = doctors.Where(s => appointmentsIDS.Contains(s.Id));

            doctors = doctors.Include(c => c.Patients).ThenInclude(c => c.Patient);

            ViewData["PatientName"] = _context.Patient.Where(t => t.Id == id).Select(t => t.FullName).FirstOrDefault();
            ViewData["patientId"] = id;
            return View(doctors);
        }
       



    }
}
