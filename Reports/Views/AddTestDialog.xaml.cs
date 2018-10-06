using Infrastructure.Wrappers;
using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.Generic;
using System.Windows;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for AddTestDialog.xaml
    /// </summary>
    public partial class AddTestDialog : Window, IView
    {
        #region Constructors

        public AddTestDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Report ReportInstance
        {
            get { return (DataContext as ViewModels.AddTestDialogViewModel).ReportInstance; }
            set
            {
                (DataContext as ViewModels.AddTestDialogViewModel).ReportInstance = value;
            }
        }

        public IEnumerable<ReportItemWrapper> TestList => (DataContext as ViewModels.AddTestDialogViewModel).TestList;

        #endregion Properties
    }
}