using Microsoft.EntityFrameworkCore;

namespace api_EXAM_01.Model
{
    //[Keyless]
    public class ProductWithMaxOrder
    {
        public string Name { get; set; }
        public int OrderCount { get; set; }
    
    }
}
