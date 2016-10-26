using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAssistantProject.Models
{
   public class Illness
   {
        public int illness_id { get; set; }
        public string illness_name { get; set; }
        public string illness_descr { get; set; }
        public int type_id { get; set; }
        public string type_name { get; set; }
        public string illness_link { get; set; }

    }
}
