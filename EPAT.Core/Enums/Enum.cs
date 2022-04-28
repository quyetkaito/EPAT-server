using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Enums
{
    //Enum giới tính
    public enum Gender
    {
        Female = 0, //Nữ
        Male = 1, //Nam
        Other = 2, //Khác
    }

    //Enum trạng thái validate
    public enum Mode
    {
        Insert = 0, //đang thêm mới
        Update = 1, //đang sửa
    }
}
