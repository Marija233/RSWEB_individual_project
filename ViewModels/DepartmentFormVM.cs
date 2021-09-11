using Microsoft.AspNetCore.Http;
using Project2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project2.ViewModels
{
    public class DepartmentFormVM
    {
        [Display(Name="Image")]
        public IFormFile ProfilePicture { get; set; }

        public int Id { get; set; }


        [StringLength(100)]
        [Required]
        public string Name { get; set; }


        [Display(Name = "Number Of Nurses")]
        public int? NumberOfNurses { get; set; }


        [Display(Name = "Number Of Paramedics")]
        public int? NumberOfParamedics { get; set; }


        [Display(Name = "Contact No. of Unit Secretary")]
        [StringLength(50)]
        public string UnitSecretaryContactNumber { get; set; }

        public ICollection<Doctor> Doctors { get; set; }

        public ICollection<Patient> Patients { get; set; }


    }
}
