using DBManager;
using Infrastructure;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportCreationDialog.xaml
    /// </summary>
    public partial class ReportCreationDialog : Window, IView
    {
        private Report _reportInstance;

        public ReportCreationDialog(DBEntities entities, 
                                    DBPrincipal principal,
                                    IMaterialServiceProvider serviceProvider)
        {
            DataContext = new ViewModels.ReportCreationDialogViewModel(entities, principal, serviceProvider, this);
            InitializeComponent();
        }

        public Report ReportInstance
        {
            get { return _reportInstance; }
            set { _reportInstance = value; }
        }
    }
}
