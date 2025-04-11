using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroApi_DotNet8.Data;
using SuperHeroApi_DotNet8.Entities;

namespace SuperHeroApi_DotNet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        public readonly DataContext _context;
        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<SuperHero>> GetAllHero()
        {
            var hero = await _context.SuperHeroes.ToListAsync();
            return Ok(hero);    
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<SuperHero>>> GetAllHero(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if(hero is null)
            {
                return BadRequest("Bad Request!!!");
            }
            return Ok(hero);
        }
        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
           _context.SuperHeroes.Add(hero);
           await _context.SaveChangesAsync();   
           return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero UpdatedHero)
        {
            var DBhero = await _context.SuperHeroes.FindAsync(UpdatedHero.Id);
            if (DBhero is null)
            {
                return BadRequest("Bad Request!!!");
            }
            DBhero.Name = UpdatedHero.Name;
            DBhero.FirstName = UpdatedHero.FirstName;
            DBhero.LastName = UpdatedHero.LastName;
            DBhero.Place = UpdatedHero.Place; 
            
            await _context.SaveChangesAsync();
            return Ok(await _context.SuperHeroes.ToListAsync());
        }
        [HttpDelete]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero is null)
            {
                return BadRequest("Bad Request!!!");
            }
            _context.SuperHeroes.Remove(hero);
            await _context.SaveChangesAsync();
            return Ok(await _context.SuperHeroes.ToListAsync());
        }
    }
}
