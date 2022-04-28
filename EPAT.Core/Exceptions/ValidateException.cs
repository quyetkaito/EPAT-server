using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Exceptions
{
    /// <summary>
    /// Custom validate exception dùng chung cho các đối tượng.
    /// </summary>
    /// Author: quyetnv(21/03/2022)
    public class ValidateException : Exception
    {
        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// Danh sách các lỗi
        /// </summary>
        public List<object> Errors { get; set; }

        /// <summary>
        /// Danh sách các lỗi 
        /// </summary>
        IDictionary ErrorData;

        /// <summary>
        /// Hàm khởi tạo 
        /// </summary>
        /// <param name="errorMsg">Thông báo lỗi</param>
        /// <param name="errorData">Danh sách các lỗi</param>
        /// Author: quyetnv (21/03/2022)
        public ValidateException(string? errorMsg, IDictionary errorData)
        {
            ErrorMsg = errorMsg;
            ErrorData = errorData;
        }
        /// <summary>
        /// nạp chồng lại message của exception
        /// </summary>
        public override string Message => ErrorMsg;

        /// <summary>
        /// Nạp chồng lại phương data của exception mặc định
        /// </summary>
        public override IDictionary Data => ErrorData;
    }
}
