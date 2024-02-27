using BusinessCostPriceWPF.ViewModels.Pages.Dashboard;
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

namespace BusinessCostPriceWPF.Views.Pages.Dashboard
{
    /// <summary>
    /// Logique d'interaction pour UserPage.xaml
    /// </summary>
    public partial class UserPage : INavigableView<UserVM>
    {
        public UserVM ViewModel { get; }

        public UserPage(UserVM viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
