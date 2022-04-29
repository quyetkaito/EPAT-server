using Dapper;
using EPAT.Core.Entities;
using EPAT.Core.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Infrasctructure.Repository
{
    public class PatientRepositoty:BaseRepository<Patient>,IPatientRepository
    {
        /// <summary>
        /// Phân trang, tìm kiếm 
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên một trang</param>
        /// <param name="pageNumber">Trang số bao nhiêu</param>
        /// <param name="textSearch">Thông tin tìm kiếm</param>
        /// <returns>Một đối tượng bao gồm tổng số trang, tổng số bản ghi, dữ liệu đối tượng</returns>
        /// Author: quyetnv (12/03/2022)
        public override object Filter(int pageSize, int pageNumber, string? textSearch)
        {
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

                //1.câu lệnh truy vấn số bản ghi phù hợp
                var sqlFilter = @$"SELECT COUNT(1) FROM patient WHERE fullname LIKE @dataFilter OR identity_number LIKE @dataFilter OR phone_number LIKE @dataFilter; 
                                SELECT * FROM patient WHERE fullname LIKE @dataFilter OR identity_number LIKE @dataFilter OR phone_number LIKE @dataFilter ORDER BY modified_date DESC LIMIT @Offset,@Limit";

                var multi = sqlConnection.QueryMultiple(sqlFilter, param: parameters);
                var totalRecord = multi.Read<int>().Single();
                var entities = multi.Read<Patient>().ToList();

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
    }
}
