using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Entities
{
    public class Export
    {
        public List<TableInfo> tableInfos { get; set; }
        public string? dataFilter { get; set; }
    }
}
