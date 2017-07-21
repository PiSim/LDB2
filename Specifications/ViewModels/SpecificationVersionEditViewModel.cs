using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Specifications.ViewModels
{
    public class SpecificationVersionEditViewModel : BindableBase, INotifyDataErrorInfo
    {
        private bool _editMode;
        private DBPrincipal _principal;
        private DelegateCommand _save,
                                _startEdit;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private List<RequirementWrapper> _requirementList;
        private SpecificationVersion _specificationVersionInstance;
        
        public SpecificationVersionEditViewModel(DBPrincipal principal)
        {
            _editMode = false;
            _principal = principal;

            _save = new DelegateCommand(
                () =>
                {
                    _specificationVersionInstance.Update();

                    if (_specificationVersionInstance == null)
                        return;

                    if (_specificationVersionInstance.IsMain)
                        SpecificationService.UpdateRequirements(_requirementList.Select(req => req.RequirementInstance));

                    else
                        SpecificationService.UpdateRequirements(_requirementList.Where(req => req.IsOverride)
                                                                                .Select(req => req.RequirementInstance));

                    EditMode = false;
                },
                () => _editMode
                    && !HasErrors);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => CanEdit && !_editMode);
        }

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            _save.RaiseCanExecuteChanged();
        }

        #endregion

        private bool CanEdit
        {
            get { return _principal.IsInRole(UserRoleNames.SpecificationEdit); }
        }

        private void GenerateRequirementList()
        {
            if (_specificationVersionInstance == null)
                _requirementList = new List<RequirementWrapper>();

            else
                _requirementList = new List<RequirementWrapper>(_specificationVersionInstance.GenerateRequirementList()
                                                                                            .Select(req => new RequirementWrapper(req, _specificationVersionInstance))
                                                                                            .ToList());
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                RaisePropertyChanged("IsOverrideVisibility");

                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public Visibility IsOverrideVisibility
        {
            get
            {
                if (_specificationVersionInstance == null)
                    return Visibility.Collapsed;

                else
                    return (_editMode && !_specificationVersionInstance.IsMain) 
                        ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public IEnumerable<RequirementWrapper> RequirementList
        {
            get { return _requirementList; }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public bool SelectedVersionIsNotMain
        {
            get
            {
                if (_specificationVersionInstance == null)
                    return false;

                return !_specificationVersionInstance.IsMain;
            }
        }


        public SpecificationVersion SpecificationVersionInstance
        {
            get { return _specificationVersionInstance; }
            set
            {
                EditMode = false;

                _specificationVersionInstance = value;
                _specificationVersionInstance.Load();
                
                GenerateRequirementList();

                RaisePropertyChanged("RequirementList");
                RaisePropertyChanged("SpecificationVersionEditViewVisibility");
                RaisePropertyChanged("SpecificationVersionName");
            }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }

        public Visibility SpecificationVersionEditViewVisibility
        {
            get
            {
                return (_specificationVersionInstance != null) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public string SpecificationVersionName
        {
            get
            {
                return _specificationVersionInstance?.Name;
            }

            set
            {
                _specificationVersionInstance.Name = value;
                

                if (!string.IsNullOrEmpty(_specificationVersionInstance?.Name))
                {
                    if (_validationErrors.ContainsKey("SpecificationVersionName"))
                    {
                        _validationErrors.Remove("SpecificationVersionName");
                        RaiseErrorsChanged("SpecificationVersionName");
                    }
                }

                else
                {
                    _validationErrors["SpecificationVersionName"] = new List<string>() { "Nome non valido" };
                    RaiseErrorsChanged("SpecificationVersionName");
                }
            }
        }
    }
}
