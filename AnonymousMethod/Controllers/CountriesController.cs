using ConsoleToWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleToWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        //[BindProperty(SupportsGet =true)]
        //public CountryModel Country { get; set; }   
        //[HttpPost("")]
        //public IActionResult GetCountry()
        //{
        //    return Ok($"{this.Country.Name} {this.Country.Address} {this.Country.City}");
        //}
        //[HttpGet("")]
        //public IActionResult GetCountry(string name,int area)
        //{
        //    return Ok($"name: {name} and area is {area}");
        //}
        //[HttpGet("{name}/{area}")]
        //public IActionResult GetCountryData(string name, int area)
        //{
        //    return Ok($"name: {name} and area is {area}");
        //}
        // FromQuery
        //[HttpPost("")]
        //public IActionResult GetCountryData([FromQuery]CountryModel country)
        //{
        //    return Ok($"name: {country.Name}");
        //}
        //FromRoute
        //[HttpPost("{name}/{address}/{city}")]
        //public IActionResult GetCountryData([FromRoute]CountryModel country ,[FromQuery]int id)
        //{
        //    return Ok($"country: {country.Name} {country.Address} {country.City}");
        //
        
        [HttpPost("{id:int}")]
        public IActionResult GetData([FromBody]int id)
        {
            return Ok($"id is {id}");
        }
    }
}

