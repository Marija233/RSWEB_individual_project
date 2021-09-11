using Microsoft.AspNetCore.Http;
using Project2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project2.ViewModels
{
    public class AppointmentFormVM
    {
        public IFormFile MedicalReport { get; set; }
        public int Id { get; set; }

        [Required]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [Required]
        [Display(Name = "Doctor")]
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
    }
}
