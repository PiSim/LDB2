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
    /// Interaction logic for MaterialCreationDialog.xaml
    /// </summary>
    public partial class MaterialCreationDialog : Window
    {
        private string _aspect, _line, _recipe, _type;

        public MaterialCreationDialog(DBEntities entities)
        {
            DataContext = new ViewModels.MaterialCreationViewModel(entities, this);
            InitializeComponent();
        }

        public string MaterialAspect
        {
            get { return _aspect; }
            set { _aspect = value; }
        }

        public string MaterialLine
        {
            get { return _line; }
            set { _line = value; }
        }

        public string MaterialRecipe
        {
            get { return _recipe; }
            set { _recipe = value; }
        }

        public string MaterialType
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
