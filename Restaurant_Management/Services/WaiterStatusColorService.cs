using Microsoft.UI.Xaml.Media;
using Restaurant_Management.Models;
using Windows.UI;

namespace Restaurant_Management.Services
{
    public class WaiterStatusColorService : IWaiterStatusColorService
    {
        public SolidColorBrush GetStatusColor(string status)
        {
            if (status == RestaurantOrderStatus.Received)
            {
                return new SolidColorBrush(Color.FromArgb(255, 59, 130, 246));
            }

            if (status == RestaurantOrderStatus.Preparing)
            {
                return new SolidColorBrush(Color.FromArgb(255, 234, 179, 8));
            }

            if (status == RestaurantOrderStatus.Done)
            {
                return new SolidColorBrush(Color.FromArgb(255, 34, 197, 94));
            }

            return new SolidColorBrush(Color.FromArgb(255, 107, 114, 128));
        }
    }
}