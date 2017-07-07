using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    public class SampleArchiveViewModel : BindableBase
    {
        public SampleArchiveViewModel() : base()
        {

        }

        public IEnumerable<Batch> BatchList
        {
            get { return null; }
        }
    }
}
