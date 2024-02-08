using BusinessCostPriceWPF.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.Views.Pages
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
