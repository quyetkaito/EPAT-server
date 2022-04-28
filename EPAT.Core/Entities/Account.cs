using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Entities
{
    public class Account : BaseEntity
    {
        [TableColumn]
        [TableKey]
        public Guid account_id { get; set; }
        [TableColumn]
        public string account_name { get; set; }
        [TableColumn] public string username { get; set; }
        [TableColumn] public string password { get; set; }
        [TableColumn] public Guid position_id { get; set; }
    }
}
