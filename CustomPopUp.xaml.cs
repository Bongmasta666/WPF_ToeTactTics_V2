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

        public void OnSubmit(Object sender, RoutedEventArgs e)
        {
            textBoxPlayerX.Text.Trim();
            textBoxPlayerO.Text.Trim();
            if (textBoxPlayerO.Text.Length > 3 && textBoxPlayerO.Text.Length > 3)
            {
                DialogResult = true;
                Close();
            }
        }

        public void OnCancel(Object sender, RoutedEventArgs e) 
        {
            DialogResult = false;
            Close();
        }
    }
}
