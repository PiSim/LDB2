using DBManager;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tasks.Views
{
    /// <summary>
    /// Interaction logic for TaskCreationDialog.xaml
    /// </summary>
    public partial class TaskCreationDialog : Window
    {
        private Task _taskInstance;
        
        public TaskCreationDialog(DBEntities entities,
                                IMaterialServiceProvider serviceProvider,
                                IReportServiceProvider reportServiceProvider)
        {
            DataContext = new ViewModels.TaskCreationDialogViewModel(entities, 
                                                                    serviceProvider, 
                                                                    reportServiceProvider, 
                                                                    this);
            InitializeComponent();
        }
        
        public Task TaskInstance
        {
            get { return _taskInstance; }
            set { _taskInstance = value; }
        }
    }
}
