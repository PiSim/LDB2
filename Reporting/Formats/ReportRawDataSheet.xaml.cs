using LabDbContext;
using System.Windows.Documents;

namespace Reporting.Formats
{
    public partial class ReportRawDataSheet : FlowDocument
    {
        #region Constructors

        public ReportRawDataSheet(Report target) : base()
        {
            DataContext = target;
            InitializeComponent();
        }

        #endregion Constructors
    }
}