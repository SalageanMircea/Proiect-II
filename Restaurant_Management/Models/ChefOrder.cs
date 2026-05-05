using System;

namespace Restaurant_Management.Models
{
    public class ChefOrder
    {
        public int OrderId { get; set; }

        public int TableNumber { get; set; }

        public string WaiterName { get; set; } = "";

        public string Details { get; set; } = "";

        public string Status { get; set; } = ChefOrderStatus.Received;

        public DateTime SentAt { get; set; }
    }
}