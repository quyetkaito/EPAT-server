using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Entities
{
    /// <summary>
    /// đối tượng login
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        /// tên đăng nhập
        /// </summary>
        public String username { get; set; }

        /// <summary>
        /// mật khẩu
        /// </summary>
        public String  password { get; set; }
    }
}
