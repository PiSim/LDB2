using DataAccess;
using Infrastructure;
using Infrastructure.Wrappers;
using LabDbContext;
using Prism.Commands;
using Prism.Mvvm;
using Specifications.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Specifications.ViewModels
{
    public class ConsolidateStandardDialogViewModel : BindableBase
    {
        #region Fields

        private IDataService<LabDbEntities> _labDbData;
        private ISpecificationService _specificationService;
        private Std _standardInstance;

        #endregion Fields

        #region Constructors

        public ConsolidateStandardDialogViewModel(IDataService<LabDbEntities> labDbData,
                                                    ISpecificationService specificationService)
        {
            _labDbData = labDbData;
            _specificationService = specificationService;

            CancelCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    _specificationService.ConsolidateStandard(StandardList.Where(giw => giw.IsSelected)
                                                                                .Select(giw => giw.Item),
                                                                _standardInstance);
                    parentDialog.DialogResult = true;
                },
                parentDialog => _standardInstance != null);
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand<Window> CancelCommand { get; }
        public DelegateCommand<Window> ConfirmCommand { get; }

        public string ParentName => _standardInstance.Name;

        public Std StandardInstance
        {
            get => _standardInstance;
            set
            {
                _standardInstance = value;
                StandardList = _labDbData.RunQuery(new StandardsQuery())
                                        .Select(std => new GenericItemWrapper<Std>(std))
                                        .ToList();
                RaisePropertyChanged("StandardList");
                RaisePropertyChanged("ParentName");
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<GenericItemWrapper<Std>> StandardList { get; private set; }

        #endregion Properties
    }
}