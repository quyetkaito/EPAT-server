using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Entities
{
    public class Department:BaseEntity
    {
        public Guid department_id { get; set; }
        public string department_code { get; set; }
        public string department_name { get; set; }
    }
}
