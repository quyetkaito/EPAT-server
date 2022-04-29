using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Entities
{
    /// <summary>
    /// vị trí làm việc
    /// </summary>
    public class Department:BaseEntity
    {
        /// <summary>
        /// khóa chính
        /// </summary>
        [TableKey]
        [TableColumn]
        public Guid department_id { get; set; }
        
        /// <summary>
        /// mã vị trí làm việc
        /// </summary>
        [TableColumn]
        public string department_code { get; set; }

        /// <summary>
        /// tên vị trí làm việc
        /// </summary>

        [TableColumn]
        public string department_name { get; set; }
    }
}
