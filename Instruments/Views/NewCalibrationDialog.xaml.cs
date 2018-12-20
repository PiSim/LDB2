using LInst;
using System.Windows;

namespace Instruments.Views
{
    /// <summary>
    /// Interaction logic for NewCalibrationDialog.xaml
    /// </summary>
    public partial class NewCalibrationDialog : Window
    {
        #region Constructors

        public NewCalibrationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Instrument InstrumentInstance
        {
            get { return (DataContext as ViewModels.NewCalibrationDialogViewModel).InstrumentInstance; }
            set { (DataContext as ViewModels.NewCalibrationDialogViewModel).InstrumentInstance = value; }
        }

        public CalibrationReport ReportInstance => (DataContext as ViewModels.NewCalibrationDialogViewModel).ReportInstance;

        #endregion Properties
    }
}