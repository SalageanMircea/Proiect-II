namespace Restaurant_Management.Models
{
    public class ShoppingItemInput
    {
        public string Name { get; set; } = "";

        public string QuantityText { get; set; } = "";

        public string Unit { get; set; } = "";

        public bool IsPurchased { get; set; }
    }
}