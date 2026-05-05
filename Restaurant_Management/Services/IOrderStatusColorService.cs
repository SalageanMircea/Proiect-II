using Microsoft.UI.Xaml.Media;

namespace Restaurant_Management.Services
{
    public interface IOrderStatusColorService
    {
        SolidColorBrush GetStatusColor(string status);
    }
}