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

namespace Organizations.Views
{
    /// <summary>
    /// Interaction logic for OrganizationCreationDialog.xaml
    /// </summary>
    public partial class OrganizationCreationDialog : Window
    {
        public OrganizationCreationDialog(DBEntities entities)
        {
            DataContext = new ViewModels.OrganizationCreationViewModel(entities, this);
            InitializeComponent();
        }

        public string OrganizationName
        {
            get { return (DataContext as ViewModels.OrganizationCreationViewModel).OrganizationName; }
        }
    }
}
