using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Specifications.ViewModels
{
    public class MethodEditViewModel : BindableBase
    {
        #region Fields

        private bool _editMode;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private Method _methodInstance;
        private ObservableCollection<MethodVariant> _methodVariantList;
        private StandardFile _selectedFile;
        private Organization _selectedOrganization;
        private Property _selectedProperty;
        private Test _selectedTest;
        private ISpecificationService _specificationService;

        #endregion Fields

        #region Constructors

        public MethodEditViewModel(IEventAggregator aggregator,
                                    IDataService<LabDbEntities> labDbData,
                                    ISpecificationService specificationService) : base()
        {
            _labDbData = labDbData;
            _editMode = false;
            _eventAggregator = aggregator;
            _specificationService = specificationService;

            OrganizationList = _labDbData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.StandardPublisher })
                                                                        .ToList(); ;
            PropertyList = _labDbData.RunQuery(new PropertiesQuery()).ToList();

            Measurements = new List<SubMethod>();

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

                            _labDbData.Execute(new InsertEntityCommand(temp));
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

            OpenReportCommand = new DelegateCommand<Test>(
                tst =>
                {
                    NavigationToken token = new NavigationToken((tst.TestRecord.RecordTypeID == 1) ? ReportViewNames.ReportEditView : ReportViewNames.ExternalReportEditView,
                                                                tst.TestRecord.Reports.First());

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                });

            OpenSpecificationCommand = new DelegateCommand<Specification>(
                spec =>
                {
                    NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationEdit,
                                                                spec);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                });

            RemoveFileCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new DeleteEntityCommand(_selectedFile));
                    SelectedFile = null;
                },
                () => EditMode && _selectedFile != null);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new UpdateEntityCommand(_methodInstance));

                    _specificationService.UpdateSubMethods(Measurements);
                    _specificationService.UpdateMethodVariantRange(_methodVariantList);

                    if (_selectedOrganization.ID != _methodInstance.Standard.OrganizationID)
                    {
                        _methodInstance.Standard.OrganizationID = _selectedOrganization.ID;
                        _labDbData.Execute(new UpdateEntityCommand(_methodInstance.Standard));
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

        #endregion Constructors

        #region Commands

        public DelegateCommand AddFileCommand { get; }

        public DelegateCommand CancelEditCommand { get; }

        public DelegateCommand OpenFileCommand { get; }

        public DelegateCommand<Test> OpenReportCommand { get; }

        public DelegateCommand<Specification> OpenSpecificationCommand { get; }

        public DelegateCommand RemoveFileCommand { get; }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand StartEditCommand { get; }

        #endregion Commands

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

        #endregion Methods

        #region Properties

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
                if (_methodInstance != null)
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

        public IEnumerable<StandardFile> FileList => _methodInstance.GetFiles();
        public bool IsMoreThanOneVariant => _methodVariantList?.Count != 0;
        public bool? IsOld => _methodInstance?.IsOld;
        public IEnumerable<SubMethod> Measurements { get; private set; }
        public string MethodEditSpecificationListRegionName => RegionNames.MethodEditSpecificationListRegion;
        public string MethodFileRegionName => RegionNames.MethodFileRegion;

        public Method MethodInstance
        {
            get { return _methodInstance; }
            set
            {
                EditMode = false;

                _methodInstance = value;

                MethodVariantList = new ObservableCollection<MethodVariant>(_methodInstance?.GetVariants());

                SelectedOrganization = OrganizationList.FirstOrDefault(org => org.ID == _methodInstance?.Standard.OrganizationID);
                _selectedProperty = PropertyList.FirstOrDefault(prp => prp.ID == _methodInstance?.PropertyID);
                Measurements = _methodInstance?.SubMethods;

                RaisePropertyChanged("Duration");
                RaisePropertyChanged("FileList");
                RaisePropertyChanged("IsMoreThanOneVariant");
                RaisePropertyChanged("IsOld");
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

        public string Name => _methodInstance?.Standard.Name;
        public IEnumerable<Organization> OrganizationList { get; }
        public IEnumerable<Property> PropertyList { get; }
        public IEnumerable<Test> ResultList => _methodInstance.Tests.ToList();

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

        public Specification SelectedSpecification { get; set; }

        public Test SelectedTest
        {
            get { return _selectedTest; }
            set
            {
                _selectedTest = value;

                RaisePropertyChanged("SelectedTest");
            }
        }

        public IEnumerable<Specification> SpecificationList => (_methodInstance == null) ? null : _methodInstance.MethodVariants.SelectMany(mtdvar => mtdvar.Requirements.Select(req => req.SpecificationVersions.Specification)).ToList();
        public DelegateCommand UpdateCommand { get; }
        private bool CanModify => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationEdit);

        #endregion Properties
    }
}