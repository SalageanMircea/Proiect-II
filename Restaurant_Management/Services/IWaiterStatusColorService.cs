using Microsoft.UI.Xaml.Media;

namespace Restaurant_Management.Services
{
    public interface IWaiterStatusColorService
    {
        SolidColorBrush GetStatusColor(string status);
    }
}