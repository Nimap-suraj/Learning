using Test.Model;

namespace Test.Services
{
    public interface IService
    {
        public Task<string> AddCustomer(Customer customer);
        public Task<string> AddProduct(Product product);
        public Task<string> AddCategory(Category category);
        public Task<string> AddOrder(Order order);


        Task<string> GetProductWithMaxOrder();
    }
}
