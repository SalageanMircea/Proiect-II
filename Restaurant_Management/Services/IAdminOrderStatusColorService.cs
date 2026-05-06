using Microsoft.UI.Xaml.Media;

namespace Restaurant_Management.Services
{
    public interface IAdminOrderStatusColorService
    {
        SolidColorBrush GetColor(string status);
    }
}