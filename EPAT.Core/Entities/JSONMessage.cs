using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Entities
{
    /// <summary>
    /// Đối tượng message để gửi lại client khi API được gọi.
    /// </summary>
    /// Author: quyetkaito (12/03/2022)
    public class JSONMessage
    {
        /// <summary>
        /// Thông báo cho lập trình viên
        /// </summary>
        public string? DevMsg { get; set; }

        /// <summary>
        /// Thông báo cho người sử dụng 
        /// </summary>
        public string? UserMsg { get; set; }

        /// <summary>
        /// Mã lỗi tự định nghĩa (theo tổ chức)
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// Hỗ trợ cho dev tìm hiểu thêm thông tin về lỗi
        /// </summary>
        public string? MoreInfor { get; set; }

        /// <summary>
        /// một object Data trả về
        /// </summary>
        public object? Data { get; set; }
    }
}
