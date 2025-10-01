using System.Windows;

/*
  Author: Michael Millar
  Date: 10-01-2025
  Description:
    A Quick Rigid Window I Built in Order to Show The Endgame Results Rather Than Use
    The Ugly Default MessageBox
*/
namespace ToeTactTics_V2
{
    public partial class WinDrawPopUp : Window
    {
        MainWindow main;
        public WinDrawPopUp(MainWindow root)
        {
            main = root;
            InitializeComponent();
            btnContinue.Focus();
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
