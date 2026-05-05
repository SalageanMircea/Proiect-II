using Microsoft.UI.Xaml.Media;
using Restaurant_Management.Models;

namespace Restaurant_Management.ViewModels
{
    public class ChefOrderItem
    {
        public int OrderId { get; set; }

        public int TableNumber { get; set; }

        public string WaiterName { get; set; }

        public string Details { get; set; }

        public string Status { get; set; }

        public string SentAt { get; set; }

        public SolidColorBrush StatusColor { get; set; }

        public ChefOrderItem(ChefOrder order, SolidColorBrush statusColor)
        {
            OrderId = order.OrderId;
            TableNumber = order.TableNumber;
            WaiterName = order.WaiterName;
            Details = order.Details;
            Status = order.Status;
            SentAt = order.SentAt.ToString("dd/MM/yyyy HH:mm");
            StatusColor = statusColor;
        }
    }
}