using System.Windows;
using System.Windows.Controls;

/*
  Author: Michael Millar
  Date: 09-25-2025
  Description:
    A Custom PopUp Window designed for this project. The window has 2 textboxes to allow for username input
    and only allows submission if both users names are > 3. If both users have differnt first intials
    allows for an additional option for game to be played with first intials.
*/
namespace ToeTactTics_V2
{
    public partial class CustomPopUp : Window
    {
        int minChars = 3;
        public CustomPopUp()
        {
            InitializeComponent();
            textBoxPlayerX.Focus();
        }

        public void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string nameOne = textBoxPlayerX.Text.Trim();
            string nameTwo = textBoxPlayerO.Text.Trim();
            if (nameOne.Length > minChars && nameTwo.Length > minChars)
            {
                buttonSubmit.IsEnabled = true;
                checkboxIntials.IsEnabled = (nameOne[0] != nameTwo[0]);
            }
            else
            {
                buttonSubmit.IsEnabled = false;
                checkboxIntials.IsEnabled = false;
            }
        }

        public void OnSubmit(Object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public void OnCancel(Object sender, RoutedEventArgs e) 
        {
            DialogResult = false;
            Close();
        }
    }
}
