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
    public partial class CustomPopUp : Window
    {
        public CustomPopUp()
        {
            InitializeComponent();
            textBoxPlayerX.Focus();
        }

        public void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string nameOne = textBoxPlayerX.Text.Trim();
            string nameTwo = textBoxPlayerO.Text.Trim();
            if (nameOne.Length > 3 && nameTwo.Length > 3)
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
