using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media;
using Restaurant_Management.Models;
using Restaurant_Management.Services.Interfaces;
using System.Collections.ObjectModel;
using Windows.UI;

namespace Restaurant_Management.ViewModels
{
    public class KitchenChefViewModel
    {
        private readonly IOrderService _orderService;
        private List<Order> _allOrders = new List<Order>();

        public ObservableCollection<OrderDisplayItem> VisibleOrders { get; }
            = new ObservableCollection<OrderDisplayItem>();

        public string ChefName { get; private set; }

        public KitchenChefViewModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public void Initialize(string name)
        {
            ChefName = name;
            LoadOrders();
        }

        public void LoadOrders()
        {
            _allOrders = _orderService.GetAllOrders();
            ApplyFilter("");
        }

        public void ApplyFilter(string status)
        {
            var filtered = _orderService.GetOrdersByStatus(status);

            VisibleOrders.Clear();

            foreach (var order in filtered)
            {
                VisibleOrders.Add(new OrderDisplayItem
                {
                    OrderId = order.OrderId,
                    TableNumber = order.TableNumber,
                    WaiterName = order.WaiterName,
                    Details = order.Details,
                    Status = order.Status,
                    SentAt = order.SentAt.ToString("dd/MM/yyyy HH:mm"),
                    StatusColor = GetStatusColor(order.Status)
                });
            }
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            _orderService.UpdateOrderStatus(orderId, newStatus);
            LoadOrders();
        }

        private SolidColorBrush GetStatusColor(string status)
        {
            return status switch
            {
                "Received" => new SolidColorBrush(Color.FromArgb(255, 59, 130, 246)),
                "Preparing" => new SolidColorBrush(Color.FromArgb(255, 234, 179, 8)),
                "Done" => new SolidColorBrush(Color.FromArgb(255, 34, 197, 94)),
                _ => new SolidColorBrush(Color.FromArgb(255, 107, 114, 128))
            };
        }
    }

    public class OrderDisplayItem
    {
        public int OrderId { get; set; }
        public int TableNumber { get; set; }
        public string WaiterName { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public string SentAt { get; set; }
        public SolidColorBrush StatusColor { get; set; }
    }
}
    

