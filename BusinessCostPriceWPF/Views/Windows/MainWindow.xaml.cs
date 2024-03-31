// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using BusinessCostPriceAPI.Client.Services;
using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.ViewModels.Windows;
using System.ComponentModel;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.Views.Windows
{
    public partial class MainWindow
    {
        public MainWindowVM ViewModel { get; }
        private ExceptionService _exceptionService { get; }

        public MainWindow(
            MainWindowVM viewModel,
            ExceptionService exceptionService,
            INavigationService navigationService,
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            IContentDialogService contentDialogService
        )
        {
            Wpf.Ui.Appearance.Watcher.Watch(this);

            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            navigationService.SetNavigationControl(NavigationView);
            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            contentDialogService.SetContentPresenter(RootContentDialog);

            NavigationView.SetServiceProvider(serviceProvider);

#if DEBUG
            APIService.Url = @"http://localhost:5281/";
#else
            APIService.Url = @"https://businesscostpriceapi.azurewebsites.net/";
#endif

            _exceptionService = exceptionService;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Focus();
            DataService.SaveIngredients();
            DataService.SaveRecipes();
            DataService.SaveFurnitures();

            base.OnClosing(e);
        }
    }
}
