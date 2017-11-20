
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
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace Reporting.Views
{
    /// <summary>
    /// Logica di interazione per DocumentPreviewDialog.xaml
    /// </summary>
    public partial class DocumentPreviewDialog : Window
    {
        public DocumentPreviewDialog()
        {
            InitializeComponent();
        }

        public IDocumentPaginatorSource Document
        {
            get { return MainViewer.Document; }
            set
            {
                MainViewer.Document = value;
            }
        }
    }
}
