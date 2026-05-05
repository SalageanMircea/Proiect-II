namespace Restaurant_Management.Models
{
    public class MenuProduct
    {
        public int MenuId { get; set; }

        public string Name { get; set; } = "";

        public string Category { get; set; } = "";

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }
    }
}