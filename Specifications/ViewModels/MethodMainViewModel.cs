using DBManager;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Specifications.ViewModels
{
    public class MethodMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _newMethod;
        private Method _selectedMethod;
        private ObservableCollection<Method> _methodList;
        private UnityContainer _container;

        public MethodMainViewModel(DBEntities entities,
                                    UnityContainer container) : base()
        {
            _container = container;
            _entities = entities;
            _methodList = new ObservableCollection<Method>(_entities.Methods);

            _newMethod = new DelegateCommand(
                () =>
                {
                    Views.MethodCreationDialog creationDialog = 
                        _container.Resolve<Views.MethodCreationDialog>();

                    if (creationDialog.ShowDialog() == true)
                    {
                        Method temp = creationDialog.MethodInstance;
                        _methodList.Add(temp);
                        SelectedMethod = temp;
                    }
                });
        }

        public ObservableCollection<Method> MethodList
        {
            get
            {
                return _methodList;
            }
        }


        public DelegateCommand NewMethodCommand
        {
            get { return _newMethod; }
        }

        public Method SelectedMethod
        {
            get
            {
                return _selectedMethod;
            }

            set
            {
                _selectedMethod = value;
                OnPropertyChanged("SelectedMethod");
            }
        }
    }
}
