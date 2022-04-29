using EPAT.Core.Entities;
using EPAT.Core.Interfaces;
using EPAT.Core.Interfaces.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPAT.WebApi.Controllers
{
    /// <summary>
    /// API cho bệnh nhân
    /// </summary>
    public class PatientsController : EPatBaseController<Patient>
    {
        IPatientService _patientService;
        public PatientsController(IPatientService patientService) : base(patientService)
        {
            _patientService = patientService;
        }
    }
}
