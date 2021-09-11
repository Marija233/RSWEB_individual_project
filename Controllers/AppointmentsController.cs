using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project2.Data;
using Project2.Models;
using Project2.ViewModels;

namespace Project2.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly Project2Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AppointmentsController(Project2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Appointments
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string SearchString)
        {
              IQueryable<Appointment> appointments = _context.Appointment.AsQueryable();
           



            if (!string.IsNullOrEmpty(SearchString))
            {
               

             appointments = appointments.Where(s => (s.Patient.FirstName + " " + s.Patient.LastName).ToLower().Contains(SearchString.ToLower()));

            }

          

            appointments = appointments.Include(s => s.Patient).Include(s => s.Doctor);
            
          

            var AppointmentsVM = new AppointmentDoctorFullNamePatientFullNameVM
            {


                Appointments =   appointments.ToList(),
            
            };

            return View(AppointmentsVM);
        }
        [Authorize(Roles = "Admin, Doctor, Patient")]

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "FullName");
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "FullName");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AppointmentFormVM Vmodel)
        {
           

            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(Vmodel);

                Appointment appointment = new Appointment
                {
                    MedicalReport = uniqueFileName,

                    PatientId = Vmodel.PatientId,

                    DoctorId = Vmodel.DoctorId,
                    Date = Vmodel.Date,
                    Symptoms = Vmodel.Symptoms,
                    Examination = Vmodel.Examination,
                    Diagnosis = Vmodel.Diagnosis,
                    Therapy = Vmodel.Therapy
                };



                ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "FullName", appointment.DoctorId);
                ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "FullName", appointment.PatientId);
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View();
        }
        private string UploadedFile(AppointmentFormVM model)
        {
            string uniqueFileName = null;

            if (model.MedicalReport != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "medicalreports");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.MedicalReport.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.MedicalReport.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        [Authorize(Roles = "Admin")]
        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            AppointmentFormVM Vmodel = new AppointmentFormVM
            {

                Id = appointment.Id,
                PatientId = appointment.PatientId,

                DoctorId = appointment.DoctorId,
                Date = appointment.Date,
                Symptoms = appointment.Symptoms,
                Examination = appointment.Examination,
                Diagnosis = appointment.Diagnosis,
                Therapy = appointment.Therapy
            };



            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "FullName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "FullName", appointment.PatientId);
            return View(Vmodel);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, AppointmentFormVM Vmodel)
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

                    Appointment appointment = new Appointment
                    {
                        Id = Vmodel.Id,
                        MedicalReport = uniqueFileName,

                        PatientId = Vmodel.PatientId,

                        DoctorId = Vmodel.DoctorId,
                        Date = Vmodel.Date,
                        Symptoms = Vmodel.Symptoms,
                        Examination = Vmodel.Examination,
                        Diagnosis = Vmodel.Diagnosis,
                        Therapy = Vmodel.Therapy
                    };

                    ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "FullName", appointment.DoctorId);
                    ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "FullName", appointment.PatientId);

                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(Vmodel.Id))
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
        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
         
            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.Id == id);
        }

        public string UploadFile(IFormFile file)
        {
            string uniqueFileName = "";
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "medicalreports");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            uniqueFileName = "/medicalreports/" + uniqueFileName;
            return uniqueFileName;
        }





        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> EditByPatient(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "FullName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "FullName", appointment.PatientId);
              return View(appointment);
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> EditByPatient(int id,  [Bind("Id,PatientId,DoctorId,Date,Symptoms,Examination,Diagnosis,Therapy,MedicalReport")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

          


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MyDoctors", "Patients", new { id = appointment.PatientId });
                //return RedirectToAction("Index", "Appointments");
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "FullName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "FullName", appointment.PatientId);
             return View(appointment);
           
        }

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> EditByDoctor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "FullName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "FullName", appointment.PatientId);
            return View(appointment);
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> EditByDoctor(int id, [Bind("Id,PatientId,DoctorId,Date,Symptoms,Examination,Diagnosis,Therapy,MedicalReport")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }




            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // return RedirectToAction("Index", "Appointments");
                return RedirectToAction("MyPatients", "Doctors", new { id = appointment.DoctorId });
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "FullName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "FullName", appointment.PatientId);
              return View(appointment);
           
           
        }
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> EditByDoctor2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "FullName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "FullName", appointment.PatientId);
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> EditByDoctor2(int id, [Bind("Id,PatientId,DoctorId,Date,Symptoms,Examination,Diagnosis,Therapy,MedicalReport")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }




            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MyPatients", "Doctors", new { id = appointment.DoctorId });
                //return RedirectToAction("Index", "Appointments");
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "FullName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "FullName", appointment.PatientId);
            return View(appointment);
        }





    }
}
