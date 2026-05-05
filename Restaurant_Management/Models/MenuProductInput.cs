namespace Restaurant_Management.Models
{
    public class MenuProductInput
    {
        public string Name { get; set; } = "";

        public string Category { get; set; } = "";

        public string PriceText { get; set; } = "";

        public bool IsAvailable { get; set; }
    }
}