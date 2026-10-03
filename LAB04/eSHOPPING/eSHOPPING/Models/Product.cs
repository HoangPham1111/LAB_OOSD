namespace eSHOPPING.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Manufacturer { get; set; }
        public int ProductGroupId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public string ImageUrl { get; set; }
    }
}