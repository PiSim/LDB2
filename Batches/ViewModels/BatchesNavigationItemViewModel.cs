using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batches.ViewModels
{
    class BatchesNavigationItemViewModel : Prism.Mvvm.BindableBase
    {
        private DelegateCommand _onSelected;

        public BatchesNavigationItemViewModel() : base()
        {
            _onSelected = new DelegateCommand(
                () => { });
        }
    }
}
