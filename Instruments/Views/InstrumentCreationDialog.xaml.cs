using LInst;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows;

namespace Instruments.Views
{
    /// <summary>
    /// Interaction logic for InstrumentCreationDialog.xaml
    /// </summary>
    public partial class InstrumentCreationDialog : Window, IView
    {
        #region Constructors

        public InstrumentCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Instrument InstrumentInstance => (DataContext as ViewModels.InstrumentCreationDialogViewModel).InstrumentInstance;

        #endregion Properties
    }
}