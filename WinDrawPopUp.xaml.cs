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
using System.Windows.Shapes;

namespace ToeTactTics_V2
{
    /// <summary>
    /// Interaction logic for WinDrawPopUp.xaml
    /// </summary>
    public partial class WinDrawPopUp : Window
    {
        MainWindow main;
        public WinDrawPopUp(MainWindow root)
        {
            main = root;
            InitializeComponent();
        }

        public void OnContinue(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void OnQuitGame(object sender, RoutedEventArgs e)
        {
            Close();
            main.OnQuitGame(sender, e);
        }

        public void ShowText(string text)
        {
            textbox.Text = text;
        }
    }
}
