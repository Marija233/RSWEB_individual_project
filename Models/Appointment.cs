using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project2.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [Required]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime? Date { get; set; }

        [StringLength(300)]
        public string Symptoms { get; set; }

        [StringLength(300)]
        public string Examination { get; set; }

        [StringLength(300)]
        public string Diagnosis { get; set; }

        [StringLength(300)]
        public string Therapy { get; set; }

        [StringLength(200)]
        [Display(Name = "Dictated Medical Report")]
        public string MedicalReport { get; set; }
    }
}
