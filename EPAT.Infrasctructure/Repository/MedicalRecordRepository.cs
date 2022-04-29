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
    public class MedicalRecordRepository:BaseRepository<MedicalRecord>,IMedicalRecordRepository
    {
        /// <summary>
        /// truy cập db lấy thông tin bệnh án theo mã bệnh nhân
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<MedicalRecord> GetByPatient(Guid id)
        {
            //Khởi tạo kết nối với MariaDB
            using (var sqlConnection = new MySqlConnection(ConnectionString))
            {
                //truyền para
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);

                //Câu lệnh truy vấn lấy dữ liệu
                var sqlCommand = $"SELECT * FROM medical_record WHERE patient_id = @id";
                var res = sqlConnection.Query<MedicalRecord>(sql: sqlCommand, param: parameters);

                //trả kết quả cho client
                return res;
            }
        }
    }
}
