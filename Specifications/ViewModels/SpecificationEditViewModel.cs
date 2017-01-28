using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications.ViewModels
{
    internal class SpecificationEditViewModel : BindableBase
    {
        private DBEntities _entities;
        private Specification _instance;

        internal SpecificationEditViewModel(DBEntities entities, Specification instance) 
            : base()
        {
            _entities = entities;
        }

        public Specification Instance
        {
            get { return _instance; }
        }

    }
}
