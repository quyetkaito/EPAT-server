using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Interfaces.Base
{
    /// <summary>
    /// Interface base service, liệt kê các nghiệp vụ có thể xử lý chung cho các đối tượng khác nhau.
    /// </summary>
    /// <typeparam name="T">Đối tượng tự định nghĩa</typeparam>
    /// Author: quyetnv (18/03/2022)
    public interface IBaseService<T> where T : class
    {
        /// <summary>
        /// base service thêm mới đối tượng
        /// </summary>
        /// <param name="entity">đối tượng cần thêm</param>
        /// <returns>Số bản ghi được thêm mới</returns>
        /// Author: quyetnv (12/03/2022)
        public int InsertService(T entity);


        /// <summary>
        /// base service cập nhật đối tượng
        /// </summary>
        /// <param name="entity">đối tượng cần sửa</param>
        /// <returns>Số bản ghi được update</returns>
        /// Author: quyetnv (12/03/2022)
        public int UpdateService(T entity);


        /// <summary>
        /// base servive xóa đối tượng
        /// </summary>
        /// <param name="entityId">id của đối tượng</param>
        /// <returns>Số bản ghi được xóa</returns>
        public int DeleteService(Guid entityId);


        /// <summary>
        /// base service xóa nhiều
        /// </summary>
        /// <param name="entityIds">danh sách các id của đối tượng cần xóa</param>
        /// <returns>Số bản ghi được xóa</returns>
        /// Author: quyetnv (14/03/2022)
        public int MultiDeleteService(Guid[] entityIds);


        /// <summary>
        /// base service phân trang, tìm kiếm
        /// </summary>
        /// <param name="pageSize">số bản ghi một trang</param>
        /// <param name="pageNumber">trang số bao nhiêu</param>
        /// <param name="textSearch">cụm từ tìm kiếm</param>
        /// <returns>Một đối tượng bao gồm tổng số trang, tổng số bản ghi, dữ liệu đối tượng</returns>
        /// Author: quyetnv (18/03/2022)
        public object Filter(int pageSize, int pageNumber, string? textSearch);

        public IEnumerable<T> Get();
        public T GetById(Guid id);
        public Object Paging(int pageSize, int pageNumber);
    }
}
