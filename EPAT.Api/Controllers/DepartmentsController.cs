using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web01.Api.Model;
using MySqlConnector;

namespace MISA.Web01.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        /// <summary>
        /// Lấy toàn bộ danh sách phòng ban
        /// </summary>
        /// <returns>
        /// 200 - Danh sách phòng ban
        /// 204 - Không có dữ liệu
        /// </returns>
        /// Author: quyetkaito (05/03/2022)
        [HttpGet]
        public IActionResult Get()
        {
            //1.Khai báo thông tin database
            var connectionString = "Host=13.229.200.157; Port=3306; Database = MISA.WEB01.QUYETKAITO; User Id = dev; Password = 12345678";
            //Khởi tạo kết nối với MariaDB
            var sqlConnection = new MySqlConnection(connectionString);
            //2.lấy dữ liệu
            //2.1 Câu lệnh truy vấn lấy dữ liệu
            var sqlCommand = "SELECT * FROM Department";
            var departments = sqlConnection.Query<object>(sql: sqlCommand);
            //2.2 Thực hiện lấy dữ liệu
            //trả kết quả cho client
            return Ok(departments);
        }


        /// <summary>
        /// Lấy phòng ban/đơn vị theo mã
        /// </summary>
        /// <returns>
        /// 200 - Danh sách phòng ban
        /// 204 - Không có dữ liệu
        /// </returns>
        /// Author: quyetkaito (08/03/2022)
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            //1.Khai báo thông tin database
            var connectionString = "Host=13.229.200.157; Port=3306; Database = MISA.WEB01.QUYETKAITO; User Id = dev; Password = 12345678";
            //Khởi tạo kết nối với MariaDB
            var sqlConnection = new MySqlConnection(connectionString);
            //2.lấy dữ liệu
            //2.1 Câu lệnh truy vấn lấy dữ liệu
            var sqlCommand = "SELECT * FROM Department";
            var departments = sqlConnection.Query<object>(sql: sqlCommand);
            //2.2 Thực hiện lấy dữ liệu
            //trả kết quả cho client
            return Ok(departments);
        }


        [HttpPost]
        public IActionResult Post()
        {
            return Ok("post");
        }

        [HttpDelete("{departmentId}")]
        public IActionResult Delete(string departmentId)
        {
            //1.Khai báo thông tin database
            var connectionString = "Host=13.229.200.157; Port=3306; Database = MISA.WEB01.QUYETKAITO; User Id = dev; Password = 12345678";
            //Khởi tạo kết nối với MariaDB
            var sqlConnection = new MySqlConnection(connectionString);
            //2.lấy dữ liệu
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DepartmentId", departmentId);
            //2.1 Câu lệnh truy vấn lấy dữ liệu
            var sqlCommand = $"DELETE FROM Department WHERE DepartmentId = @DepartmentId";
            var department = sqlConnection.Query<object>(sql: sqlCommand, param: parameters);
            //2.2 Thực hiện lấy dữ liệu
            //trả kết quả cho client
            return Ok("Xóa thành công");
        }

    }
}
