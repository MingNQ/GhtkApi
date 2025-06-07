namespace Ghtk.Repository.Entities
{
    public class Product
    {
        public string Name { get; set; } = default!;
        
        public double Weight { get; set; }

        public long Quantity { get; set; }

        public long ProductCode { get; set; }
    }
}
