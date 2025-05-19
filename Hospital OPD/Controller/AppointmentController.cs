using Hospital_OPD.Model;
using Hospital_OPD.Services.Implementation;
using Hospital_OPD.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_OPD.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentServices _services;
        public AppointmentController(IAppointmentServices services)
        {
            _services = services;
        }
        [HttpPost("BookAppointment")]
        public async Task<IActionResult> BookAppointment([FromBody]Appointment appointment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Something Appointment date");
               
                var result = await _services.BookAppointment(appointment);
                // Check if result is a failure message
                //if (result.Contains("not") || result.Contains("leave") || result.Contains("booked") || result.Contains("try"))
                //{
                //    return BadRequest(result);
                //}

                return Ok(result); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong: {ex.Message}");
            }
        }
        [HttpDelete("CancelAppointment")]
        public async Task<IActionResult> CancelAppointment([FromQuery] int patientId, [FromQuery] int doctorId)
        {
            try
            {
                var result = await _services.CancelAppointment(patientId, doctorId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong: {ex.Message}");
            }
        }

    }
}
