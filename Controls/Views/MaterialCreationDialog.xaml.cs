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

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for MaterialCreationDialog.xaml
    /// </summary>
    public partial class MaterialCreationDialog : Window
    {
        private Material _validatedMaterial;

        public MaterialCreationDialog(DBEntities entities)
        {
            DataContext = new ViewModels.MaterialCreationViewModel(entities, this);
            InitializeComponent();
        }

        public Material ValidatedMaterial
        {
            get { return _validatedMaterial; }
            set { _validatedMaterial = value; }
        }
    }
}
