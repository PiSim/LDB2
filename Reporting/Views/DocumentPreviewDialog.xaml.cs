using System.Windows;
using System.Windows.Documents;

namespace Reporting.Views
{
    /// <summary>
    /// Logica di interazione per DocumentPreviewDialog.xaml
    /// </summary>
    public partial class DocumentPreviewDialog : Window
    {
        #region Constructors

        public DocumentPreviewDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public IDocumentPaginatorSource Document
        {
            get { return MainViewer.Document; }
            set
            {
                MainViewer.Document = value;
            }
        }

        #endregion Properties
    }
}