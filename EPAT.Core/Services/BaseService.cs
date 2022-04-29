using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAT.Core.Interfaces.Base;
using EPAT.Core.Entities;
using EPAT.Core.Enums;
using EPAT.Core.Exceptions;
using EPAT.Core.Resources;

namespace EPAT.Core.Services
{
    /// <summary>
    /// Base Service xử lý các nghiệp vụ chung cho các đối tượng
    /// </summary>
    /// <typeparam name="T">Tên đối tượng</typeparam>
    /// Author: quyetnv (17/03/2022)
    public class BaseService<T> : IBaseService<T> where T : class
    {

        #region Declare
        IBaseRepository<T> _baseRepository;
        #endregion


        #region Constructor
        /// <summary>
        /// Hàm khởi tạo baseservice với tham số
        /// </summary>
        /// <param name="baseRepository">interface base repository</param>
        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        #endregion


        /// <summary>
        /// Xử lý nghiệp vụ chung khi thêm mới dữ liệu 
        /// </summary>
        /// <param name="entity">đối tượng thao tác</param>
        /// <returns>số bản ghi được thêm</returns>
        /// Author: quyetnv (12/03/2022)
        public int InsertService(T entity)
        {
            //Xử lý nghiệp vụ validate
            //check trùng mã, check bắt buộc nhập...
            var isValid = ValidateObject(entity, Mode.Insert);
            if (isValid == true)
            {
                //nếu hợp lệ => thêm dữ liệu vào db
                var res = _baseRepository.Insert(entity);
                return res;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Xử lý nghiệp vụ chung khi cập nhật dữ liệu 
        /// </summary>
        /// <param name="entity">đối tượng có thể xử lý nghiệp vụ chung</param>
        /// <returns>số bản ghi được update</returns>
        /// Author: quyetnv (12/03/2022)
        public int UpdateService(T entity)
        {
            //Xử lý nghiệp vụ validate
            var isValid = ValidateObject(entity, Mode.Update);
            if (isValid == true)
            {
                //nếu hợp lệ => cập nhật dữ liệu vào db
                var res = _baseRepository.Update(entity);
                return res;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Xử lý nghiệp vụ chung khi xóa dữ liệu
        /// </summary>
        /// <param name="entityId">id đối tượng cần xóa</param>
        /// <returns>số bản ghi được xóa</returns>
        /// Author: quyetnv (12/03/2022)
        public int DeleteService(Guid entityId)
        {
            return _baseRepository.Delete(entityId);
        }

        /// <summary>
        /// Service xóa nhiều
        /// </summary>
        /// <param name="entityIds">danh sách các id đối tượng cần xóa</param>
        /// <returns>số bản ghi được xóa</returns>
        /// Author: quyetnv (14/03/2022)
        public int MultiDeleteService(Guid[] entityIds)
        {
            return _baseRepository.MultiDelete(entityIds);
        }


        /// <summary>
        /// Service xử lý đầu vào filter
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên một trang</param>
        /// <param name="pageNumber">Trang số mấy</param>
        /// <param name="textSearch">Cụm từ tìm kiếm</param>
        /// <returns>
        /// Một đối tượng bao gồm tổng số trang, tổng số bản ghi, dữ liệu đối tượng
        /// </returns>
        /// Author: quyetnv (16/03/2022)
        public object Filter(int pageSize, int pageNumber, string? textSearch)
        {
            //xử lý nghiệp vụ, dữ liệu nhập vào <=0
            if (pageNumber < 1 || string.IsNullOrEmpty(pageNumber.ToString())) pageNumber = 1;
            if (string.IsNullOrEmpty(textSearch)) textSearch = "";
            if (pageSize < 1 || string.IsNullOrEmpty(pageSize.ToString())) pageSize = 10;

            //gọi repo truy vấn
            return _baseRepository.Filter(pageSize, pageNumber, textSearch);
        }


        /// <summary>
        /// Xử lý validate các nghiệp vụ chunng
        /// </summary>
        /// <param name="entity">đối tượng cần validate</param>
        /// <param name="mode">trạng thái là thêm mới hay sửa</param>
        /// <returns>
        /// true - hợp lệ/false - không hợp lệ
        /// </returns>
        /// Author: quyetnv (16/03/2022)
        protected virtual bool ValidateObject(T entity, Mode mode)
        {
            var isValid = true;
            //biến chứa các thông tin không hợp lệ
            var errorData = new Dictionary<string, string>();
            //validate chung
            //quét toàn bộ các prop có att đánh dấu validate chung
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                //lấy ra tên của prop
                var propName = property.Name;
                //lấy giá trị của prop
                var propValue = property.GetValue(entity);

                //lấy ra custom attribute thông tin tên cột
                var columnName = Attribute.GetCustomAttribute(property, typeof(PropertyNameDisplay));
                if (columnName != null)
                {
                    propName = (columnName as PropertyNameDisplay).propName;
                }

                //kiểm tra có att đánh dấu bắt buộc nhập hay không?
                var isRequired = Attribute.IsDefined(property, typeof(Required));
                if (isRequired == true && (propValue == null || propValue.ToString() == string.Empty))
                {
                    isValid = false;
                    //lấy ra errorMgs của att bắt buộc nhập
                    var requiredAtt = Attribute.GetCustomAttribute(property, typeof(Required));
                    var errorMsg = (requiredAtt as Required).messageError;
                    if (errorMsg == null)
                    {
                        errorMsg = string.Format(ResourceVN.Empty_PropName, propName);
                    }

                    //gán cảnh báo, text thông báo
                    errorData.Add(property.Name, errorMsg);
                }

                //kiểm tra có attribute là email hay không?
                var isEmail = Attribute.IsDefined(property, typeof(EmailValid));
                if (isEmail == true && propValue != null && propValue.ToString() != string.Empty)
                {
                    var checkMail = IsValidEmail(propValue.ToString());
                    if (checkMail == false)
                    {
                        //false
                        isValid = false;
                        //gán cảnh báo, text thông báo
                        errorData.Add(property.Name, ResourceVN.Invalid_Email);
                    }

                }
            }

            //xử lý validate riêng cho từng đối tượng
            ValidateCustom(entity, errorData, mode);


            //trả về danh sách lỗi nếu có
            if (errorData.Count > 0)
            {
                throw new ValidateException(ResourceVN.Error_ValidateData, errorData);
            }
            else
            {
                if(mode == Mode.Insert)
                {
                    //tạo mã mới cho key
                    var tableKey = _baseRepository.GetTableKey();
                    entity.GetType().GetProperty(tableKey).SetValue(entity, Guid.NewGuid());
                    entity.GetType().GetProperty("created_date").SetValue(entity, DateTime.Now);
                    entity.GetType().GetProperty("modified_date").SetValue(entity, DateTime.Now);
                }
                if(mode == Mode.Update)
                {
                    entity.GetType().GetProperty("modified_date").SetValue(entity, DateTime.Now);
                }

            }           

            return isValid;
        }


        /// <summary>
        /// Hàm virutal validate với mỗi đối tượng khác nhau
        /// </summary>
        /// <param name="entity">đối tượng validate</param>
        /// <param name="errorData">Danh sách các lỗi hiện tại</param>
        /// <returns>
        /// true - hợp lệ;
        /// false - không hợp lệ
        /// </returns>
        /// Author: quyetnv (15/03/2022)
        protected virtual bool ValidateCustom(T entity, Dictionary<string, string> errorData, Mode mode)
        {
            return true;
        }



        /// <summary>
        /// Kiểm tra định dạng email.
        /// <param name="email">chuỗi email client gửi lên</param>
        /// <returns>
        /// true - đúng định dạng
        /// false - sai định dạng
        /// </returns>
        /// Author: quyetnv(08/03/2022)
        /// </summary>
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<T> Get()
        {
            return _baseRepository.Get();   
        }

        public object Paging(int pageSize, int pageNumber)
        {
            //xử lý nghiệp vụ, dữ liệu nhập vào <=0
            if (pageNumber < 1 || string.IsNullOrEmpty(pageNumber.ToString())) pageNumber = 1;
            if (pageSize < 1 || string.IsNullOrEmpty(pageSize.ToString())) pageSize = 10;

            //gọi repo truy vấn
            return _baseRepository.Paging(pageSize, pageNumber);
        }

        public T GetById(Guid id)
        {
            return _baseRepository.GetById(id);
        }
    }
}
