using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Runtime;
using Test.Data;
using Test.Model;

namespace Test.Services
{
    public class Services : IService
    {
        private readonly ApplicationDbContext _context;
        public Services(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddCategory(Category category)
        {
           var exiting = _context.Category.FirstOrDefault(c => c.Id == category.Id);
            if (exiting != null)
            {
                // update
                exiting.Id = category.Id;
                exiting.Name = category.Name;
                _context.Category.Update(exiting);
            }
            else
            {
               _context.Category.Add(category);
            }
            await _context.SaveChangesAsync();
            return "added";
        }


        public async Task<string> AddCustomer(Customer customer)
        {
            var exiting = _context.Customers.FirstOrDefault(c => c.Id == customer.Id);
            if (exiting != null)
            {
                // update
                _context.Customers.Update(exiting);
            }
            else
            {

                _context.Customers.Add(customer);
            }
            await _context.SaveChangesAsync();
            return "added";
        }

        
        public async Task<string> AddOrder(Order order)
        {
            var exiting = _context.Orders.FirstOrDefault(c => c.Id == order.Id);
            if (exiting != null)
            {
                // update
                _context.Orders.Update(exiting);
            }
            else
            {

              _context.Orders.Add(order);
            }

            await _context.SaveChangesAsync();
            return "added";
        }

        public async Task<string> AddProduct(Product product)
        {
            var exiting = _context.products.FirstOrDefault(c => c.Id == product.Id);
            if (exiting != null)
            {
                // update
                _context.products.Update(product);
            }
            else
            {

                _context.products.Add(product);
            }
         
            await _context.SaveChangesAsync();
            return "added";
        }




        public async Task<string> GetProductWithMaxOrder()
        {
                var result = await _context.Database
         .SqlQuery<ProductCountDto>(
             @$"SELECT TOP 1 p.Name, COUNT(o.Id) AS OrderCount
               FROM Products p
               LEFT JOIN Orders o ON o.ProducrId = p.Id
               GROUP BY p.Name
               ORDER BY COUNT(o.Id) DESC")
         .AsNoTracking()
         .FirstOrDefaultAsync();



            //Console.Write($"{result.Name} {result.OrderCount}"); // Returns null if no data
            if( result == null )
            {
                return "date is null";
            }
            var ans = result.Name + " " + result.OrderCount;
            return ans;
        }



        //public Task<Product> getProduct()
        //{
        //    return _context.products.OrderByDescending(p => p.Id);
        //}
        //public Task<Product> getCustomer()
        //{
        //    return _context.products.OrderByDescending(p => p.Id);
        //}
        //public Task<Product> getCategory()
        //{
        //    return _context.products.OrderByDescending(p => p.Id);
        //}
    }
}
