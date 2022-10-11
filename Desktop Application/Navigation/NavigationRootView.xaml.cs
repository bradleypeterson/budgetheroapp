using Desktop_Application.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application.Navigation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationRootView : Page
    {
        private double NavigationViewControlCompactThresholdWidth => NavigationViewControlCompactThresholdWidth;

        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page
        private readonly List<(string Tag, Type Page)> _pages = new()
        {
            ("profile", typeof(ProfileView)),
            ("dashboard", typeof(DashboardView))
        };

        public NavigationRootView()
        {
            this.InitializeComponent();
        }

        private void NavigationViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Add handler for ContentFrame navigation
            ContentFrame.Navigated += On_Navigated;

            // Load a default page
            NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[0];

            // Navigate to the default page
            NavigationViewControl_Navigate("dashboard", new EntranceNavigationTransitionInfo());
        }

        private void NavigationViewControl_Navigate(string navigationItemTag, NavigationTransitionInfo navigationTransitionInfo)
        {
            Type _page = null;
            if (navigationItemTag == "settings")
            {
                _page = typeof(SettingsView);
            }
            else
            {
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navigationItemTag));
                _page = item.Page;
            }

            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack
            var preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded
            if ((_page is not null) && !Type.Equals(preNavPageType, _page))
            {
                ContentFrame.Navigate(_page, null, navigationTransitionInfo);
            }
        }
        private bool TryGoBack()
        {
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed
            if (NavigationViewControl.IsPaneOpen &&
                (NavigationViewControl.DisplayMode == NavigationViewDisplayMode.Compact ||
                 NavigationViewControl.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavigationViewControl.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(SettingsView))
            {
                // SettingsItem is not part of NavigationViewControl.MenuItems, and doesn't have a Tag
                NavigationViewControl.SelectedItem = (NavigationViewItem)NavigationViewControl.SettingsItem;
                NavigationViewControl.Header = "Settings";
            } 
            else if (ContentFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);

                NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));

                NavigationViewControl.Header =
                    ((NavigationViewItem)NavigationViewControl.SelectedItem)?.Content?.ToString();
            }
        }

        private void NavigationViewControl_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true)
            {
                NavigationViewControl_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null)
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                NavigationViewControl_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
