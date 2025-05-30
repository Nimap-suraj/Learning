using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task.Model;
using Task.Services.Interfaces;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly IDepartmentServices _services;
        public DepartmentController(IDepartmentServices services)
        {
            _services = services;
        }

        [HttpPost("AddingDepartment")]
        public async Task<IActionResult> AddDepartment(Department department)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Something gone wrong!!");
                if (department.Name == "string")
                    return BadRequest("DepartName should not be String");

                var result = await _services.CreateDepartment(department);
                return Ok("Department added successfully");
            }
            catch (Exception)
            {

                return StatusCode(500, "Something gone wrong!! while adding");
            }

        }
        [HttpPut("UpdateDepartment")]
        public async Task<IActionResult> Update(int id, Department department)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Something gone wrong!!");
                if (department.Name == "string")
                    return BadRequest("DepartName should not be String");
                var result = await _services.UpdateDepartment(id, department);
                return Ok("Updated successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something gone wrong!! while adding");
            }
        }

        [HttpGet("GetAllDepartment")]
        public async Task<IActionResult> getALL()
        {
            var results = await _services.GetALLDepartment();
            return Ok(results);
        }

        [HttpDelete("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var results = await _services.DeleteDepartment(id);
            if (!results)
                return NotFound("Deapartmernt not found");
            return Ok("Department Deleted Successfully");
        }
    }
}
