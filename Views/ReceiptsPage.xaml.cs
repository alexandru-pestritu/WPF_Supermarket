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
using WPF_Supermarket.ViewModels;

namespace WPF_Supermarket.Views
{
    /// <summary>
    /// Interaction logic for ReceiptsPage.xaml
    /// </summary>
    public partial class ReceiptsPage : Page
    {
        public ReceiptsPage()
        {
            InitializeComponent();
            DataContext = new ReceiptViewModel();
        }
    }
}
