// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace CostOfRevenue.ViewModels.Windows
{
    public partial class MainWindowVM : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Cost Of Revenue";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Accueil",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "Ingrédients",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentOnePage24 },
                TargetPageType = typeof(Views.Pages.IngredientsPage)
            },
            new NavigationViewItem()
            {
                Content = "Recettes",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Notebook24 },
                TargetPageType = typeof(Views.Pages.RecipesPage)
            },
            new NavigationViewItem()
            {
                Content = "Inventaire",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Archive24 },
                TargetPageType = typeof(Views.Pages.StockPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Paramètres",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
