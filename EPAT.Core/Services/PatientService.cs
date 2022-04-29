using EPAT.Core.Entities;
using EPAT.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Services
{
    public class PatientService : BaseService<Patient>
    {
        public PatientService(IBaseRepository<Patient> baseRepository) : base(baseRepository)
        {
        }
    }
}
