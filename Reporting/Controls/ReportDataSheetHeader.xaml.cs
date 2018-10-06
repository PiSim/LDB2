using LabDbContext;
using System.Windows.Controls;

namespace Reporting.Controls
{
    /// <summary>
    /// Logica di interazione per ReportDataSheetHeader.xaml
    /// </summary>
    public partial class ReportDataSheetHeader : UserControl
    {
        #region Fields

        private Batch _batchInstance;
        private Report _reportInstance;

        #endregion Fields

        #region Constructors

        public ReportDataSheetHeader()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Batch BatchInstance
        {
            get => _batchInstance;
            set
            {
                _batchInstance = value;

                BatchNumberBox.Text = _batchInstance?.Number;
                MaterialCodeBox.Text = _batchInstance?.MaterialFullCode;
                ConstructionBox.Text = _batchInstance?.Material?.ExternalConstruction?.Name;
                ColorNameBox.Text = _batchInstance?.Material?.Recipe?.Colour?.Name;
            }
        }

        public Report ReportInstance
        {
            get => _reportInstance;
            set
            {
                _reportInstance = value;
                BatchInstance = _reportInstance?.Batch;
                ReportNumberBox.Text = _reportInstance?.Number.ToString();
                DateBox.Text = _reportInstance?.StartDate;
                TechNameBox.Text = _reportInstance?.Author?.Name;
                OemBox.Text = _reportInstance?.Batch?.Material?.Project?.Oem?.Name;
                ProjectBox.Text = _reportInstance?.Batch?.Material?.Project?.ProjectString;
                SpecificationBox.Text = _reportInstance?.SpecificationVersion?.VersionString;
                NotesBox.Text = _reportInstance.Description;
            }
        }

        #endregion Properties
    }
}