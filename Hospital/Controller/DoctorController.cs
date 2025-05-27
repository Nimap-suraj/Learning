using Hospital_OPD.Data;
using Hospital_OPD.Model;
using Hospital_OPD.Services.Implementation;
using Hospital_OPD.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Hospital_OPD.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorServices _services;
        public DoctorController(IDoctorServices services)
        {
            _services = services;
        }
        [Authorize(Roles = "doctor,receptionist")]
        [HttpGet("GetDoctorScheduleWithNames")]
        public async Task<IActionResult> GetDoctorScheduleWithNames([FromQuery] int doctorId, [FromQuery] DateTime date)
        {
            var result = await _services.GetDoctorScheduleWithNames(doctorId, date);
            return Ok(result);
        }
        [Authorize(Roles = "receptionist")]
        [HttpPost("AddDoctor")] 
        public async Task<IActionResult> Add(Doctor doctor)
        {
            var Data = await _services.AddDoctor(doctor);
            return Ok(Data);
        }
        [Authorize(Roles = "receptionist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _services.GetDoctor(id);
            return data == null ? NotFound("Data Found") : Ok(data);
        }
        [Authorize(Roles = "receptionist,doctor")]
        [HttpGet("GetALLDoctor")]
        public async Task<IActionResult> GetALL()
        {
            var data = await _services.GetAllDoctor();
            return Ok(data);
        }
        [Authorize(Roles = "receptionist,doctor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Doctor doctor)
        {
            var success = await _services.UpdateDoctor(id, doctor);
            return  Ok("Updated");
        }
        [Authorize(Roles = "receptionist,doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _services.DeleteDoctor(id);
            return success ? Ok("Deleted") : NotFound();
        }

        [Authorize(Roles = "receptionist,doctor")]
        [HttpPut("MarkLeave/{doctorId}")]
        public async Task<IActionResult> MarkDoctorOnLeave(int doctorId, [FromQuery] bool IsOnLeave)
        {
            try
            {
                var result = await _services.MarkDoctorLeave(doctorId, IsOnLeave);
                return Ok(result);  
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Something went wrong: {ex.Message}");

            }
        }
    }
}
