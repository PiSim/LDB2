using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Reporting.Formats
{
    public partial class ReportRawDataSheet : FlowDocument
    {

        public ReportRawDataSheet(Report target) : base()
        {
            DataContext = target;
            InitializeComponent();
        }

    }
}
