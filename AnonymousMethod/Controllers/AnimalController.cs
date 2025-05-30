using AnonymousMethod.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnonymousMethod.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        public List<AnimalModel> animals = null;
        public AnimalController()
        {
            animals = new List<AnimalModel>()
            {
                new AnimalModel() { Id = 1, Name = "lion" },
                new AnimalModel() { Id = 2, Name = "tiger" },
                new AnimalModel() { Id = 3, Name = "Cheetah" },
            }
            ;
        }
        [Route("~/api/[action]",Name = "ALL")]
        public IActionResult GetAnimal()
        {
            return Ok(animals);
        }
        [Route("~/test")]
        public IActionResult GetAnimalTest()
        {
            return AcceptedAtRoute("ALL");
        }

        //[Route("{name}")]
        //public IActionResult GetanimalNAme(string name)
        //{
        //    if (!name.Contains("ABC"))
        //    {
        //        return BadRequest();    
        //    }
        //    return Ok(animals);
        //}
        [Route("{id:int}")]
        public IActionResult GetAnimalByID(int id)
        {
            if(id == 0)
            {
                return Ok("id is Zero");
            }
            var animal = animals.FirstOrDefault(x => x.Id == id);
            if(animal == null)
            {
                return NotFound();
            }
            return Ok(animal);
        }
        [HttpPost("")]
        public IActionResult AddAnimal(AnimalModel animal)
        {   
            animals.Add(animal); 
            return CreatedAtAction("GetAnimalByID", new {id = animal.Id},animal);
        }
        [Route("test1")]
        public IActionResult GetData()
        {
            return LocalRedirectPermanent("~/api/GetAnimal");
        }
        //LocalRedirect 302
        //LocalRedirectPermanent 301

    }


}
   
