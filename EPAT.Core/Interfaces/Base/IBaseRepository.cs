using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Interfaces.Base
{
    /// <summary>
    /// Interface liệt kê các công việc chung của các interface khác khi tương tác với database
    /// </summary>
    /// <typeparam name="T">Một đối tượng tự định nghĩa chưa có giá trị</typeparam>
    /// Author: quyetnv (18/03/2022)
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Interface lấy toàn bộ bảng dữ liệu
        /// </summary>
        /// <returns>Toàn bộ danh sách đối tượng</returns>
        /// Author: quyetnv (12/03/2022)
        public IEnumerable<T> Get();


        /// <summary>
        /// Interface lấy một đối tượng theo id.
        /// </summary>
        /// <param name="id">id của đối tượng</param>
        /// <returns>Một đối tượng</returns>
        /// Author: quyetnv (12/03/2022)
        public T GetById(Guid entityId);


        /// <summary>
        /// Interface thêm mới một đối tượng 
        /// </summary>
        /// <param name="entity">một đối tượng truyền lên từ client</param>
        /// <returns>Số bản ghi được thêm mới</returns>
        /// Author: quyetnv (12/03/2022)
        public int Insert(T entity);


        /// <summary>
        /// Interface cập nhật thông tin 1 đối tượng
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns>Số bản ghi được update</returns>
        /// Author: quyetnv(18/03/2022)
        public int Update(T entity);


        /// <summary>
        /// Interface xóa một đối tượng theo id.
        /// </summary>
        /// <param name="id">id của đối tượng</param>
        /// <returns>Số bản ghi được xóa</returns>
        /// Author: quyetnv (12/03/2022)
        public int Delete(Guid entityId);


        /// <summary>
        /// Interface phân trang, tìm kiếm 
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên một trang</param>
        /// <param name="pageNumber">Trang số bao nhiêu</param>
        /// <param name="textSearch">Thông tin tìm kiếm</param>
        /// <returns>Một đối tượng bao gồm tổng số trang, tổng số bản ghi, dữ liệu đối tượng</returns>
        /// Author: quyetnv (12/03/2022)
        public object Filter(int pageSize, int pageNumber, string? textSearch);

        /// <summary>
        /// Interface xóa nhiều đối tượng
        /// </summary>
        /// <param name="entityIds">Danh sách id của các đối tượng cần xóa</param>
        /// <returns>Số bản ghi bị xóa</returns>
        /// Author: quyetnv (14/03/2022)
        public int MultiDelete(Guid[] entityIds);
        public Object Paging(int pageSize, int pageNumber);
        public string GetTableKey();
    }
}
