using System;

namespace Restaurant_Management.Services
{
    public interface INavigationService
    {
        bool NavigateTo(Type pageType);

        bool NavigateTo(Type pageType, object parameter);
    }
}