
namespace MISA.Web01.Api.Model
{
    /// <summary>
    /// Nhân viên
    /// </summary>
    public class Employee
    {
        #region Declare
        #endregion

        #region Constructor
        #endregion

        #region Property
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên đầy đủ của nhân viên
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Giới tính 0-Nữ 1-Nam 2-Khác
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// Tên giới tính
        /// </summary>
        public string? GenderName
        {
            get
            {
                switch (Gender)
                {
                    case 0: return "Nữ"; break;
                    case 1: return "Nam"; break;
                    case 2: return "Khác"; break;
                    default: return "";

                }
            }
        }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số chứng minh nhân dân/ căn cước công dân
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp CMND
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp CMND
        /// </summary>
        public string? IdentityPlace { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Địa chỉ Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Điện thoại cố định
        /// </summary>
        public string? TelephoneNumber { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        public string? BankAccountNumber { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Tên chi nhánh ngân hàng
        /// </summary>
        public string? BankBranchName { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Vị trí làm việc
        /// </summary>
        public string? PositionName { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Method
        #endregion
    }
}
