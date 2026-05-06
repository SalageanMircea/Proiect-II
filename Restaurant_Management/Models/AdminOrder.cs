using System;

namespace Restaurant_Management.Models
{
    public class AdminOrder
    {
        public int OrderId { get; set; }

        public int TableNumber { get; set; }

        public string WaiterName { get; set; } = "";

        public string Details { get; set; } = "";

        public string Status { get; set; } = "";

        public DateTime SentAt { get; set; }
    }
}