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

namespace Services.Views
{
    /// <summary>
    /// Logica di interazione per SampleLogDialog.xaml
    /// </summary>
    public partial class SampleLogDialog : Window
    {
        public SampleLogDialog()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ConfirmButton.Command.CanExecute(null))
                ConfirmButton.Command.Execute(null);             
        }
    }
}
