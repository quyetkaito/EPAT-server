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
    public class AccountRepository:BaseRepository<Account>,IAccountRepository
    {
        /// <summary>
        /// tìm kiếm tài khoản theo tên
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public override object Filter(int pageSize, int pageNumber, string? textSearch)
        {
           //Khởi tạo kết nối với mysql
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

                //1.câu lệnh truy vấn số bản ghi phù hợp với, base là ko tìm theo cột nào cả.
                var sqlFilter = @$"SELECT COUNT(1) FROM account WHERE lower(account_name) LIKE lower(@dataFilter); 
                                SELECT * FROM account WHERE lower(account_name) LIKE lower(@dataFilter) ORDER BY modified_date DESC LIMIT @Offset,@Limit";

                var multi = sqlConnection.QueryMultiple(sqlFilter, param: parameters);
                var totalRecord = multi.Read<int>().Single();
                var entities = multi.Read<Account>().ToList();

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
