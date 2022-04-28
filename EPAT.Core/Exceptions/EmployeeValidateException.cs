using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Exceptions
{
    /// <summary>
    /// Class chứa thông tin lỗi validate riêng của employee
    /// </summary>
    /// Author: quyetnv (21/03/2022)
    public class EmployeeValidateException : Exception
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
        /// Hàm khởi tạo kèm theo danh sách lỗi và tiêu đề lỗi
        /// </summary>
        /// <param name="errorMsg">Thông báo lỗi</param>
        /// <param name="listError">Danh sách các lỗi</param>
        /// Author: quyetnv(21/03/2022)
        public EmployeeValidateException(string? errorMsg, List<object> listError)
        {
            ErrorMsg = errorMsg;
            Errors = listError;
        }

        /// <summary>
        /// Nạp chồng lại trường message của Exception mặc định
        /// </summary>
        public override string Message => ErrorMsg;
    }
}
