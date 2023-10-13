using CostOfRevenue.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace CostOfRevenue.Views.Pages
{
    /// <summary>
    /// Logique d'interaction pour FurnituresPage.xaml
    /// </summary>
    public partial class FurnituresPage : INavigableView<FurnituresVM>
    {
        public FurnituresVM ViewModel { get; }

        public FurnituresPage(FurnituresVM viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
