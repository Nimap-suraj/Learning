using Hospital_OPD.Model;
using Hospital_OPD.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_OPD.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientServices _services;
        public PatientController(IPatientServices services)
        {
            _services = services;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> AddPatient(Patient patient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                if (patient.Name?.ToLower() == "string" || patient.Email?.ToLower() == "string")
                {
                    return BadRequest("Please provide valid values for Name and Email.");
                }

                var result = await _services.AddPatient(patient);
                return Ok("Patient added successfully!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong while adding the patient.");
            }
        }
        [HttpGet("Search")]
        public async Task<IActionResult> SearchPatient(string query)
        {
            var results = await _services.SearchPatient(query);
            return Ok(results);
        }
        [HttpGet("GetAllPatients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var results = await _services.GetALLPatient();
            return Ok(results);
        }
    }
}
