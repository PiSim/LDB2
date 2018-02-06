using DBManager;
using Infrastructure;
using Infrastructure.Wrappers;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Specifications.ViewModels
{
    public class ConsolidateStandardDialogViewModel : BindableBase
    {
        private DelegateCommand<Window> _cancel,
                                        _confirm;
        private IDataService _dataService;
        private IEnumerable<GenericItemWrapper<Std>> _selectedElements;
        private ISpecificationService _specificationService;
        private Std _standardInstance;

        public ConsolidateStandardDialogViewModel(IDataService dataService,
                                                    ISpecificationService specificationService)
        {
            _dataService = dataService;
            _specificationService = specificationService;

            _cancel = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parentDialog =>
                {
                    _specificationService.ConsolidateStandard(_selectedElements.Where(giw => giw.IsSelected)
                                                                                .Select(giw => giw.Item),
                                                                _standardInstance);
                    parentDialog.DialogResult = true;
                },
                parentDialog => _standardInstance != null);
        }

        public DelegateCommand<Window> CancelCommand => _cancel;
        public DelegateCommand<Window> ConfirmCommand => _confirm;

        public string ParentName => _standardInstance.Name;

        public Std StandardInstance
        {
            get => _standardInstance;
            set
            {
                _standardInstance = value;
                _selectedElements = _dataService.GetStandards()
                                                .Where(std => std.ID != _standardInstance.ID)
                                                .Select(std => new GenericItemWrapper<Std>(std))
                                                .ToList();
                OnPropertyChanged("StandardList");
                OnPropertyChanged("ParentName");
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<GenericItemWrapper<Std>> StandardList => _selectedElements;

    }
}
