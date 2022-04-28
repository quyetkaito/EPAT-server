using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web01.Api.Model;
using MySqlConnector;
using Dapper;

namespace MISA.Web01.Api.Controllers
{
    /// <summary>
    /// Controller xử lý các api dành cho nhân viên
    /// Author: quyetkaito (04/03/2022)
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string connectionString;



        /// <summary>
        /// Contructor config một số thông tin
        /// Author: quyetkaito (06/03/2022)
        /// </summary>
        public EmployeesController(IConfiguration config)
        {
            this._configuration = config;
            this.connectionString = _configuration.GetConnectionString("MyConnectionStrings");
        }


        /// <summary>
        /// Lấy danh sách toàn bộ nhân viên
        /// </summary>
        /// <returns>
        /// 200 - Danh sách khách hàng
        /// 204 - Không có dữ liệu
        /// </returns>
        /// Author: quyetkaito (04/03/2022)
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                //1.Khai báo thông tin database, lấy connectionString từ appsettings.json 
                var connectionString = this.connectionString;
                //Khởi tạo kết nối với MariaDB
                var sqlConnection = new MySqlConnection(connectionString);
                //2.lấy dữ liệu
                //2.1 Câu lệnh truy vấn lấy dữ liệu
                var sqlCommand = "SELECT * FROM Employee";
                var employees = sqlConnection.Query<object>(sql: sqlCommand);
                //2.2 Thực hiện lấy dữ liệu
                //trả kết quả cho client
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }


        }

        /// <summary>
        /// Lấy một nhân viên theo id
        /// </summary>
        /// <param name="employeeId">
        /// Id từ Client truyền lên
        /// </param>
        /// <returns>
        /// 200 - Dữ liệu 1 khách hàng
        /// 204 - Không có dữ liệu
        /// </returns>
        /// Author: quyetkaito (04/03/2022)
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                //1.Khai báo thông tin database
                var connectionString = this.connectionString;
                //Khởi tạo kết nối với MariaDB
                var sqlConnection = new MySqlConnection(connectionString);
                //2.lấy dữ liệu
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeId", id);
                //2.1 Câu lệnh truy vấn lấy dữ liệu
                var sqlCommand = $"SELECT * FROM Employee WHERE EmployeeId = @EmployeeId";
                var employee = sqlConnection.QueryFirstOrDefault<object>(sql: sqlCommand, param: parameters);
                //2.2 Thực hiện lấy dữ liệu
                //trả kết quả cho client
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

        }

        /// <summary>
        /// Thêm mới một nhân viên
        /// </summary>
        /// <param name="employee">Một đối tượng nhân viên</param>
        /// <returns>
        /// 201 - Thêm mới thành công
        /// 400 - Dữ liệu đầu vào không hợp lệ
        /// 500 - Có Exception
        /// </returns>
        /// Author:quyetkaito (07/03/2022)
        [HttpPost]
        public IActionResult Post(Employee employee)
        {
            try
            {
                //validate
                //check trùng mã, check bắt buộc nhập...
                var error = new ErrorService();
                var errorData = new List<Object>();
                var errorMsgs = new List<string>();

                if (string.IsNullOrEmpty(employee.EmployeeCode))
                {
                    //thông báo mã không được bỏ trống
                    errorData.Add(new { fieldName = "employeeCode", content = Resources.ResourceVN.Empty_EmployeeCode });
                    errorMsgs.Add(Resources.ResourceVN.Empty_EmployeeCode);
                }
                if (string.IsNullOrEmpty(employee.EmployeeName))
                {
                    //thông báo tên ko được bỏ trống
                    errorMsgs.Add(Resources.ResourceVN.Empty_EmployeeName);
                    errorData.Add(new { fieldName = "employeeName", content = Resources.ResourceVN.Empty_EmployeeName });
                }
                if ((employee.Email != null) && !IsValidEmail(employee.Email))
                {
                    errorMsgs.Add(Resources.ResourceVN.Invalid_Email);
                    errorData.Add(new { fieldName = "email", content = Resources.ResourceVN.Invalid_Email });
                }
                if (CheckEmployeeCode(employee.EmployeeCode))
                {
                    //thông báo trùng mã
                    errorMsgs.Add(Resources.ResourceVN.Duplicate_EmployeeCode);
                    errorData.Add(new { fieldName = "employeeCode", content = Resources.ResourceVN.Duplicate_EmployeeCode });
                }
                if (errorData.Count > 0)
                {
                    error.UserMsg = Resources.ResourceVN.Error_ValidateData;
                    error.Data = errorData; //danh sách các lỗi
                    return BadRequest(error);
                }

                //tạo mới EmployeeId
                employee.EmployeeId = Guid.NewGuid();

                //1.Khai báo thông tin database
                var connectionString = this.connectionString;
                //Khởi tạo kết nối với MariaDB
                var sqlConnection = new MySqlConnection(connectionString);
                var sqlCommandText = "Proc_InsertEmployee";

                //mở kết nối đến database
                sqlConnection.Open();

                //đọc các tham số đầu vào của store
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sqlCommandText;
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlCommandBuilder.DeriveParameters(sqlCommand);

                var dynamicParam = new DynamicParameters();
                foreach (MySqlParameter parameter in sqlCommand.Parameters)
                {
                    //lấy tên của tham số
                    var paramName = parameter.ParameterName;
                    var propName = paramName.Replace("@m_", "");
                    var propValue = employee.GetType().GetProperty(propName).GetValue(employee);
                    //thực hiện gán giá trị cho các param
                    dynamicParam.Add(paramName, propValue);
                }

                var res = sqlConnection.Execute(sql: sqlCommandText, param: dynamicParam, commandType: System.Data.CommandType.StoredProcedure);





                //trả kết quả cho client
                if (res > 0)
                {
                    return StatusCode(201, res);
                }
                else
                {
                    return StatusCode(400, error);
                }
            }
            catch (Exception ex)
            {

                return HandleException(ex);
            }

        }


        /// <summary>
        /// Cập nhật thông tin một nhân viên
        /// </summary>
        /// <param name="employee">Một đối tượng nhân viên</param>
        /// <returns>
        /// 201 - Chỉnh sửa thành công
        /// 400 - Dữ liệu đầu vào không hợp lệ
        /// 500 - Có Exception
        /// </returns>
        /// Author:quyetkaito (07/03/2022)
        [HttpPut("{id}")]
        public IActionResult Put(Employee employee, Guid id)
        {
            try
            {
                employee.EmployeeId = id;
                //validate
                //check trùng mã, check bắt buộc nhập...
                var error = new ErrorService();
                var errorData = new List<Object>();
                var errorMsgs = new List<string>();

                if (string.IsNullOrEmpty(employee.EmployeeCode))
                {
                    //thông báo mã không được bỏ trống
                    errorData.Add(new { fieldName = "employeeCode", content = Resources.ResourceVN.Empty_EmployeeCode });
                    errorMsgs.Add(Resources.ResourceVN.Empty_EmployeeCode);
                }
                if (string.IsNullOrEmpty(employee.EmployeeName))
                {
                    //thông báo tên ko được bỏ trống
                    errorMsgs.Add(Resources.ResourceVN.Empty_EmployeeName);
                    errorData.Add(new { fieldName = "employeeName", content = Resources.ResourceVN.Empty_EmployeeName });
                }
                if ((employee.Email != null) && !IsValidEmail(employee.Email))
                {
                    errorMsgs.Add(Resources.ResourceVN.Invalid_Email);
                    errorData.Add(new { fieldName = "email", content = Resources.ResourceVN.Invalid_Email });
                }
                if (CheckEmployeeCode(id, employee.EmployeeCode))
                {
                    //Thông báo trùng mã
                    errorData.Add(new { fieldName = "employeeCode", content = Resources.ResourceVN.Duplicate_EmployeeCode });
                    errorMsgs.Add(Resources.ResourceVN.Duplicate_EmployeeCode);
                }
                if (errorData.Count > 0)
                {
                    error.UserMsg = Resources.ResourceVN.Error_ValidateData;
                    error.Data = errorData; //danh sách các lỗi
                    return BadRequest(error);
                }


                //1.Khai báo thông tin database
                var connectionString = this.connectionString;
                //Khởi tạo kết nối với MariaDB
                var sqlConnection = new MySqlConnection(connectionString);
                var sqlCommandText = "Proc_UpdateEmployee";

                //mở kết nối đến database
                sqlConnection.Open();

                //đọc các tham số đầu vào của store
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sqlCommandText;
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlCommandBuilder.DeriveParameters(sqlCommand);

                var dynamicParam = new DynamicParameters();
                foreach (MySqlParameter parameter in sqlCommand.Parameters)
                {
                    //lấy tên của tham số
                    var paramName = parameter.ParameterName;
                    var propName = paramName.Replace("@m_", "");
                    var propValue = employee.GetType().GetProperty(propName).GetValue(employee);
                    //thực hiện gán giá trị cho các param
                    dynamicParam.Add(paramName, propValue);
                }

                var res = sqlConnection.Execute(sql: sqlCommandText, param: dynamicParam, commandType: System.Data.CommandType.StoredProcedure);

                //trả kết quả cho client
                if (res > 0)
                {
                    return StatusCode(201, res);
                }
                else
                {
                    return StatusCode(400, error);
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

        }

        /// <summary>
        /// Thực hiện xóa một nhân viên.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        /// Author: quyetkaito (04/03/2022)
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                //1.Khai báo thông tin database
                var connectionString = this.connectionString;
                //Khởi tạo kết nối với MariaDB
                var sqlConnection = new MySqlConnection(connectionString);
                //2.lấy dữ liệu
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeId", id);
                //2.1 Câu lệnh truy vấn lấy dữ liệu
                var sqlCommand = $"DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
                var employee = sqlConnection.QueryFirstOrDefault<object>(sql: sqlCommand, param: parameters);
                //2.2 Thực hiện lấy dữ liệu
                //trả kết quả cho client
                return Ok("Xóa thành công");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

        }

        /// <summary>
        /// Hàm xử lý xóa nhiều Employee
        /// </summary>
        /// <param name="employeeIds">list các id cần xóa</param>
        /// <returns>
        /// 200 - thành công
        /// 400 - dữ liệu không hợp lệ
        /// </returns>
        /// Author: quyetkaito (08/03/2022)
        [HttpDelete]
        public IActionResult Delete([FromBody] Guid[] employeeIds)
        {
            // Khởi tạo kết nối MariaDb
            var sqlConnection = new MySqlConnection(this.connectionString);
            // Câu lệnh truy vấn
            var sqlCommand = "DELETE FROM Employee WHERE EmployeeId IN (";
            DynamicParameters parameters = new DynamicParameters();
            // Xử lý chuỗi id client gửi lên
            for (int i = 0; i < employeeIds.Length; i++)
            {
                if (i != employeeIds.Length - 1)
                {
                    sqlCommand += $"@param{i}, ";
                    parameters.Add($"@param{i}", employeeIds[i]);
                }
                else
                {
                    sqlCommand += $"@param{i}";
                    parameters.Add($"@param{i}", employeeIds[i]);
                }
            }
            sqlCommand += ")";
            //Thực hiện truy vấn
            var res = sqlConnection.Execute(sql: sqlCommand, param: parameters);
            if (res > 0)
            {
                return Ok();
            }
            else
            {
                var error = new ErrorService();
                error.UserMsg = Resources.ResourceVN.Error_Exception_Default;
                error.DevMsg = Resources.ResourceVN.Error_Exception_Default;                
                return BadRequest(error);
            }
        }

        /// <summary>
        /// Filter dữ liệu nhân viên theo tên, mã, phân trang
        /// </summary>
        /// <param name="name">Tên nhân viên</param>
        /// <param name="code">Mã nhân viên</param>
        /// <returns>
        /// 200 - Danh sách nhân viên khớp
        /// 204 - Không có dữ liệu
        /// </returns>
        /// Author: quyetkaito (05/03/2022)
        [HttpGet("filter")]
        public IActionResult Filter([FromQuery] int pageSize, int pageNumber, string? employeeFilter)
        {
            try
            {
                //Khởi tạo kết nối với MariaDB
                var sqlConnection = new MySqlConnection(this.connectionString);

                if (string.IsNullOrEmpty(employeeFilter))
                {
                    employeeFilter = "";
                }
                //1.câu lệnh truy vấn số bản ghi phù hợp với employeeFilter
                var sqlEmployeeFilter = "SELECT COUNT(EmployeeId) FROM Employee WHERE EmployeeCode LIKE @employeeFilter OR EmployeeName LIKE @employeeFilter";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@employeeFilter", "%" + employeeFilter + "%");

                int totalRecord = int.Parse(sqlConnection.ExecuteScalar(sql: sqlEmployeeFilter, param: parameters).ToString());
                //tính tổng số       
                if (pageSize == 0) //số bản ghi 1 trang
                {
                    pageSize = totalRecord;
                }
                if (pageNumber == 0)
                {
                    pageNumber = 1;
                }
                if (totalRecord < pageSize)
                {
                    totalRecord = pageSize;
                }
                int totalPage = totalRecord / pageSize; //tổng số lượng các trang
                //offset - bỏ qua bao nhiêu bản ghi
                int offset = (pageSize * (pageNumber - 1));

                //2. Câu lệnh truy vấn dữ liệu
                var sqlEmployeeData = "SELECT * FROM Employee WHERE EmployeeCode LIKE @employeeFilter OR EmployeeName LIKE @employeeFilter LIMIT @Offset,@Limit";
                parameters.Add("@Offset", offset);
                parameters.Add("@Limit", pageSize);

                var employees = sqlConnection.Query<object>(sql: sqlEmployeeData, param: parameters);
                //build Data gửi về cho client
                var data = new
                {
                    Data = employees,
                    TotalRecord = totalRecord,
                    TotalPage = totalPage
                };
                //trả kết quả cho client
                return Ok(data);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Kiểm tra định dạng email.
        /// </summary>
        /// <param name="email">chuỗi email client gửi lên</param>
        /// <returns>
        /// true - đúng định dạng
        /// false - sai định dạng
        /// </returns>
        /// Author: quyetkaito(08/03/2022)
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
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


        /// <summary>
        /// Xử lý exception.
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>
        /// 500 - Exception
        /// </returns>
        /// Author: quyetkaito (08/03/2022)
        private IActionResult HandleException(Exception ex)
        {
            var error = new ErrorService();
            error.DevMsg = ex.Message;
            error.UserMsg = Resources.ResourceVN.Error_Exception_Default;
            error.Data = ex.Data;
            return StatusCode(500, error);
        }

        /// <summary>
        /// Hàm kiểm tra mã đã có trong hệ thống chưa khi thêm mới nhân viên.
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên</param>
        /// <returns>
        /// true - đã tồn tại trong hệ thống
        /// false - chưa có trong hệ thống
        /// </returns>
        /// Author: quyetkaito (08/03/2022)
        private bool CheckEmployeeCode(string employeeCode)
        {
            //1.Khai báo thông tin database
            var connectionString = _configuration.GetConnectionString("MyConnectionStrings");
            //Khởi tạo kết nối với MariaDB
            var sqlConnection = new MySqlConnection(connectionString);
            //2.lấy dữ liệu
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeCode", employeeCode);
            //2.1 Câu lệnh truy vấn lấy dữ liệu
            var sqlCommand = $"SELECT EmployeeCode FROM Employee WHERE EmployeeCode = @EmployeeCode";
            var res = sqlConnection.QueryFirstOrDefault<object>(sql: sqlCommand, param: parameters);
            //trả kết quả cho client
            if (res != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Hàm kiểm tra mã đã có trong hệ thống chưa khi update
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên</param>
        /// <param name="employeeId">Id nhân viên</param>
        /// <returns>
        /// true - đã tồn tại trong hệ thống
        /// false - chưa có trong hệ thống
        /// </returns>
        /// Author: quyetkaito (08/03/2022)
        private bool CheckEmployeeCode(Guid employeeId, string employeeCode)
        {
            //1.Khai báo thông tin database
            var connectionString = _configuration.GetConnectionString("MyConnectionStrings");
            //Khởi tạo kết nối với MariaDB
            var sqlConnection = new MySqlConnection(connectionString);
            //2.lấy dữ liệu
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeCode", employeeCode);
            parameters.Add("@EmployeeId", employeeId);
            //2.1 Câu lệnh truy vấn lấy dữ liệu
            var sqlCommand = $"SELECT EmployeeCode FROM Employee WHERE EmployeeCode = @EmployeeCode AND EmployeeId NOT IN (@EmployeeId)";
            var res = sqlConnection.QueryFirstOrDefault<string>(sql: sqlCommand, param: parameters);
            //trả kết quả cho client
            if (res != null)
            {
                return true;
            }
            return false;
        }
    }
}

