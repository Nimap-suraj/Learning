using Hospital_OPD.Model;
using Hospital_OPD.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_OPD.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalHistoryController : ControllerBase
    {
        private readonly IMedicalRecord _service;
        public MedicalHistoryController(IMedicalRecord service)
        {
            _service = service;

        }
        [HttpPost("AddMedicalRecord")]
        public async Task<IActionResult> CreateMedicalRecord(MedicalRecord history)
        {
            var result = await _service.AddMedicalHistory(history);
            return Ok(new { status = true, message = result });
          
        }
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetHistory(int patientId)
        {
            var result = await _service.GetMedicalHistory(patientId);
            return Ok(new { status = true, data = result });
        }
    }
}
