using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAssistantProject.Models
{
    public class Doctor
    {
        public int doctor_id { get; set; }
        public string doctor_name { get; set; }
        public string doctor_prof_name { get; set; }
        public string doctor_bio { get; set; }
        public string doctor_telephone { get; set; }
        public string doctor_email { get; set; }
        public string hospital_name { get; set; }
        public string town { get; set; }
        public string address_str { get; set; }
        public string type_name { get; set; }
        public int hospital_id { get; set; }
        public float latitude { get; set; } 
        public float longitude { get; set; }
        public int type_id { get; set; }

    }
}
