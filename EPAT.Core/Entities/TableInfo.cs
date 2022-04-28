using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Entities
{
    /// <summary>
    /// Thông tin bảng dùng để map với list header mà client gửi lên khi xuất excel
    /// </summary>
    /// Author: quyetnv (21/03/2022)
    public class TableInfo
    {
        /// <summary>
        /// Tên cột dữ liệu
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Tên đầy đủ của cột dữ liệu
        /// </summary>
        public string? TextFull { get; set; }

        /// <summary>
        /// Tên cột tương ứng trong bảng cơ sở dữ liệu
        /// </summary>
        public string? FieldName { get; set; }

        /// <summary>
        /// Căn lề 
        /// </summary>
        public string? Align { get; set; }
    }
}
