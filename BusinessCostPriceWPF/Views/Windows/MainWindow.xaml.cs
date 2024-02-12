﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.Services.API;
using BusinessCostPriceWPF.ViewModels.Windows;
using BusinessCostPriceWPF.Views.Pages;
using System.ComponentModel;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.Views.Windows
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

            //APIService.BaseUrl = @"https://businesscostpriceapi.azurewebsites.net/";
            APIService.BaseUrl = @"http://localhost:5281/";
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