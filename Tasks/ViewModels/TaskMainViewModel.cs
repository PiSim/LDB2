using DBManager;
using Controls.Views;
using Microsoft.Practices.Unity;
using Prism.Commands;
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
        private DelegateCommand _newTask;
        private EventAggregator _eventAggregator;
        private ObservableCollection<DBManager.Task> _taskList;
        private UnityContainer _container;

        internal TaskMainViewModel(DBEntities entities, 
                                    EventAggregator eventAggregator,
                                    UnityContainer container) 
            : base()
        {
            _container = container;
            _entities = entities;
            _eventAggregator = eventAggregator;
            _taskList = new ObservableCollection<DBManager.Task>(_entities.Tasks);
            
            _newTask = new DelegateCommand(
                () => 
                {
                    TaskCreationDialog taskDialog = _container.Resolve<TaskCreationDialog>();
                    if (taskDialog.ShowDialog() == true)
                    {
                        
                    }
                } );
        }
        
        public ObservableCollection<DBManager.Task> TaskList
        {
            get { return _taskList; }
        }
    }
}
