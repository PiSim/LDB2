using DBManager;
using Microsoft.Practices.Prism.Mvvm;
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

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for ColorPickerDialog.xaml
    /// </summary>
    public partial class ColorPickerDialog : Window, IView
    {
        public ColorPickerDialog()
        {
            InitializeComponent();
        }

        public Colour ColourInstance
        {
            get { return (DataContext as ViewModels.ColorPickerDialogViewModel).SelectedColour; }
            set { (DataContext as ViewModels.ColorPickerDialogViewModel).SelectedColour = value; }
        } 
    }
}
