using Project2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2.ViewModels
{
    public class PatientFullNameVM
    {
        public IList<Patient> Patients { get; set; }
      
        public string SearchString { get; set; }
    }
}
