using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASPWEBAPI.Controllers
{
    [Route("api/[controller]")] // attribute routing
    [ApiController]
    // locahoclhost port no api/FruitsApiController
    //agar get hoga toh
    public class FruitsApiController : ControllerBase
    {
        public List<String> fruits = new List<String>()
        {
            "banana",
            "apple",
            "grapes",
            "mangoes",
            "chiku"
        }; 

        [HttpGet]
        public List<string> GetFruits()
        {
            return fruits.Where(x=>x.Length>=2).ToList();  
        }
        // local potr api/
        [HttpGet("{id}")]
        public string GetFruitsByIndex(int id)
        {
            //return fruits[id];
            return fruits[id];
        }

    }
}
