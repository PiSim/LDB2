using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments.ViewModels
{
    internal class InstrumentMainViewModel : BindableBase
    {
        private DBEntities _entities;

        internal InstrumentMainViewModel(DBEntities entities) : base()
        {

        }

    }
}
