using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Entities    
{
    /// <summary>
    /// thông tin bệnh nhân
    /// </summary>
    public class Patient:BaseEntity
    {
        /// <summary>
        /// khóa chính
        /// </summary>
        [TableKey]
        [TableColumn] public Guid patient_id { get; set; }

        /// <summary>
        /// tên đầy đủ
        /// </summary>
        [TableColumn] public string fullname { get; set; }
        
        /// <summary>
        /// Ngày sinh
        /// </summary>        
        [TableColumn] public DateTime date_of_birth { get; set; }

        /// <summary>
        /// Giới tính - 0 - nữ, 1 nam, 2 khác
        /// </summary>
        [TableColumn] public int gender { get; set; }

        /// <summary>
        /// số chứng minh nhân dân
        /// </summary>
        [TableColumn] public string identity_number { get; set; }


        /// <summary>
        /// Số điện thoại
        /// </summary>
        [TableColumn] public string phone_number { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [TableColumn] public string address{ get; set; }

        /// <summary>
        /// mô tả
        /// </summary>
        [TableColumn] public string description { get; set; }
    }
}
