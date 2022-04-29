using EPAT.Core.Entities;
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
        public PatientsController(IBaseService<Patient> baseService) : base(baseService)
        {
        }
    }
}
