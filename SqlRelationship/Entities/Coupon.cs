namespace SqlRelationship.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public  string Code { get; set; }

        public List<User>? Users { get; set; }
    }

}
