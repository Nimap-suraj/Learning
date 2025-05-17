using Hospital_OPD.Data;
using Hospital_OPD.Model;
using Hospital_OPD.Services.Interface;
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
        [HttpPost("AddDoctor")] 
        public async Task<IActionResult> Add(Doctor doctor)
        {
            var Data = await _services.AddDoctor(doctor);
            return Ok(Data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _services.GetDoctor(id);
            return data == null ? NotFound("Data Found") : Ok(data);
        }
        [HttpGet("GetALLDoctor")]
        public async Task<IActionResult> GetALL()
        {
            var data = await _services.GetAllDoctor();
            return Ok(data);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Doctor doctor)
        {
            var success = await _services.UpdateDoctor(id, doctor);
            return success ? Ok("Updated") : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _services.DeleteDoctor(id);
            return success ? Ok("Deleted") : NotFound();
        }
    }
}
