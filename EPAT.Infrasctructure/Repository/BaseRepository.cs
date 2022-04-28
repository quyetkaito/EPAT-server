using MySqlConnector;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using EPAT.Core.Entities;

namespace EPAT.Infrasctructure.Repository
{
    /// <summary>
    /// Thực hiện các công việc có cơ chế chung, chỉ khác đối tượng (lấy toàn bộ,
    /// lấy 1, thêm, sửa, xóa, xóa nhiều, tìm kiếm, phân trang).
    /// </summary>
    /// Author: quyetnv (11/03/2022)
    public class BaseRepository<MISAEntity> where MISAEntity : class
    {

        #region Field
        //chuỗi kết nối local        
        //protected string ConnectionString = "Host=localhost; Port=3306; Database = benhnhandb; User Id = root; Password = ''";
        //server Quyết
        protected string ConnectionString = "Host=168.138.171.44; Port=3306; Database = EpatDB; User Id = quyetkaito; Password = Quyet@1234";
        string tableName = string.Empty;
        #endregion

        #region Constructor
        public BaseRepository()
        {
            //lấy tên bảng dữ liệu
            tableName = ToUnderscoreCase(typeof(MISAEntity).Name);
        }
        #endregion

        #region Base REPO                                                                    

        /// <summary>
        /// Lấy toàn bộ bảng dữ liệu
        /// </summary>
        /// <returns>Toàn bộ danh sách đối tượng</returns>
        /// Author: quyetnv (12/03/2022)
        public virtual IEnumerable<MISAEntity> Get() //xong oke
        {
            //khởi tạo kết nối
            using (var sqlConnection = new MySqlConnection(ConnectionString))
            {
                var sqlCommand = $"SELECT * FROM {tableName}";
                //thực hiện truy vấn
                var entities = sqlConnection.Query<MISAEntity>(sql: sqlCommand);
                return entities;
            }
        }


        /// <summary>
        /// Lấy một đối tượng theo id.
        /// </summary>
        /// <param name="id">id của đối tượng</param>
        /// <returns>Một đối tượng</returns>
        /// Author: quyetnv (12/03/2022)
        public MISAEntity GetById(Guid id) //xong oke
        {
            //lấy tên cột là id của bảng
            var tableId = GetTableKey();
            //Khởi tạo kết nối với MariaDB
            using (var sqlConnection = new MySqlConnection(ConnectionString))
            {
                //truyền para
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);

                //Câu lệnh truy vấn lấy dữ liệu
                var sqlCommand = $"SELECT * FROM {tableName} WHERE {tableId} = @id";
                var res = sqlConnection.QueryFirstOrDefault<MISAEntity>(sql: sqlCommand, param: parameters);

                //trả kết quả cho client
                return res;
            }
        }



        /// <summary>
        /// Xóa một đối tượng theo id.
        /// </summary>
        /// <param name="id">id của đối tượng</param>
        /// <returns>số bản ghi bị xóa</returns>
        /// Author: quyetkaito (12/03/2022)
        public int Delete(Guid id) //xong oke
        {
            //lấy tên id của bảng
            var tableId = GetTableKey();
            //Khởi tạo kết nối với MariaDB
            using (var sqlConnection = new MySqlConnection(ConnectionString))
            {
                //truyền para
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);

                //Câu lệnh truy vấn lấy dữ liệu
                var sqlCommand = $"DELETE FROM {tableName} WHERE {tableId} = @id";
                var res = sqlConnection.Execute(sql: sqlCommand, param: parameters);

                //trả kết quả cho client
                return res;
            }
        }


        /// <summary>
        /// Thêm mới một đối tượng 
        /// </summary>
        /// <param name="entity">một đối tượng truyền lên từ client</param>
        /// <returns>số bản ghi được thêm</returns>
        /// Author: quyetnv (12/03/2022)
        /// <summary>
        /// Thêm mới một đối tượng 
        /// </summary>
        /// <param name="entity">một đối tượng truyền lên từ client</param>
        /// <returns>số bản ghi được thêm</returns>
        /// Author: quyetkaito (07/04/2022)
        public int Insert(MISAEntity entity)
        {
            // Lấy tên cột từ table
            var columns = GetTableColumns();
            var sp = new StringBuilder();
            sp.Append($"INSERT INTO {tableName}(");
            for (var i = 0; i < columns.Count(); i++)
            {
                if (i > 0) { sp.Append(","); }
                sp.Append($"{columns[i]}");
            }
            sp.Append(") VALUES(");
            for (var i = 0; i < columns.Count(); i++)
            {
                if (i > 0) { sp.Append(","); }
                sp.Append($"@{columns[i]}");
            }
            sp.Append(")");

            var sqlConnection = new MySqlConnection(ConnectionString);
            // Thêm dữ liệu từ DB:
            //Câu lệnh truy vấn dữ liệu:
            var sqlCommand = sp.ToString();
            var res = sqlConnection.Execute(sql: sqlCommand, param: entity);
            return res;

        }





        /// <summary>
        /// Sửa thông tin một đối tượng 
        /// </summary>
        /// <param name="entity">Một đối tượng cần sửa</param>
        /// <returns>số bản ghi được cập nhật</returns>
        /// Author: quyetnv (12/03/2022)
        public int Update(MISAEntity entity)
        {
            // Lấy key từ bảng
            var key = GetTableKey();
            // Lấy danh sách các cột
            var columns = GetTableColumns();
            //biến lưu câu lệnh SQL
            var sp = new StringBuilder();
            sp.Append($"UPDATE {tableName} SET");
            var hasColumn = false;
            for (var i = 0; i < columns.Count(); i++)
            {
                var column = columns[i];
                if (column == key)
                {
                    continue; //bỏ qua column là khóa chính
                }

                if (hasColumn) { sp.Append(","); }
                sp.Append($" {column}=@{column}");
                hasColumn = true;
            }
            sp.Append($" WHERE {key}= @{key}");
            var sqlConnection = new MySqlConnection(ConnectionString);

            // build câu lệnh truy vấn dữ liệu:
            var sqlCommand = sp.ToString();
            var res = sqlConnection.Execute(sql: sqlCommand, param: entity);
            return res;
        }


        /// <summary>
        /// Phân trang, tìm kiếm 
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên một trang</param>
        /// <param name="pageNumber">Trang số bao nhiêu</param>
        /// <param name="textSearch">Thông tin tìm kiếm</param>
        /// <returns>Một đối tượng bao gồm tổng số trang, tổng số bản ghi, dữ liệu đối tượng</returns>
        /// Author: quyetnv (12/03/2022)
        public virtual object Filter(int pageSize, int pageNumber, string? textSearch)
        {
            //lấy các cột của bảng, EmployeeId = "Employee"+"Id", DepartmentId = "Department"+"Id
            var columnName = $"{tableName}Name";
            var columnCode = $"{tableName}Code";

            //Khởi tạo kết nối với MariaDB
            using (var sqlConnection = new MySqlConnection(ConnectionString))
            {
                if (string.IsNullOrEmpty(textSearch))
                {
                    textSearch = "";
                }
                //tạo các parameter gán dữ liệu từ client truyền vào
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@dataFilter", "%" + textSearch + "%");
                parameters.Add("@Offset", (pageNumber - 1) * pageSize);
                parameters.Add("@Limit", pageSize);

                //1.câu lệnh truy vấn số bản ghi phù hợp với employeeFilter
                var sqlFilter = @$"SELECT COUNT(1) FROM {tableName} WHERE {columnCode} LIKE @dataFilter OR {columnName} LIKE @dataFilter; 
                                SELECT * FROM {tableName}  WHERE {columnCode} LIKE @dataFilter OR {columnName} LIKE @dataFilter ORDER BY ModifiedDate DESC LIMIT @Offset,@Limit";

                var multi = sqlConnection.QueryMultiple(sqlFilter, param: parameters);
                var totalRecord = multi.Read<int>().Single();
                var entities = multi.Read<MISAEntity>().ToList();

                double totalPage;
                if (totalRecord < pageSize)
                {
                    totalPage = 1;
                }
                else
                {
                    totalPage = (double)totalRecord / pageSize; //tổng số lượng các trang
                    if (totalPage != Math.Floor(totalPage)) //nếu chia có dư => số lượng page + 1
                    {
                        totalPage = Math.Floor(totalPage) + 1;
                    }
                }

                //build Data gửi về cho client
                var data = new
                {
                    Data = entities,
                    TotalRecord = totalRecord,
                    TotalPage = totalPage
                };
                //trả kết quả cho client
                return data;
            }

        }

        public virtual Object Paging(int pageSize, int pageNumber)
        {
            using var sqlConnection = new MySqlConnection(ConnectionString);

            //tạo các parameter gán dữ liệu từ client truyền vào
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Offset", (pageNumber - 1) * pageSize);
            parameters.Add("@Limit", pageSize);

            //1.câu lệnh truy vấn số bản ghi phù hợp với employeeFilter
            var sqlFilter = @$"SELECT COUNT(1) FROM {tableName}; 
                                SELECT * FROM {tableName} ORDER BY modified_date DESC LIMIT @Limit OFFSET @Offset";

            var multi = sqlConnection.QueryMultiple(sqlFilter, param: parameters);
            var totalRecord = multi.Read<int>().Single();
            var entities = multi.Read<MISAEntity>().ToList();

            double totalPage;
            if (totalRecord < pageSize)
            {
                totalPage = 1;
            }
            else
            {
                totalPage = (double)totalRecord / pageSize; //tổng số lượng các trang
                if (totalPage != Math.Floor(totalPage)) //nếu chia có dư => số lượng page + 1
                {
                    totalPage = Math.Floor(totalPage) + 1;
                }
            }

            //build Data gửi về cho client
            var data = new
            {
                Data = entities,
                TotalRecord = totalRecord,
                TotalPage = totalPage
            };
            //trả kết quả cho client
            return data;

        }

        /// <summary>
        /// Thực hiện xóa nhiều đối tượng cùng lúc
        /// </summary>
        /// <param name="entityIds">danh sách các id của đối tượng</param>
        /// <returns>số bản ghi được xóa</returns>
        /// Author: quyetnv (14/03/2022)
        public int MultiDelete(Guid[] entityIds)
        {
            // Khởi tạo kết nối MariaDb
            using (var sqlConnection = new MySqlConnection(ConnectionString))
            {
                var columnId = GetTableKey();
                // Câu lệnh truy vấn
                var sqlCommand = $"DELETE FROM {tableName} WHERE {columnId} IN (";
                DynamicParameters parameters = new DynamicParameters();
                // Xử lý chuỗi id client gửi lên
                for (int i = 0; i < entityIds.Length; i++)
                {
                    if (i != entityIds.Length - 1)
                    {
                        sqlCommand += $"@param{i}, ";
                        parameters.Add($"@param{i}", entityIds[i]);
                    }
                    else
                    {
                        sqlCommand += $"@param{i}";
                        parameters.Add($"@param{i}", entityIds[i]);
                    }
                }
                sqlCommand += ")";
                //Thực hiện truy vấn
                var res = sqlConnection.Execute(sql: sqlCommand, param: parameters);
                return res;
            }


        }

        /// <summary>
        /// Hàm lấy khóa chính của bảng.
        /// </summary>
        /// <returns></returns>
        public string GetTableKey()
        {
            var prs = typeof(MISAEntity).GetProperties();
            foreach (var p in prs)
            {
                var att = p.GetCustomAttribute<TableKey>();
                if (att != null)
                {
                    return p.Name;
                }
            }
            return null;
        }

        /// <summary>
        /// Hàm lấy danh sách các cột của bảng.
        /// </summary>
        /// <returns></returns>
        public List<string> GetTableColumns()
        {
            var columns = new List<string>();
            var prs = typeof(MISAEntity).GetProperties();
            foreach (var p in prs)
            {
                var att = p.GetCustomAttribute<TableColumn>();
                if (att != null)
                {
                    columns.Add(p.Name);
                }
            }
            return columns;
        }
        #endregion

        public string ToUnderscoreCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
