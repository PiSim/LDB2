using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace Reporting.ViewModels
{
    public class DocumentPreviewDialogViewModel : BindableBase
    {
        private IDocumentPaginatorSource _documentInstance;

        public DocumentPreviewDialogViewModel()
        {

        }


        public IDocumentPaginatorSource DocumentInstance
        {
            get { return _documentInstance; }
            set
            {
                _documentInstance = value;
                RaisePropertyChanged("DocumentInstance");
            }
        }
    }
}
