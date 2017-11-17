using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Reporting.ViewModels
{
    public class DocumentPreviewDialogViewModel : BindableBase
    {
        private FixedDocument _documentInstance;

        public DocumentPreviewDialogViewModel()
        {

        }


        public FixedDocument DocumentInstance
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
