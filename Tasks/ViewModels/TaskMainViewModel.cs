using DBManager;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.ViewModels
{
    internal class TaskMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private ObservableCollection<DBManager.Task> _taskList;

        internal TaskMainViewModel(DBEntities entities, EventAggregator eventAggregator) 
            : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
            _taskList = new ObservableCollection<DBManager.Task>(_entities.Tasks);
        }
        
        public ObservableCollection<DBManager.Task> TaskList
        {
            get { return _taskList; }
        }
    }
}
