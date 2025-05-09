using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ItemsController : Controller
    {
        private readonly MyAppContext _context;
        public ItemsController(MyAppContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _context.Items.ToListAsync();
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id","Name","Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                   _context.Items.Add(item);
                   await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
            }
            return View(item);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,[Bind("Id", "Name", "Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirfed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
                return RedirectToAction("Index");
        }


    }
}
