using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAssistantProject.Models
{
    public class HealthFacility
    {
        public int hospital_id { get; set; }
        public string hospital_name { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public double distance { get; set; }
    }
}
