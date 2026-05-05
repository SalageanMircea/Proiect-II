using System;
using Microsoft.UI.Xaml.Controls;

namespace Restaurant_Management.Services
{
    public class FrameNavigationService : INavigationService
    {
        private readonly Frame frame;

        public FrameNavigationService(Frame frame)
        {
            this.frame = frame;
        }

        public bool NavigateTo(Type pageType)
        {
            if (pageType == null)
            {
                return false;
            }

            return frame.Navigate(pageType);
        }

        public bool NavigateTo(Type pageType, object parameter)
        {
            if (pageType == null)
            {
                return false;
            }

            return frame.Navigate(pageType, parameter);
        }
    }
}