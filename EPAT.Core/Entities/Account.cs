using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Entities
{
    //thông tin người dùng
    public class Account : BaseEntity
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        [TableColumn]
        [TableKey]        
        public Guid account_id { get; set; }

        /// <summary>
        /// tên
        /// </summary>
        [TableColumn]
        public string account_name { get; set; }

        /// <summary>
        /// tài khoản đăng nhập
        /// </summary>
        [TableColumn] public string username { get; set; }

        /// <summary>
        /// mật khẩu dăng nhập
        /// </summary>
        [TableColumn] public string password { get; set; }

        /// <summary>
        /// vị trí làm việc
        /// </summary>
        [TableColumn] public Guid department_id { get; set; }
    }
}
