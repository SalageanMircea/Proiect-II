using System;

namespace Restaurant_Management.Models
{
    public class ShoppingItem
    {
        public int ShoppingItemId { get; set; }

        public string Name { get; set; } = "";

        public decimal Quantity { get; set; }

        public string Unit { get; set; } = "";

        public bool IsPurchased { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}