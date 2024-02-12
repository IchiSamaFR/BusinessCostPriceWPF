// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using BusinessCostPriceWPF.ViewModels.Pages.Dashboard;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.Views.Pages
{
    public partial class HomePage : INavigableView<HomeVM>
    {
        public HomeVM ViewModel { get; }

        public HomePage(HomeVM viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
