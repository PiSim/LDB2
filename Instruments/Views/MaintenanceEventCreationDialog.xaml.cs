using LInst;
using System.Windows;

namespace Instruments.Views
{
    /// <summary>
    /// Logica di interazione per MaintenanceEventCreationDialog.xaml
    /// </summary>
    public partial class MaintenanceEventCreationDialog : Window
    {
        #region Constructors

        public MaintenanceEventCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public InstrumentMaintenanceEvent InstrumentEventInstance => (DataContext as ViewModels.MaintenanceEventCreationDialogViewModel).EventInstance;

        public Instrument InstrumentInstance
        {
            get { return (DataContext as ViewModels.MaintenanceEventCreationDialogViewModel).InstrumentInstance; }
            set { (DataContext as ViewModels.MaintenanceEventCreationDialogViewModel).InstrumentInstance = value; }
        }

        #endregion Properties
    }
}