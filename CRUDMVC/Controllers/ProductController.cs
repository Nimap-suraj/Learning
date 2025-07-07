using CRUDMVC.Data;
using CRUDMVC.Filters;
using CRUDMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDMVC.Controllers
{
    [LogActionFilter]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductController(ApplicationDbContext context,IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        
        // product ka list is showing......
        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p=>p.Id).ToList();
            return View(products);
        }
        // create page is showing.
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult Create(ProductDto dto)
        {
            if (dto.ImageFile == null) {
                ModelState.AddModelError("ImageFile", "Image cannot be Null!");
            }
            if (!ModelState.IsValid) {
                return View(dto);
            }
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(dto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using(var stream = System.IO.File.Create(imageFullPath))
            {
                dto.ImageFile.CopyTo(stream);   
            }

            var product = new Product()
            {
                Name = dto.Name,
                Brand = dto.Brand,
                Category = dto.Category,
                Price = dto.Price,
                Description = dto.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };
            context.Products.Add(product);
            context.SaveChanges();
            TempData["Success"] = "Product created successfully!";
            return RedirectToAction("Index","Product");
        }





        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }
            var productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,
            };
            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

            return View(productDto);
        }
        [HttpPost]
         public IActionResult Edit(int id,ProductDto productDto)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index","Product");
            }
            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

                return View(productDto);
            }

            string newFileName = product.ImageFileName;
            if(productDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImageFile!.FileName);
                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);
                }
                // delete the old file path
                string OldImageFilePath = environment.WebRootPath + "/products/" + product.ImageFileName;
                System.IO.File.Delete(OldImageFilePath);

            }
            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Description = productDto.Description;
            product.Category = productDto.Category;
            product.Price = productDto.Price;
            product.ImageFileName = newFileName;
            context.SaveChanges();
            return RedirectToAction("Index", "Product");

        }
      
        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index","Product");
            }
            string ImageFilePath = environment.WebRootPath + "/products/" + product.ImageFileName;
            System.IO.File.Delete(ImageFilePath);

            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("Index","Product");
        }
    }
}
