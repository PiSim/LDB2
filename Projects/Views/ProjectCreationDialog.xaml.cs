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

namespace Projects.Views
{
    /// <summary>
    /// Interaction logic for ProjectCreationDialog.xaml
    /// </summary>
    public partial class ProjectCreationDialog : Window, IView
    {
        public ProjectCreationDialog()
        {
            InitializeComponent();
        }

        public Project ProjectInstance
        {
            get { return (DataContext as ViewModels.ProjectCreationDialogViewModel).ProjectInstance; }
        }
    }
}
