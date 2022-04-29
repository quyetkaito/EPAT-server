using EPAT.Core.Entities;
using EPAT.Core.Interfaces;
using EPAT.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Services
{
    public class MedicalRecordService : BaseService<MedicalRecord>,IMedicalRecordService
    {
        IMedicalRecordRepository _repo;
        public MedicalRecordService(IMedicalRecordRepository repo) : base(repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Xử lý nghiệp vụ lấy bệnh án theo mã bệnh nhân
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<MedicalRecord> GetByPatient(Guid id)
        {
            return _repo.GetByPatient(id);
        }
    }
}
