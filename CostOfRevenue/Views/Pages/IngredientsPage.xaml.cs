using CostOfRevenue.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
