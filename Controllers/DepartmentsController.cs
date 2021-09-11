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
    public class DepartmentsController : Controller
    {
        private readonly Project2Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DepartmentsController(Project2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [Authorize(Roles = "Admin")]
        // GET: Departments
        public async Task<IActionResult> Index( string DepartmentName)
        {

   
          

            var departments = from d in _context.Department
                              select d;



            

            if (!String.IsNullOrEmpty(DepartmentName))
            {
                departments = departments.Where(s => s.Name.Contains(DepartmentName));
            }
            ViewData["FilterNames"] = DepartmentName;

            return View(await departments.AsNoTracking().ToListAsync());

      }



        // GET: Departments/Details/5
        [Authorize(Roles = "Admin")]    
       public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [Authorize(Roles = "Admin")]

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
     


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(DepartmentFormVM Vmodel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(Vmodel);

                Department department = new Department
                {
                    ProfilePicture = uniqueFileName,
                    
                    Name = Vmodel.Name,
                   
                    NumberOfNurses = Vmodel.NumberOfNurses,
                    NumberOfParamedics = Vmodel.NumberOfParamedics,
                    UnitSecretaryContactNumber = Vmodel.UnitSecretaryContactNumber,
                    Doctors = Vmodel.Doctors,
                    Patients = Vmodel.Patients
                };

                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        private string UploadedFile(DepartmentFormVM model)
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





        [Authorize(Roles = "Admin")]
        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            DepartmentFormVM Vmodel = new DepartmentFormVM
            {
               
                Id = department.Id,
                Name = department.Name,

                NumberOfNurses = department.NumberOfNurses,
                NumberOfParamedics = department.NumberOfParamedics,
                UnitSecretaryContactNumber = department.UnitSecretaryContactNumber,
                Doctors = department.Doctors,
                Patients = department.Patients
            };
            ViewData["DepartmentName"] = _context.Department.Where(t => t.Id == id).Select(t => t.Name).FirstOrDefault();
            return View(Vmodel);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, DepartmentFormVM Vmodel)
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

                    Department department = new Department
                    {
                        Id = Vmodel.Id,
                        ProfilePicture = uniqueFileName,

                        Name = Vmodel.Name,

                        NumberOfNurses = Vmodel.NumberOfNurses,
                        NumberOfParamedics = Vmodel.NumberOfParamedics,
                        UnitSecretaryContactNumber = Vmodel.UnitSecretaryContactNumber,
                        Doctors = Vmodel.Doctors,
                        Patients = Vmodel.Patients
                    };
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(Vmodel.Id))
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


        // GET: Departments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Department.FindAsync(id);
         
            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.Id == id);
        }
        [Authorize(Roles = "Admin")]
        // GET: Departments/Doctors/
        public async Task<IActionResult> Doctors(int id)
        {

            var doctors = _context.Doctor.Where(c => c.DepartmentId == id);
            doctors = doctors.Include(t => t.Department);

            ViewData["DepartmentId"] = id;
            ViewData["DepartmentName"] = _context.Department.Where(t => t.Id == id).Select(t => t.Name).FirstOrDefault();
            return View(doctors);
        }
    }
}
