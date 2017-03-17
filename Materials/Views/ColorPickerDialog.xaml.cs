using DBManager;
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

namespace Materials.Views
{
    /// <summary>
    /// Interaction logic for ColorPickerDialog.xaml
    /// </summary>
    public partial class ColorPickerDialog : Window
    {
        private Colour _colourInstance;
        
        public ColorPickerDialog(DBEntities entities)
        {
            DataContext = new ViewModels.ColorPickerDialogViewModel(entities, this);
            InitializeComponent();
        }

        public Colour ColourInstance
        {
            get { return _colourInstance; }
            set { _colourInstance = value; }
        } 
    }
}
