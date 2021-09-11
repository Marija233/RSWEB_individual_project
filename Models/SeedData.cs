using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project2.Areas.Identity.Data;
using Project2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<Project2User>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            Project2User user = await UserManager.FindByEmailAsync("admin@mvchospital.com");
            if (user == null)
            {
                var User = new Project2User();
                User.Email = "admin@mvchospital.com";
                User.UserName = "admin@mvchospital.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            //Add Doctor Role
            roleCheck = await RoleManager.RoleExistsAsync("Doctor");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Doctor"));
            }

            user = await UserManager.FindByEmailAsync("annetaylor@mvchospital.com");
            if (user == null)
            {
                var User = new Project2User();
                User.Email = "annetaylor@mvchospital.com";
                User.UserName = "annetaylor@mvchospital.com";
                User.DoctorId = 1;
                string userPWD = "Anne1234";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Doctor"); }
            }

            //Add Patient Role
            roleCheck = await RoleManager.RoleExistsAsync("Patient");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Patient"));
            }
            user = await UserManager.FindByEmailAsync("annasmith@mvchospital.com");
            if (user == null)
            {
                var User = new Project2User();
                User.Email = "annasmith@mvchospital.com";
                User.UserName = "annasmith@mvchospital.com";
                User.PatientId = 1;
                string userPWD = "Anna1234";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Patient"); }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Project2Context(
            serviceProvider.GetRequiredService<DbContextOptions<Project2Context>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Department.Any() || context.Doctor.Any() || context.Patient.Any())
                {
                    return; // DB has been seeded
                }
                context.Department.AddRange(
                new Department { /*Id = 1, */Name = "Ophthalmology", NumberOfNurses = 12, NumberOfParamedics = 2, UnitSecretaryContactNumber = "+98952111004", ProfilePicture = "https://s23527.pcdn.co/wp-content/uploads/2019/12/Downside-Up-745x449.jpg.optimal.jpg" },
                new Department { /*Id = 2, */Name = "Cardiology", NumberOfNurses = 17, NumberOfParamedics = 10, UnitSecretaryContactNumber = "+98952111003", ProfilePicture = "https://s23527.pcdn.co/wp-content/uploads/2019/12/Downside-Up-745x449.jpg.optimal.jpg" },
                new Department { /*Id = 3, */Name = "Neurosurgery", NumberOfNurses = 6, NumberOfParamedics = 1, UnitSecretaryContactNumber = "+98952111002", ProfilePicture = "https://s23527.pcdn.co/wp-content/uploads/2019/12/Downside-Up-745x449.jpg.optimal.jpg" },
                new Department { /*Id = 4, */Name = "Orthopedics", NumberOfNurses = 14, NumberOfParamedics = 5, UnitSecretaryContactNumber = "+98952111001", ProfilePicture = "https://s23527.pcdn.co/wp-content/uploads/2019/12/Downside-Up-745x449.jpg.optimal.jpg" },
                new Department { /*Id = 5, */Name = "Oncology", NumberOfNurses = 8, NumberOfParamedics = 2, UnitSecretaryContactNumber = "+98952111006", ProfilePicture = "https://s23527.pcdn.co/wp-content/uploads/2019/12/Downside-Up-745x449.jpg.optimal.jpg" },
                new Department { /*Id = 6, */Name = "Plastic Surgery", NumberOfNurses = 10, NumberOfParamedics = 4, UnitSecretaryContactNumber = "+98952111007", ProfilePicture = "https://s23527.pcdn.co/wp-content/uploads/2019/12/Downside-Up-745x449.jpg.optimal.jpg" }
                 );
                context.SaveChanges();
                context.Doctor.AddRange(
                new Doctor { /*Id = 1, */FirstName = "Anne", LastName = "Taylor", DepartmentId = 1, HireDate = DateTime.Parse("2004-3-14"), Specialty = "Ophthalmology", Subspecialty = "Neuro-Ophthalmology", Membership = "American Academy of Ophthalmology (AAO)" , ProfilePicture = "https://s23527.pcdn.co/wp-content/uploads/2019/12/Downside-Up-745x449.jpg.optimal.jpg" },
                  new Doctor { /*Id = 2, */FirstName = "Mary", LastName = "Johnson", DepartmentId = 1, HireDate = DateTime.Parse("1997-7-10"), Specialty = "Ophthalmology", Subspecialty = "Glaucoma", Membership = "American Glaucoma Society (AGS)" },
                    new Doctor { /*Id = 3, */FirstName = "Tom", LastName = "Park", DepartmentId = 2, HireDate = DateTime.Parse("2012-2-5"), Specialty = "Internal Medicine", Subspecialty = "Cardiology", Membership = "European Society of Cardiology (ESC) " },
                      new Doctor { /*Id = 4, */FirstName = "James", LastName = "Flack", DepartmentId = 5, HireDate = DateTime.Parse("2018-5-14"), Specialty = "Oncology", Subspecialty = "Radiation Oncology", Membership = "National Cancer Institute" },
                       new Doctor { /*Id = 5, */FirstName = "Hannah", LastName = "Hale", DepartmentId = 2, HireDate = DateTime.Parse("1987-11-15"), Specialty = "Cardiology", Subspecialty = "Pediatric Cardiology", Membership = "International Pediatric Association (IPA)" },
                         new Doctor { /*Id = 6, */FirstName = "Helen", LastName = "White", DepartmentId = 4, HireDate = DateTime.Parse("2004-3-14"), Specialty = "Orthopedics", Subspecialty = "Orthopedic Physiotherapy", Membership = "American Orthopedic Society" },
                          new Doctor { /*Id = 7, */FirstName = "Adriana", LastName = "Clarke", DepartmentId = 3, HireDate = DateTime.Parse("2004-3-14"), Specialty = "Neurosurgery", Subspecialty = "Pediatric Neurosurgery", Membership = "International Pediatric Association (IPA)" },
                            new Doctor { /*Id = 8, */FirstName = "Julio", LastName = "Lopez", DepartmentId = 2, HireDate = DateTime.Parse("2008-9-18"), Specialty = "Cardiology", Subspecialty = "Electrophysiology", Membership = "European Society of Cardiology (ESC)" },
                              new Doctor { /*Id = 9, */FirstName = "Anastasia", LastName = "Mendes", DepartmentId = 3, HireDate = DateTime.Parse("2000-4-24"), Specialty = "Neurosurgery", Subspecialty = "Neurovascular Surgery", Membership = "European Association of Neurosurgical Societies (EANS)" },
                                new Doctor { /*Id = 10, */FirstName = "Emily", LastName = "Lee", DepartmentId = 6, HireDate = DateTime.Parse("2020-8-7"), Specialty = "Plastic Surgery", Subspecialty = "Reconstructive Surgery", Membership = "American Society of Plastic Surgeons" },
                                  new Doctor { /*Id = 11, */FirstName = "Lucy", LastName = "Thompson", DepartmentId = 6, HireDate = DateTime.Parse("2009-7-15"), Specialty = "Plastic Surgery", Subspecialty = "Hand Surgery", Membership = "Plastic Surgery Associates of Montgomery" },
                                    new Doctor { /*Id = 12, */FirstName = "Timothy", LastName = "Riddle", DepartmentId = 5, HireDate = DateTime.Parse("1995-1-20"), Specialty = "Oncology", Subspecialty = "Surgical Oncology", Membership = "American College of Surgeons" },
                                    new Doctor { /*Id = 13, */FirstName = "Jason", LastName = "Spring", DepartmentId = 4, HireDate = DateTime.Parse("1990-6-4"), Specialty = "Orthopedics", Subspecialty = "Orthopedic Surgery", Membership = "American College of Surgeons" },
                                      new Doctor { /*Id = 14, */FirstName = "Jennie", LastName = "Blue", DepartmentId = 1, HireDate = DateTime.Parse("1992-3-3"), Specialty = "Ophthalmology", Subspecialty = "Comprehensive Ophthalmology", Membership = "American Academy of Ophthalmology (AAO)" },
                                       new Doctor { /*Id = 15, */FirstName = "Alan", LastName = "Blackburn", DepartmentId = 6, HireDate = DateTime.Parse("2015-9-8"), Specialty = "Plastic Surgery", Subspecialty = "Dermatology", Membership = "Aesthetic Surgery Association (ASA)" }



                                      );
                context.SaveChanges();


                context.Patient.AddRange(
                new Patient { /*Id = 1, */FirstName = "Anna", LastName = "Smith", DepartmentId = 1, PatientNumber = 123458753, BirthDate = DateTime.Parse("1967-1-2"), Gender = "Female", PhoneNumber = "+3445656767", EmailAddress = "asmith23@gmail.com", MailingAddress = "7921 South Smith Store St" },
                new Patient { /*Id = 2, */FirstName = "Jenna", LastName = "May", PatientNumber = 986752, BirthDate = DateTime.Parse("1964-3-3"), Gender = "Female", PhoneNumber = "+1223445", EmailAddress = "jennamay@gmail.com", MailingAddress = "260-C North El Camino Real" },
                new Patient { /*Id = 3, */FirstName = "Rose", LastName = "Cavanaugh", DepartmentId = 2, PatientNumber = 1895453673, BirthDate = DateTime.Parse("2004-1-9"), Gender = "Female", PhoneNumber = "+34412127", EmailAddress = "rosecav13@yahoo.com", MailingAddress = "591 Grand Avenue " },
                new Patient { /*Id = 4, */FirstName = "Janet", LastName = "Jones", DepartmentId = 3, PatientNumber = 8967453, BirthDate = DateTime.Parse("1955-8-9"), Gender = "Female", PhoneNumber = "+34423", EmailAddress = "jjones1@gmail.com", MailingAddress = "18 Cross Drive, Cheadle Hulme, Cheadle" },
                new Patient { /*Id = 5, */FirstName = "Chloe", LastName = "Kim", DepartmentId = 4, PatientNumber = 239834678, BirthDate = DateTime.Parse("1953-10-2"), Gender = "Female", PhoneNumber = "+15674", EmailAddress = "kimchloe12@yahoo.com", MailingAddress = "3650 Rosecrans Street" },
                new Patient { /*Id = 6, */FirstName = "Troian", LastName = "Pattinson", DepartmentId=1, PatientNumber = 967545, BirthDate = DateTime.Parse("2001-1-8"), Gender = "Male", PhoneNumber = "+44565644767", EmailAddress = "troipat1@gmail.com", MailingAddress = "4545 LaJolla Village Dr" },
                new Patient { /*Id = 7, */FirstName = "Chris", LastName = "Smith", DepartmentId=6, PatientNumber = 0653457, BirthDate = DateTime.Parse("1965-6-22"), Gender = "Male", PhoneNumber = "+12767", EmailAddress = "chrissmith@live.com", MailingAddress = "1 Lightfoot Street, Hoole, Chester" },
                new Patient { /*Id = 8, */FirstName = "Thomas", LastName = "Jones", DepartmentId=3, PatientNumber = 9786754, BirthDate = DateTime.Parse("1972-11-3"), Gender = "Male", PhoneNumber = "07742 193218 ", EmailAddress = "joneeeees@gmail.com", MailingAddress = "1 The House, 2 The Road, Liverpool, L1 3BA" },
                new Patient { /*Id = 9, */FirstName = "Christina", LastName = "Stewart", DepartmentId = 1, PatientNumber = 398675345, BirthDate = DateTime.Parse("2011-11-3"), Gender = "Female", PhoneNumber = "+64478657", EmailAddress = "cstew334@gmail.com", MailingAddress = "3141 Crow Canyon Place" },

                new Patient { /*Id = 10, */FirstName = "Matthew", LastName = "David", DepartmentId = 6, PatientNumber = 344, BirthDate = DateTime.Parse("1939-1-8"), Gender = "Male", PhoneNumber = "+344512312367", EmailAddress = "matthewdav@yahoo.com", MailingAddress = "575 Market St" },
                new Patient { /*Id = 11, */FirstName = "Jennifer", LastName = "Hamptons", DepartmentId=2, PatientNumber = 123458753, BirthDate = DateTime.Parse("1962-6-4"), Gender = "Female", PhoneNumber = "+34876567", EmailAddress = "jennyhamptons@gmail.com", MailingAddress = "6123 Sunrise Blvd" },
                new Patient { /*Id = 12, */FirstName = "Sophie", LastName = "Horvat", DepartmentId = 4, PatientNumber = 7896, BirthDate = DateTime.Parse("1987-5-9"), Gender = "Other", PhoneNumber = "+376546767", EmailAddress = "sophor@hotmail.com", MailingAddress = "1640 E. Monte Vista Ave" },

                new Patient { /*Id = 13, */FirstName = "Layla", LastName = "Jepsen", PatientNumber = 07862, BirthDate = DateTime.Parse("1995-11-3"), Gender = "Female", PhoneNumber = "+323467", EmailAddress = "laylaj123@gmail.com", MailingAddress = "2521 Palomar Airport Rd" },
                new Patient { /*Id = 14, */FirstName = "Anne", LastName = "Dawson", DepartmentId = 5, PatientNumber = 8754, BirthDate = DateTime.Parse("2005-1-3"), Gender = "Female", PhoneNumber = "+5656767", EmailAddress = "dawsonne45@gmail.com", MailingAddress = "3151 Zinfandel Drive" },
                new Patient { /*Id = 15, */FirstName = "April", LastName = "Fitzgerald", DepartmentId = 6, PatientNumber = 78365345, BirthDate = DateTime.Parse("1973-3-9"), Gender = "Other", PhoneNumber = "+344342347", EmailAddress = "aprilfitz@hotmail.com", MailingAddress = "1110 Concord Avenue" }



                );
                context.SaveChanges();


                context.Appointment.AddRange(
                new Appointment {  /*Id = 1, */ PatientId = 1, DoctorId = 8, Date = DateTime.Parse("2021-5-14"), Symptoms = "She experiences occasional chest discomfort described as sharp in nature and on one occasion radiating through to her back, it has recently gotten worse with movement and associated with an inability to take a deep breath.", Examination = "Blood tests, coronary calcium scan. Normal results of an echocardiogram. No visible blood clots or tumors in the patient's heart. Irregular blood pressure", Diagnosis = "Diabetes mellitus type. Dyslipidemia. Obesity. Primary hypertension.", Therapy = "levothyroxine 75 mcg 1 tab(s) daily, valsartan 80 mcg 1 tab(s) every morning, bedoxine 2 tab(s) daily", MedicalReport = "medicalreports/audio_2.mp3" },
                new Appointment {  /*Id = 2, */ PatientId = 6, DoctorId = 1, Date = DateTime.Parse("2020-7-28"), Symptoms = "Difficulties with breathing.", Examination = "Fiber Nasopharyngoscopic examination - deviated nasal septum which causes difficulties, the airway is almost completely obstructed on the right side.", Diagnosis = "Deviated nasal septum", Therapy = "Recommended septorhinoplasty.", MedicalReport = "Book an appointment after the surgical procedure" },
                new Appointment {  /*Id = 3, */ PatientId = 11, DoctorId = 15, Date = DateTime.Parse("2021-3-7"), Symptoms = "59-year-old woman who presents for evaluation of “facial skin problems”. After one year with proper therapy, she has noticed an improvement in the texture of her skin and the brown spots on her face. She also has several growths on her legs.", Examination = "She has Fitzpatrick type 1 skin. Her lateral cheeks have several scattered brown to tan macules. Her right medial cheek has three 1 mm dark blue to gray macules. Her mid upper lip has a 2/3 mm pearly papule.", Diagnosis = "Possible basal cell carcinoma, upper lip, a biopsy needs to be done", Therapy = "Continue using Retin-A Micro.", MedicalReport = "medicalreports/audio_1.mp3" },
                new Appointment {  /*Id = 4, */ PatientId = 4, DoctorId = 6, Date = DateTime.Parse("2010-4-15"), Symptoms = "The patient suffered a forced double flexion/extension movement of her back and neck, had severe neck, and back pain, interfering with sleep.", Examination = "Examination of her upper limbs is normal.There was no spinal tenderness. There were no other marks  scars or bruises and no inappropriate responses.", Diagnosis = "Lower back strain.", Therapy = "Stretching and strengthening exercises.", MedicalReport = "medicalreports/janet_jones.mp3" },
                new Appointment {  /*Id = 5, */ PatientId = 2, DoctorId = 5, Date = DateTime.Parse("2019-11-12"), Symptoms = "Atypical chest pain.", Examination = "Blood tests done. High blood pressure", Diagnosis = "Hypertension. Costochondritis.", Therapy = "We will see the patient back in the office in four weeks and reevaluate blood pressure readings at that time, as to whether or not medical therapy will be necessary.", MedicalReport = "medicalreports/audio_4.mp3" },
                new Appointment {  /*Id = 6, */ PatientId = 8, DoctorId = 6, Date = DateTime.Parse("2011-6-16"), Symptoms = "Since buying tight footwear, the patient has been suffering from left medial foot arch pain.", Examination = "There is no bony tenderness. He is somewhat tender over the mid point of his medial arch on his left foot.", Diagnosis = "More examinations are needed in order to establish an accurate diagnosis.I wonder if this is unresolved plantar fasciitis and whether he would benefit from a steroid injection. I would be grateful for your expert opinion, Mr. Spring, because i think this is your field of work. ", Therapy = "He has unfortunately received no benefit from physiotherapy. He finds that the pain gets worse after he resumes his exercise regime", MedicalReport = "medicalreports/thomas_jones.mp3" },
                new Appointment {  /*Id = 7, */ PatientId = 7, DoctorId = 13, Date = DateTime.Parse("2010-4-15"), Symptoms = "The patient has neck and back pain.", Examination = "Examination of his cervical spine shows a full range of movement actively and passively in all directions.There was no muscular tenderness.Flexion/extension of his lumbosacral spine is full. Straight leg raising is full. There was no spinal tenderness.", Diagnosis = "As a result of a road traffic accident on the 10 February 2010, Chris Smith sustained a whiplash injury to his cervical spine and a lower back sprain.", Therapy = "Prescribed medications for pain relief", MedicalReport = "medicalreports/chris_smith.mp3" },
                new Appointment {  /*Id = 8, */ PatientId = 10, DoctorId = 5, Date = DateTime.Parse("2017-11-20"), Symptoms = "The patient has undergone periodic cystoscopies and on the recent cystoscopy a lesion was found. This has since been shown to be a recurrent bladder carcinoma and the patient is now scheduled for cystoscopic removal.", Examination = "The patient’s chest x-ray, EKG, and labs have been reviewed and he is cleared for surgery.", Diagnosis = "Reccurent bladder carcinoma. Hypercholesterolemia.", Therapy = "As his C-reactive protein and homocysteine are normal, the patient will attempt six months of diet and exercise. A diet sheet has been given for the patient to review.  This will be checked in six months, after healing up from the surgery.", MedicalReport = "medicalreports/audio_3.mp3" }
           
              
                

                 );


                context.SaveChanges();
            }
        }
    }
}
