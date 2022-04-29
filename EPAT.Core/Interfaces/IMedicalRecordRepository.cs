using EPAT.Core.Entities;
using EPAT.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Interfaces
{
    public interface IMedicalRecordRepository:IBaseRepository<MedicalRecord>
    {
        public IEnumerable<MedicalRecord> GetByPatient(Guid id);
    }
}
