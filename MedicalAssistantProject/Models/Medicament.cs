using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAssistantProject.Models
{
    public class Medicament
    {
        public int med_id { get; set; }
        public string med_name { get; set; }
        public string med_descr { get; set; }
        public string med_before_use { get; set; }
        public string med_how_to_use { get; set; }
        public string med_side_effect { get; set; }
        public string med_how_to_storage { get; set; }
        public string med_additional_info { get; set; }
    }
}
