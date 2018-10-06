using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using Infrastructure.Wrappers;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Specifications.ViewModels
{
    public class SpecificationVersionEditViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly ISpecificationService _specificationService;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private bool _editMode;
        private IDataService<LabDbEntities> _labDbData;
        private IEventAggregator _eventAggregator;
        private List<RequirementWrapper> _requirementList;
        private SpecificationVersion _specificationVersionInstance;

        #endregion Fields

        #region Constructors

        public SpecificationVersionEditViewModel(IDataService<LabDbEntities> labDbData,
                                                IEventAggregator eventAggregator,
                                                ISpecificationService specificationService)
        {
            _specificationService = specificationService;
            _editMode = false;
            _eventAggregator = eventAggregator;
            _labDbData = labDbData;


            DeleteRequirementCommand = new DelegateCommand<Requirement>(
                req =>
                {
                    req.Delete();

                    _specificationVersionInstance.Load();

                    GenerateRequirementList();

                    RaisePropertyChanged("RequirementList");
                });

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _specificationVersionInstance.Update();

                    if (_specificationVersionInstance == null)
                        return;

                    if (_specificationVersionInstance.IsMain)
                        _specificationService.UpdateRequirements(_requirementList.Select(req => req.RequirementInstance));
                    else
                        _specificationService.UpdateRequirements(_requirementList.Where(req => req.IsOverride)
                                                                                .Select(req => req.RequirementInstance));

                    EditMode = false;
                },
                () => _editMode
                    && !HasErrors);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => CanEdit && !_editMode);

            StartTestListEditCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(SpecificationViewNames.AddMethod,
                                                                null,
                                                                RegionNames.SpecificationVersionTestListEditRegion);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationEdit));

            _eventAggregator.GetEvent<SpecificationMethodListChanged>()
                            .Subscribe(
                spc =>
                {
                    if (spc.ID == _specificationVersionInstance.SpecificationID)
                    {
                        GenerateRequirementList();
                        RaisePropertyChanged("RequirementList");
                    }
                });
        }

        #endregion Constructors

        #region Commands

        public DelegateCommand<Requirement> DeleteRequirementCommand { get; }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand StartEditCommand { get; }
        public DelegateCommand StartTestListEditCommand { get; }

        #endregion Commands

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _validationErrors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion INotifyDataErrorInfo interface elements

        #region Properties

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                RaisePropertyChanged("IsOverrideVisibility");

                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
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

        public IEnumerable<RequirementWrapper> RequirementList => _requirementList;

        public bool SelectedVersionIsNotMain
        {
            get
            {
                if (_specificationVersionInstance == null)
                    return false;

                return !_specificationVersionInstance.IsMain;
            }
        }

        public Visibility SpecificationVersionEditViewVisibility => (_specificationVersionInstance != null) ? Visibility.Visible : Visibility.Hidden;

        public SpecificationVersion SpecificationVersionInstance
        {
            get { return _specificationVersionInstance; }
            set
            {
                EditMode = false;

                _specificationVersionInstance = value;

                GenerateRequirementList();

                RaisePropertyChanged("RequirementList");
                RaisePropertyChanged("SpecificationVersionEditViewVisibility");
                RaisePropertyChanged("SpecificationVersionName");
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

        private bool CanEdit => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationEdit);

        #endregion Properties

        #region Methods

        private void GenerateRequirementList()
        {
            if (_specificationVersionInstance == null)
                _requirementList = new List<RequirementWrapper>();
            else
                _requirementList = new List<RequirementWrapper>(_specificationVersionInstance.GenerateRequirementList()
                                                                                            .Select(req => new RequirementWrapper(req))
                                                                                            .ToList());
            SetRequirementListOnIsOverrideChanged();
        }

        private void SetRequirementListOnIsOverrideChanged()
        {
            if (_requirementList == null)
                return;

            foreach (RequirementWrapper rwr in _requirementList)
                rwr.IsOverrideChanged += OnIsOverrideChanged;
        }

        private void OnIsOverrideChanged(object sender, IsOverrideChangedEventArgs e)
        {
            if (e.IsOverride)
                AddOverride(sender as RequirementWrapper);

            else
                RemoveOverride(sender as RequirementWrapper);
        }

        public void AddOverride(RequirementWrapper wrapper)
        {
            Requirement newOverride = new Requirement
            {
                Description = wrapper.RequirementInstance.Description,
                IsOverride = true,
                MethodVariantID = wrapper.RequirementInstance.MethodVariantID,
                OverriddenID = wrapper.RequirementInstance.ID,
                SpecificationVersionID = SpecificationVersionInstance.ID
            };

            foreach (SubRequirement subReq in wrapper.RequirementInstance.SubRequirements)
            {
                SubRequirement tempSub = new SubRequirement
                {
                    SubMethodID = subReq.SubMethodID,
                    RequiredValue = subReq.RequiredValue
                };

                newOverride.SubRequirements.Add(tempSub);
            }

            _labDbData.Execute(new InsertEntityCommand(newOverride));
            wrapper.RequirementInstance = newOverride;
        }

        private void RemoveOverride(RequirementWrapper wrapper)
        {
            int overrID = (int)wrapper.RequirementInstance.OverriddenID;
            _labDbData.Execute(new DeleteEntityCommand(wrapper.RequirementInstance));
            wrapper.RequirementInstance = _labDbData.RunQuery(new RequirementsQuery() { OrderResults = false, EagerLoadingEnabled = false } )
                                                    .FirstOrDefault(req => req.ID == overrID);
        }
        #endregion Methods
    }
}