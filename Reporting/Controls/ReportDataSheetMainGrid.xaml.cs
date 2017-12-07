using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Reporting.Controls
{
    /// <summary>
    /// Logica di interazione per ReportDataSheetMainGrid.xaml
    /// </summary>
    public partial class ReportDataSheetMainGrid : UserControl
    {
        private Report _reportInstance;

        public ReportDataSheetMainGrid()
        {
            InitializeComponent();
        }

        public Report ReportInstance
        {
            get => _reportInstance;

            set
            {
                _reportInstance = value;
                ReportHeader.ReportInstance = _reportInstance;
            }
        }
    }
}
