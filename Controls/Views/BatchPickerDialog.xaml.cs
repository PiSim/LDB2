using Microsoft.Practices.Prism.Mvvm;
using System.Windows;

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for BatchPickerDialog.xaml
    /// </summary>
    public partial class BatchPickerDialog : Window, IView
    {
        #region Constructors

        public BatchPickerDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string BatchNumber
        {
            get { return (DataContext as ViewModels.BatchPickerDialogViewModel).Number; }
            set { (DataContext as ViewModels.BatchPickerDialogViewModel).Number = value; }
        }

        #endregion Properties
    }
}