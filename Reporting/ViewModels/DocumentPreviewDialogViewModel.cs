using Prism.Mvvm;
using System.Windows.Documents;

namespace Reporting.ViewModels
{
    public class DocumentPreviewDialogViewModel : BindableBase
    {
        #region Fields

        private IDocumentPaginatorSource _documentInstance;

        #endregion Fields

        #region Constructors

        public DocumentPreviewDialogViewModel()
        {
        }

        #endregion Constructors

        #region Properties

        public IDocumentPaginatorSource DocumentInstance
        {
            get { return _documentInstance; }
            set
            {
                _documentInstance = value;
                RaisePropertyChanged("DocumentInstance");
            }
        }

        #endregion Properties
    }
}