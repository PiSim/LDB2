using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batches.ViewModels
{
    class BatchInfoViewModel : BindableBase
    {
        Batch _instance;

        internal BatchInfoViewModel(Batch instance) : base()
        {
            _instance = instance;
        }

        public string Number
        {
            get { return _instance.number; }
        }
    }
}
