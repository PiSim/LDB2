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
    /// Interaction logic for ProjectPickerDialog.xaml
    /// </summary>
    public partial class ProjectPickerDialog : Window
    {
        private Project _projectInstance;

        public ProjectPickerDialog(DBEntities entities)
        {
            DataContext = new ViewModels.ProjectPickerViewModel(entities, this);
            InitializeComponent();
        }

        public Project ProjectInstance
        {
            get { return _projectInstance; }
            set { _projectInstance = value; }
        }
    }
}
