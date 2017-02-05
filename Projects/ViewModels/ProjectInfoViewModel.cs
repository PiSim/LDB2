using DBManager;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.ViewModels
{
    internal class ProjectInfoViewModel : BindableBase
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private Project _projectInstance;

        internal ProjectInfoViewModel(DBEntities entities,
                                    EventAggregator aggregator,
                                    Project instance)
            : base()
        {

        }
    }        
}