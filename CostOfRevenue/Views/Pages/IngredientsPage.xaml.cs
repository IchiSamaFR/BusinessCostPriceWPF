using CostOfRevenue.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace CostOfRevenue.Views.Pages
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
