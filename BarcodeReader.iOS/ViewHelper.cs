using System.Linq;
using UIKit;

namespace BarcodeReader
{
    public static class ViewHelper
    {
        public static UIViewController CurrentController
        {
            get
            {
                var windowsList = UIApplication.SharedApplication.Windows;
                var window = UIApplication.SharedApplication.KeyWindow;
                if (window.WindowLevel != 0)
                {
                    foreach (var windowItem in windowsList.Where(windowItem => windowItem.WindowLevel == 0))
                    {
                        window = windowItem;
                    }
                }

                var rootController = window.RootViewController;

                if (rootController == null || (rootController.PresentedViewController != null && rootController.PresentedViewController.GetType() == typeof(UIAlertController)))
                {
                    window = UIApplication.SharedApplication.Windows.OrderByDescending(w => w.WindowLevel)
                        .FirstOrDefault(w => w.RootViewController != null);

                    rootController = window.RootViewController;
                }

                while (rootController.PresentedViewController != null)
                {
                    if (rootController.PresentedViewController.GetType() == typeof(UIAlertController)) break;
                    rootController = rootController.PresentedViewController;
                }

                return rootController;
            }
        }
    }
}