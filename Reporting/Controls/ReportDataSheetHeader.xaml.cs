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
    /// Logica di interazione per ReportDataSheetHeader.xaml
    /// </summary>
    public partial class ReportDataSheetHeader : UserControl
    {
        private Batch _batchInstance;
        private Report _reportInstance;

        public ReportDataSheetHeader()
        {
            InitializeComponent();
        }

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
    }
}
