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
    public class PatientService : BaseService<Patient>,IPatientService
    {
        IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository) : base(patientRepository)
        {
            _patientRepository = patientRepository; 
        }
    }
}
