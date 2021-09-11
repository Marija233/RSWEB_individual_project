using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project2.Models
{
    public class Department
    {

        public int Id { get; set; }


        [StringLength(100)]
        [Required]
        public string Name { get; set; }


        [Display(Name = "Number of Nurses")]
        public int? NumberOfNurses { get; set; }


        [Display(Name = "Number of Paramedics")]
        public int? NumberOfParamedics { get; set; }


        [Display(Name = "Contact No. of Unit Secretary")]
        [StringLength(50)]
        public string UnitSecretaryContactNumber { get; set; }

        public string ProfilePicture { get; set; }

        public ICollection<Doctor> Doctors { get; set; }

        public ICollection<Patient> Patients { get; set; }

    }
}
