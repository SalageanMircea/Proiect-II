using System;

namespace Restaurant_Management.Models
{
    public class WaiterOrder
    {
        public int OrderId { get; set; }

        public int TableNumber { get; set; }

        public string Details { get; set; } = "";

        public string Status { get; set; } = RestaurantOrderStatus.Received;

        public DateTime SentAt { get; set; }
    }
}