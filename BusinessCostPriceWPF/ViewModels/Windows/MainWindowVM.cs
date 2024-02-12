// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using BusinessCostPriceWPF.Services.API;
using BusinessCostPriceWPF.Views.Pages;
using BusinessCostPriceWPF.Views.Pages.Login;
using BusinessCostPriceWPF.Views.Windows;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.ViewModels.Windows
{
    public partial class MainWindowVM : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Business Cost Price";

        [ObservableProperty]
        private bool _isLogged;

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Accueil",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(HomePage)
            },
            new NavigationViewItem()
            {
                Content = "Fournitures",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Box24 },
                TargetPageType = typeof(FurnituresPage)
            },
            new NavigationViewItem()
            {
                Content = "Ingrédients",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentOnePage24 },
                TargetPageType = typeof(IngredientsPage)
            },
            new NavigationViewItem()
            {
                Content = "Recettes",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Notebook24 },
                TargetPageType = typeof(RecipesPage)
            },
            new NavigationViewItem()
            {
                Content = "Inventaire",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Archive24 },
                TargetPageType = typeof(StockPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Paramètres",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };

        [ObservableProperty]
        private LoginView _loginView;

        private IServiceProvider _serviceProvider;
        public MainWindowVM(LoginView loginView, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _loginView = loginView;
            _loginView.ViewModel.OnLogged += OnLogged;
        }
        private void OnLogged()
        {
            IsLogged = true;
            if (IsLogged)
            {
                var nav = (INavigationService)_serviceProvider.GetService(typeof(INavigationService));
                nav.Navigate(typeof(HomePage));
            }
        }
    }
}
