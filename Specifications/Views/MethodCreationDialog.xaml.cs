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

namespace Specifications.Views
{
    /// <summary>
    /// Interaction logic for MethodCreationDialog.xaml
    /// </summary>
    public partial class MethodCreationDialog : Window
    {
        private Method _methodInstance;

        public MethodCreationDialog(DBEntities entities)
        {
            DataContext = new ViewModels.MethodCreationViewModel(entities, this);
            InitializeComponent();
        }

        public Method MethodInstance
        {
            get { return _methodInstance; }
            set { _methodInstance = value; }
        }
    }
}
