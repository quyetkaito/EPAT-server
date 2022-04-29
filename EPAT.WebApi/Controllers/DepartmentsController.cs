using EPAT.Core.Entities;
using EPAT.Core.Interfaces.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPAT.WebApi.Controllers
{
    /// <summary>
    /// API cho vị trí làm việc
    /// </summary>
    /// Author: quyetkaito (29/04/2022)
    public class DepartmentsController : EPatBaseController<Department>
    {
        public DepartmentsController(IBaseService<Department> baseService) : base(baseService)
        {
        }
    }
}
