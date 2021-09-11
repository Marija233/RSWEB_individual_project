using Microsoft.AspNetCore.Http;
using Project2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project2.ViewModels
{
    public class DoctorFormVM
    {
        [Display(Name = "Image")]
        public IFormFile ProfilePicture { get; set; }

        public int Id { get; set; }


        [Display(Name = "First Name")]
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }



        [Display(Name = "Last Name")]
        [StringLength(50)]
        [Required]
        public string LastName { get; set; }



        [StringLength(100)]
        public string Specialty { get; set; }


        [StringLength(100)]
        public string Subspecialty { get; set; }


        
        [Display(Name = "Department")]
        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }


        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }


        public string Membership { get; set; }

      


        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        public ICollection<Appointment> Patients { get; set; }
    }
}
