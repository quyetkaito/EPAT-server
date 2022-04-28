using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Entities
{
    /// <summary>
    /// Attribute dùng để đánh dấu các trường bắt buộc nhập
    /// </summary>
    /// Author: quyetnv (21/03/2022)
    [AttributeUsage(AttributeTargets.Property)]
    public class MISARequired : Attribute
    {
        /// <summary>
        /// Thông tin lỗi khởi tạo
        /// </summary>
        public string? messageError;

        /// <summary>
        /// Hàm khởi tạo Attribute kèm theo message
        /// </summary>
        /// <param name="errorMgs">Giá trị message khởi tạo kèm theo attribute</param>
        /// Author: quyetnv (21/03/2022)
        public MISARequired(string? errorMgs = null)
        {
            messageError = errorMgs;
        }
        public MISARequired()
        {

        }
    }

    /// <summary>
    /// Attribute dùng để dánh dấu trường email
    /// </summary>
    /// Author: quyetnv(21/03/2022)
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailValid : Attribute
    {
    }

    /// <summary>
    /// Attribute dùng để định danh property của đối tượng
    /// </summary>
    /// Author: quyetkaito (21/03/2022)
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyNameDisplay : Attribute
    {
        public string propName = string.Empty;

        /// <summary>
        /// Hàm khởi tạo kèm theo giá trị tên của property
        /// </summary>
        /// <param name="name">tên của property</param>
        /// Author: quyetnv (21/03/2022)
        public PropertyNameDisplay(string name)
        {
            propName = name;
        }
    }

    /// <summary>
    /// Custom attribute định nghĩa 1 thuộc tính là cột map với cơ sở dữ liệu
    /// </summary>
    /// Author: quyetkaito (21/04/2022)
    [AttributeUsage(AttributeTargets.Property)]
    public class TableColumn : Attribute
    {

    }

    /// <summary>
    /// Custom attribute định nghĩa 1 thuộc tính là khóa chính của bảng trong csdl
    /// </summary>
    /// Author: quyetkaito (21/04/2022)
    [AttributeUsage(AttributeTargets.Property)]
    public class TableKey : Attribute
    {

    }

    /// <summary>
    /// Custom attribute định nghĩa 1 thuộc tính là unique trong csdl
    /// </summary>
    /// Author: quyetkaito (21/04/2022)
    [AttributeUsage(AttributeTargets.Property)]
    public class TableUnique : Attribute
    {

    }
}
