using DBManager;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Windows;

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for NewPODialog.xaml
    /// </summary>
    public partial class NewPODialog : Window, IView
    {
        public NewPODialog()
        {
            InitializeComponent();
        }

        public string Number
        {
            get { return (DataContext as ViewModels.NewPODialogViewModel).Number; }
            internal set { (DataContext as ViewModels.NewPODialogViewModel).Number = value; }
        }

        public Organization Supplier
        {
            get { return (DataContext as ViewModels.NewPODialogViewModel).SelectedOrganization; }
            set { (DataContext as ViewModels.NewPODialogViewModel).SelectedOrganization = value; }
        }

        public float Total
        {
            get { return (DataContext as ViewModels.NewPODialogViewModel).Total; }
            internal set { (DataContext as ViewModels.NewPODialogViewModel).Total = value; }
        }

        public void SetSupplier(Organization target)
        {
            (DataContext as ViewModels.NewPODialogViewModel).SetOrganization(target);
        }
    }
}
