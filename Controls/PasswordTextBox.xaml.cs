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

namespace WPF_Supermarket.Controls
{
    /// <summary>
    /// Interaction logic for PasswordTextBox.xaml
    /// </summary>
    public partial class PasswordTextBox : UserControl
    {
        public PasswordTextBox()
        {
            InitializeComponent();
            PasswordDisplay = string.Empty;
            Password = string.Empty;
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordTextBox), new PropertyMetadata(string.Empty));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        private string passwordDisplay;

        public string PasswordDisplay
        {
            get { return passwordDisplay; }
            set
            {
                passwordDisplay = value;
                passwordBox.Text = new string('•', passwordDisplay.Length);
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
            Password += e.Text;
            PasswordDisplay += e.Text;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && Password.Length > 0)
            {
                Password = Password.Substring(0, Password.Length - 1);
                PasswordDisplay = PasswordDisplay.Substring(0, PasswordDisplay.Length - 1);
                e.Handled = true;
            }
        }
    }
}
