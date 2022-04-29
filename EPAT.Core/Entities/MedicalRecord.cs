using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Entities
{
    /// <summary>
    /// Thông tin bệnh án
    /// </summary>
    public class MedicalRecord:BaseEntity
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        [TableColumn] [TableKey] public Guid medical_record_id { get; set; }

        /// <summary>
        /// Id của bệnh nhân
        /// </summary>
        [TableColumn] public Guid patient_id { get; set; }

        /// <summary>
        /// ngày nhập viện
        /// </summary>
        [TableColumn] public DateTime hospitalized_day { get; set; }

        /// <summary>
        /// ngày ra viện
        /// </summary>
        [TableColumn] public DateTime discharged_day { get; set; }

        /// <summary>
        /// chẩn đoán/ lý do vào viện(nhập freetext)
        /// </summary>
        [TableColumn] public string diagnose { get; set; }

        /// <summary>
        /// triệu chứng(nhập freetext)
        /// </summary>
        [TableColumn] public string symptom { get; set; }

        /// <summary>
        /// lịch sử đo trạng thái(nhiệt độ, huyết áp, nhịp tim, SPO2, ngày đo) => lưu JSON
        /// </summary>
        [TableColumn] public string status { get; set; }

        /// <summary>
        /// tiền sử bệnh(freetext)
        /// </summary>
        [TableColumn] public string diseases { get; set; }


        /// <summary>
        /// lịch sử điều trị(ngày giờ, diễn biến)=> Lưu JSON
        /// </summary>
        [TableColumn] public string treatment { get; set; }


    }
}
