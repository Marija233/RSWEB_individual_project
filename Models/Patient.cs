using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project2.Models
{
    public class Patient
    {
        public int Id { get; set; }

       


        [Display(Name = "First Name")]
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [StringLength(50)]
        [Required]
        public string LastName { get; set; }



        [Display(Name = "Patient No.")]
        [Required]
        public int PatientNumber { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [Required]
        public DateTime BirthDate { get; set; }

        [StringLength(20)]
        public string Gender { get; set; }

        [Display(Name = "Phone No.")]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        [Display(Name = "Mailing Address")]
        [StringLength(150)]
        public string MailingAddress { get; set; }

        [Display(Name = "First Name")]
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }


        [NotMapped]
        public int Age
        {
            get
            {
                TimeSpan span = DateTime.Now - BirthDate;
                double years = (double)span.TotalDays / 365.2425;
                return (int)years;
            }
        }



        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }


        public ICollection<Appointment> Doctors { get; set; }
    }
}
