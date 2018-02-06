using DBManager;
using Infrastructure;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Specifications.ViewModels
{
    public class StandardEditViewModel : BindableBase
    {
        private DBPrincipal _principal;
        private DelegateCommand _consolidate,
                                _delete;
        private ISpecificationService _specificationService;
        private Std _standardInstance;

        public StandardEditViewModel(DBPrincipal principal,
                                    ISpecificationService specificationService)
        {
            _principal = principal;
            _specificationService = specificationService;

            _consolidate = new DelegateCommand(
                () =>
                {
                    Views.ConsolidateStandardDialog consolidateDialog = new Views.ConsolidateStandardDialog();
                    consolidateDialog.StandardInstance = _standardInstance;
                    consolidateDialog.ShowDialog();
                },
                () => _principal.IsInRole(UserRoleNames.SpecificationAdmin));

            _delete = new DelegateCommand(
                () =>
                {
                _specificationService.DeleteStandard(_standardInstance);
                },
                () => _principal.IsInRole(UserRoleNames.SpecificationAdmin));
        }

        public DelegateCommand ConsolidateCommand => _consolidate;

        public DelegateCommand DeleteCommand => _delete;

        public IEnumerable<StandardFile> FileList => _standardInstance?.GetFiles();

        public IEnumerable<Method> MethodList => _standardInstance?.GetMethods();

        public IEnumerable<Specification> SpecificationList => _standardInstance?.GetSpecifications();

        public string StandardFileListRegionName => RegionNames.StandardFileListRegion;

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
        
    }

}
