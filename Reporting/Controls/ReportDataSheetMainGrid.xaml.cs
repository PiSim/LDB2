using LabDbContext;
using System.Windows.Controls;

namespace Reporting.Controls
{
    /// <summary>
    /// Logica di interazione per ReportDataSheetMainGrid.xaml
    /// </summary>
    public partial class ReportDataSheetMainGrid : UserControl
    {
        #region Fields

        private Report _reportInstance;

        #endregion Fields

        #region Constructors

        public ReportDataSheetMainGrid()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Report ReportInstance
        {
            get => _reportInstance;

            set
            {
                _reportInstance = value;
                ReportHeader.ReportInstance = _reportInstance;
            }
        }

        #endregion Properties
    }
}