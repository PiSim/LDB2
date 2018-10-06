using Infrastructure;
using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Specifications.ViewModels
{
    public class StandardEditViewModel : BindableBase
    {
        #region Fields

        private ISpecificationService _specificationService;
        private Std _standardInstance;

        #endregion Fields

        #region Constructors

        public StandardEditViewModel(ISpecificationService specificationService)
        {
            _specificationService = specificationService;

            ConsolidateCommand = new DelegateCommand(
                () =>
                {
                    Views.ConsolidateStandardDialog consolidateDialog = new Views.ConsolidateStandardDialog();
                    consolidateDialog.StandardInstance = _standardInstance;
                    consolidateDialog.ShowDialog();
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationAdmin));

            DeleteCommand = new DelegateCommand(
                () =>
                {
                    _specificationService.DeleteStandard(_standardInstance);
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationAdmin));
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand ConsolidateCommand { get; }

        public DelegateCommand DeleteCommand { get; }

        public IEnumerable<StandardFile> FileList => _standardInstance?.GetFiles();

        public IEnumerable<Method> MethodList => _standardInstance?.GetMethods();

        public IEnumerable<Specification> SpecificationList => _standardInstance?.GetSpecifications();

        public Std StandardInstance
        {
            get => _standardInstance;
            set
            {
                _standardInstance = value;

                OnPropertyChanged("FileList");
                OnPropertyChanged("MethodList");
                OnPropertyChanged("SpecificationList");
                OnPropertyChanged("Visibility");
            }
        }

        public Visibility Visibility => (_standardInstance == null) ? Visibility.Collapsed : Visibility.Visible;

        #endregion Properties
    }
}