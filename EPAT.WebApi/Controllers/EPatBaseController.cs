using EPAT.Core.Entities;
using EPAT.Core.Exceptions;
using EPAT.Core.Interfaces.Base;
using EPAT.Core.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace EPAT.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EPatBaseController<T> : ControllerBase where T : class
    {
        #region Declare
        //khai báo một interface base service
        IBaseService<T> _baseService;
        #endregion

        #region Constructor
        /// <summary>
        /// hàm khởi tạo misa base controller
        /// </summary>
        /// <param name="baseService">interface base service</param>
        /// <param name="baseRepository">interface base repo</param>
        public EPatBaseController(IBaseService<T> baseService)
        {
            _baseService = baseService;
        }

        #endregion

        #region Base API
        /// <summary>
        /// API lấy toàn bộ đối tượng
        /// </summary>
        /// <returns>
        /// 200 - Thành công
        /// 204 - Không có dữ liệu
        /// 500 - Lỗi server
        /// </returns>
        /// Author: quyetnv (09/03/2022)
        [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var entitis = _baseService.Get();
            if (entitis != null)
            {
                return Ok(entitis);
            }
            else
            {
                var message = new JSONMessage();
                message.UserMsg = ResourceVN.NoContent;
                message.DevMsg = ResourceVN.NoContent;
                message.StatusCode = 204;
                return Ok(message);
            }
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }


    /// <summary>
    /// API lấy 1 đối tượng theo id
    /// </summary>
    /// <param name="id">id client gửi lên</param>
    /// <returns>
    /// 200 - thành công
    /// 204 - không có kết quả
    /// 500 - lỗi server
    /// </returns>
    /// Author: quyetnv (09/03/2022)
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            var entity = _baseService.GetById(id);
            if (entity != null)
            {
                return Ok(entity);
            }
            else
            {
                var message = new JSONMessage();
                message.UserMsg = ResourceVN.NoContent;
                message.DevMsg = ResourceVN.NoContent;
                message.StatusCode = 204;
                return Ok(message);
            }
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }


    /// <summary>
    /// API Thêm mới một đối tượng
    /// </summary>
    /// <param name="entity">Một đối tượng</param>
    /// <returns>
    /// 201 - Thêm mới thành công
    /// 400 - Dữ liệu đầu vào không hợp lệ
    /// 500 - Có Exception
    /// </returns>
    /// Author:quyetnv (07/03/2022)
    [HttpPost]
    public IActionResult Post(T entity)
    {
        try
        {   //1. xử lý nghiệp vụ
            var res = _baseService.InsertService(entity);
            if (res > 0)
            {
                var message = new JSONMessage();
                message.UserMsg = ResourceVN.SuccessPost;
                message.DevMsg = ResourceVN.SuccessPost;
                message.StatusCode = 201;
                return StatusCode(201, message);
            }
            else
            {
                var message = new JSONMessage();
                message.UserMsg = ResourceVN.Error_Exception_Default;
                message.DevMsg = ResourceVN.Error_ValidateData;
                message.StatusCode = 400;
                return BadRequest(message);
            }

        }
        catch (ValidateException ex)
        {
            return HandleValidateException(ex);
        }
        catch (Exception ex)
        {

            return HandleException(ex);
        }

    }


    /// <summary>
    /// API Cập nhật thông tin một đối tượng
    /// </summary>
    /// <param name="entity">Một đối tượng</param>
    /// <returns>
    /// 201 - Chỉnh sửa thành công
    /// 400 - Dữ liệu đầu vào không hợp lệ
    /// 500 - Có Exception
    /// </returns>
    /// Author:quyetnv (07/03/2022)
    [HttpPut]
    public IActionResult Put(T entity)
    {
        try
        {
            //gọi tới service xử lý nghiệp vụ validate             
            var res = _baseService.UpdateService(entity);
            //trả kết quả cho client
            if (res > 0)
            {
                var message = new JSONMessage();
                message.UserMsg = ResourceVN.SuccessPut;
                message.DevMsg = ResourceVN.SuccessPut;
                message.StatusCode = 200;
                return Ok(message);
            }
            else
            {
                var message = new JSONMessage();
                message.UserMsg = ResourceVN.Error_Exception_Default;
                message.DevMsg = ResourceVN.Error_ValidateData;
                message.StatusCode = 400;
                return BadRequest(message);
            }
        }
        catch (ValidateException ex)
        {
            return HandleValidateException(ex);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }

    }


    /// <summary>
    /// API xóa một đối tượng
    /// </summary>
    /// <param name="id">id của đối tượng cần xóa</param>
    /// <returns>
    /// 200 - xóa thành công
    /// 500 - lỗi server
    /// </returns>
    /// Author: quyetnv (09/03/2022)
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            var res = _baseService.DeleteService(id);
            if (res > 0)
            {
                var message = new JSONMessage();
                message.UserMsg = ResourceVN.SuccessDelete;
                message.DevMsg = ResourceVN.SuccessDelete;
                message.StatusCode = 200;
                return StatusCode(200, message);
            }
            else
            {
                var message = new JSONMessage();
                message.UserMsg = ResourceVN.Error_Exception_Default;
                message.DevMsg = ResourceVN.Error_ValidateData;
                message.StatusCode = 400;
                return BadRequest(message);
            }

        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }


    /// <summary>
    /// API Xóa nhiều đối tượng cùng lúc
    /// </summary>
    /// <param name="ids">danh sách các id của đối tượng</param>
    /// <returns>một sối tượng thông báo dạng JSON</returns>
    /// Author: quyetnv (14/03/2022)
    [HttpDelete]
    public IActionResult MultiDelete([FromBody] Guid[] ids)
    {
        try
        {
            var res = _baseService.MultiDeleteService(ids);
            if (res > 0)
            {
                var message = new JSONMessage();
                message.UserMsg = ResourceVN.SuccessDelete;
                message.DevMsg = ResourceVN.SuccessDelete;
                message.StatusCode = 200;
                return StatusCode(200, message);
            }
            else
            {
                var message = new JSONMessage();
                message.UserMsg = ResourceVN.Error_Exception_Default;
                message.DevMsg = ResourceVN.Error_ValidateData;
                message.StatusCode = 400;
                return BadRequest(message);
            }
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// API filter dữ liệu, tìm kiếm, phân trang
    /// </summary>
    /// <param name="pageSize">Số bản ghi trên một trang</param>
    /// <param name="pageNumber">Trang số bao nhiêu</param>
    /// <param name="textFilter">Dữ liệu tìm kiếm từ client</param>
    /// <returns>
    /// 200 - Thành công
    /// 204 - Không có dữ liệu
    /// 500 - Lỗi server
    /// 400 - Đầu vào không hợp lệ
    /// </returns>
    /// Author: quyetnv (10/03/2022)
    [HttpGet("filter")]
    public IActionResult Filter([FromQuery] int pageSize, int pageNumber, string? textFilter) //xong
    {
        try
        {
            var res = _baseService.Filter(pageSize, pageNumber, textFilter);
            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return StatusCode(204);
            }


        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

        [HttpGet("paging")]
        public IActionResult Paging([FromQuery] int pageSize, int pageNumber)
        {
            try
            {
                var res = _baseService.Paging(pageSize, pageNumber);
                if (res != null)
                {
                    return Ok(res);
                }
                else
                {
                    return StatusCode(204);
                }

            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    #endregion

    #region Exception
    /// <summary>
    /// Xử lý exception.
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <returns>
    /// 500 - Có ngoại lệ
    /// </returns>
    /// Author: quyetnv (08/03/2022)
    protected IActionResult HandleException(Exception ex)

    {
        var error = new JSONMessage();
        error.DevMsg = ex.Message;
        error.UserMsg = ResourceVN.Error_Exception_Default;
        error.Data = ex.Data;
        return StatusCode(500, error);
    }


    /// <summary>
    /// Xử lý lỗi validate
    /// </summary>
    /// <param name="ex">exception</param>
    /// <returns>
    /// một đối tượng message dạng JSON
    /// </returns>
    /// Author: quyetnv (21/03/2022)
    protected virtual IActionResult HandleValidateException(ValidateException ex)
    {
        var error = new JSONMessage();
        error.DevMsg = ex.Message;
        error.UserMsg = ex.Message;
        error.StatusCode = 400;
        error.Data = ex.Data;
        return StatusCode(200, error);
    }
    #endregion

}
}
