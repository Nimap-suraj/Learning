using AnonymousMethod.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnonymousMethod.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    { // build kar 💖de
        public string GetEmployees()
        {
            return "All Employee";
        }
        [Route("~/api/[action]")]
        public EmployeeModel GetEmployeeData()
        {
            return new EmployeeModel()
            {
                Id = 1,
                Name = "suraj shah",
            };
        }
        [Route("~/api/[action]")]
        public IEnumerable<EmployeeModel> getList()
        {
            return new List<EmployeeModel>()
            {
                new EmployeeModel() { Id = 1, Name = "suraj shah" },
                new EmployeeModel() { Id = 2, Name = "rajat pandy" },
                new EmployeeModel() { Id = 3, Name = "Akshtas Dhumal" }
            };
        }
        // IActionResult
        [Route("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            return Ok(new List<EmployeeModel>()
            {
                new EmployeeModel() { Id = 1,Name ="surajshah" },
                new EmployeeModel() { Id = 2,Name ="omSambahr" },
                new EmployeeModel() { Id = 3,Name ="maheshMahanta" },
            });
        }
        // IActionResult<>
        [Route("{id}/basic")]
        public ActionResult<EmployeeModel> GetEmployeeBasicData(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            return new EmployeeModel() { Id = 1, Name = "surajshah" };
        }
    }
}
