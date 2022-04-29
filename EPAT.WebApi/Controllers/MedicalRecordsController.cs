using EPAT.Core.Entities;
using EPAT.Core.Interfaces;
using EPAT.Core.Interfaces.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPAT.WebApi.Controllers
{
    /// <summary>
    /// API cho bệnh án
    /// </summary>
    /// Author: quyetkaito (29/04/2022)
    public class MedicalRecordsController : EPatBaseController<MedicalRecord>
    {
        IMedicalRecordService _service;
        public MedicalRecordsController(IMedicalRecordService service) : base(service)
        {
            _service = service;
        }

        #region API riêng
        /// <summary>
        /// API lấy bệnh án theo mã bệnh nhân
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: quyetkaito (29/04/2022)
        [HttpGet("patient/{id}")]
        public IActionResult GetByPatient(Guid id)
        {
            var res = _service.GetByPatient(id);
            return Ok(res);
        }
        #endregion
    }
}
