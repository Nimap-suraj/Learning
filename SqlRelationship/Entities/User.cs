using System.Security.Principal;

namespace SqlRelationship.Entities
{
    public class User
    {
        public int Id { get; set; }
        public  string Name { get; set; }
        // One to One Relationship
        public  Address Address { get; set; }
        public int AddressId {  get; set; }


        // one to Many Relationship
        public  List<Product> Products { get; set; }

        // Many to Many Relationship
        public  List<Coupon> Coupons { get; set; }

    
    }
}
