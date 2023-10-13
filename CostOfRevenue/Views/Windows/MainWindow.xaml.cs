// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using CostOfRevenue.Services;
using CostOfRevenue.ViewModels.Windows;
using System.ComponentModel;
using Wpf.Ui.Controls;

namespace CostOfRevenue.Views.Windows
{
    public partial class MainWindow
    {
        public MainWindowVM ViewModel { get; }

        public MainWindow(
            MainWindowVM viewModel,
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
