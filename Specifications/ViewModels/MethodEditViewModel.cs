using DBManager;
using DBManager.EntityExtensions;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBManager.Services;
using Infrastructure;
using System.Collections.Specialized;

namespace Specifications.ViewModels
{
    public class MethodEditViewModel : BindableBase
    {
        private bool _editMode;
        private DBPrincipal _principal;
        private EventAggregator _eventAggregator;
        private IDataService _dataService;
        private IEnumerable<Organization> _organizationList;
        private IEnumerable<Property> _propertyList;
        private IEnumerable<SubMethod> _subMethodList;
        private ISpecificationService _specificationService;
        private Method _methodInstance;
        private ObservableCollection<MethodVariant> _methodVariantList;
        private Organization _selectedOrganization;
        private Property _selectedProperty;
        private Specification _selectedSpecification;
        private StandardFile _selectedFile;
        private Test _selectedTest;

        public MethodEditViewModel(DBPrincipal principal,
                                    EventAggregator aggregator,
                                    IDataService dataService,
                                    ISpecificationService specificationService) : base()
        {
            _dataService = dataService;
            _editMode = false;
            _eventAggregator = aggregator;
            _principal = principal;
            _specificationService = specificationService;

            _organizationList = _dataService.GetOrganizations(OrganizationRoleNames.StandardPublisher);
            _propertyList = _dataService.GetProperties();

            _subMethodList = new List<SubMethod>();

            AddFileCommand = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog
                    {
                        Multiselect = true
                    };

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            StandardFile temp = new StandardFile
                            {
                                Path = pth,
                                Description = "",
                                StandardID = _methodInstance.StandardID
                            };

                            temp.Create();
                        }

                        RaisePropertyChanged("FileList");
                    }
                },
                () => CanModify);

            CancelEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = false;
                },
                () => EditMode);

            OpenFileCommand = new DelegateCommand(
                () =>
                {
                    System.Diagnostics.Process.Start(_selectedFile.Path);
                },
                () => _selectedFile != null);

            OpenReportCommand = new DelegateCommand(
                () =>
                {
                NavigationToken token = new NavigationToken((_selectedTest.TestRecord.RecordTypeID == 1) ? ReportViewNames.ReportEditView : ReportViewNames.ExternalReportEditView,
                                                            _selectedTest.TestRecord.Reports.First());

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                },
                () => _selectedTest != null);
            
            OpenSpecificationCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationEdit,
                                                                SelectedSpecification);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedSpecification != null);

            RemoveFileCommand = new DelegateCommand(
                () =>
                {
                    _selectedFile.Delete();
                    SelectedFile = null;
                },
                () => EditMode && _selectedFile != null);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _methodInstance.Update();

                    _specificationService.UpdateSubMethods(_subMethodList);
                    _specificationService.UpdateMethodVariantRange(_methodVariantList);

                    if (_selectedOrganization.ID != _methodInstance.Standard.OrganizationID)
                    {
                        _methodInstance.Standard.OrganizationID = _selectedOrganization.ID;
                        _methodInstance.Standard.Update();
                    }


                    EditMode = false;
                },
                () => _editMode);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => CanModify && !_editMode);

            UpdateCommand = new DelegateCommand(
                () =>
                {
                    _specificationService.ModifyMethodTestList(_methodInstance);
                },
                () => CanModify);

        }

        #region Commands


        public DelegateCommand AddFileCommand { get; }

        public DelegateCommand CancelEditCommand { get; }
        
        public DelegateCommand OpenFileCommand { get; }

        public DelegateCommand OpenReportCommand { get; }

        public DelegateCommand OpenSpecificationCommand { get; }

        public DelegateCommand RemoveFileCommand { get; }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand StartEditCommand { get; }

        #endregion


        #region Methods

        private void OnMethodVariantCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (MethodVariant mtdvar in e.NewItems)
                    mtdvar.MethodID = _methodInstance.ID;
            }

            if (e.OldItems != null)
            {
                foreach (MethodVariant mtdvar in e.OldItems)
                    mtdvar.RemoveOrSetObsolete();

                RaisePropertyChanged("IsMoreThanOneVariant");
            }

        }

        private void SetMethodVariantCollectionChangedHandler()
        {
            if (MethodVariantList == null)
                return;

            MethodVariantList.CollectionChanged += OnMethodVariantCollectionChanged;
        }

        #endregion

        #region Properties

        private bool CanModify => _principal.IsInRole(UserRoleNames.SpecificationEdit);
        
        public double Duration
        {
            get
            {
                if (_methodInstance != null)
                    return _methodInstance.Duration;
                else
                    return 0;
            }
            set
            {
                if (_methodInstance !=null)
                    _methodInstance.Duration = value;
            }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("CanModifyTests");
                RaisePropertyChanged("EditMode");
                AddFileCommand.RaiseCanExecuteChanged();
                CancelEditCommand.RaiseCanExecuteChanged();
                RemoveFileCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<StandardFile> FileList
        {
            get
            {
                return _methodInstance.GetFiles();
            }
        }

        public bool IsMoreThanOneVariant => _methodVariantList?.Count != 0;

        public string MethodEditSpecificationListRegionName
        {
            get { return RegionNames.MethodEditSpecificationListRegion; }
        }

        public Method MethodInstance
        {
            get { return _methodInstance; }
            set
            {
                EditMode = false;

                _methodInstance = value;
                if (_methodInstance != null)
                    _methodInstance.Load(true);

                MethodVariantList = new ObservableCollection<MethodVariant>(_methodInstance?.GetVariants());
                
                SelectedOrganization = _organizationList.FirstOrDefault(org => org.ID == _methodInstance?.Standard.OrganizationID);
                _selectedProperty = _propertyList.FirstOrDefault(prp => prp.ID == _methodInstance?.PropertyID);
                _subMethodList = _methodInstance?.SubMethods;

                RaisePropertyChanged("Duration");
                RaisePropertyChanged("FileList");
                RaisePropertyChanged("IsMoreThanOneVariant");
                RaisePropertyChanged("Measurements");
                RaisePropertyChanged("MethodVariantList");
                RaisePropertyChanged("Name");
                RaisePropertyChanged("Oem");
                RaisePropertyChanged("Property");
                RaisePropertyChanged("SelectedOrganization");
                RaisePropertyChanged("SelectedProperty");
                RaisePropertyChanged("SpecificationList");
                RaisePropertyChanged("ResultList");
            }
        }

        public string MethodFileRegionName
        {
            get { return RegionNames.MethodFileRegion; }
        }

        public IEnumerable<SubMethod> Measurements
        {
            get 
            {
                return _subMethodList;
            }
        }

        public ObservableCollection<MethodVariant> MethodVariantList
        {
            get => _methodVariantList;
            private set
            {
                _methodVariantList = value;
                RaisePropertyChanged("MethodVariantList");

                // Set Event Handler to Manage added or removed Variants

                if (_methodVariantList != null)
                    _methodVariantList.CollectionChanged += OnMethodVariantCollectionChanged;
            }
        }



        public string Name
        {
            get => _methodInstance?.Standard.Name;
        }

        public IEnumerable<Organization> OrganizationList
        {
            get { return _organizationList; }
        }

        public IEnumerable<Property> PropertyList
        {
            get { return _propertyList; }
        }

        public IEnumerable<Test> ResultList
        {
            get
            {
                return _methodInstance?.GetTests();
            }
        }

        public StandardFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OpenFileCommand.RaiseCanExecuteChanged();
                RemoveFileCommand.RaiseCanExecuteChanged();
            }
        }

        public Organization SelectedOrganization
        {
            get
            {
                return _selectedOrganization;
            }
            set
            {
                _selectedOrganization = value;

                RaisePropertyChanged("SelectedOrganization");
            }
        }

        public Property SelectedProperty
        {
            get
            {
                return _selectedProperty;
            }
            set
            {
                _selectedProperty = value;
                _methodInstance.PropertyID = _selectedProperty.ID;
                RaisePropertyChanged("SelectedProeprty");
            }
        }

        public Specification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;
            }
        }

        public Test SelectedTest
        {
            get { return _selectedTest; }
            set
            {
                _selectedTest = value;

                RaisePropertyChanged("SelectedTest");
                OpenReportCommand.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<Specification> SpecificationList
        {
            get
            {
                return _methodInstance.GetSpecifications();
            }
        }

        public DelegateCommand UpdateCommand { get; }

        #endregion
    }
}
