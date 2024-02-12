using BusinessCostPriceWPF.ViewModels.Pages.Dashboard;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.Views.Pages
{
    /// <summary>
    /// Logique d'interaction pour IngredientsPage.xaml
    /// </summary>
    public partial class IngredientsPage : INavigableView<IngredientsVM>
    {
        public IngredientsVM ViewModel { get; }

        public IngredientsPage(IngredientsVM viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
