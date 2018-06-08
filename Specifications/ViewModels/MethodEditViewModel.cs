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

namespace Specifications.ViewModels
{
    public class MethodEditViewModel : BindableBase
    {
        private bool _editMode;
        private DBPrincipal _principal;
        private DelegateCommand _addFile,
                                _addMeasurement,
                                _openFile,
                                _openReport,
                                _openSpecification,
                                _removeFile,
                                _removeMeasurement,
                                _save,
                                _startEdit;
        private EventAggregator _eventAggregator;
        private IDataService _dataService;
        private IEnumerable<Organization> _organizationList;
        private IEnumerable<Property> _propertyList;
        private IEnumerable<SubMethod> _subMethodList;
        private ISpecificationService _specificationService;
        private Method _methodInstance;
        private Organization _selectedOrganization;
        private Property _selectedProperty;
        private Specification _selectedSpecification;
        private SubMethod _selectedMeasurement;
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

            _addFile = new DelegateCommand(
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
                () => IsSpecAdmin);

            _addMeasurement = new DelegateCommand(
                () =>
                {
                    SubMethod tempMea = new SubMethod
                    {
                        Name = "Nuova Prova",
                        UM = ""
                    };

                    _methodInstance.AddSubMethod(tempMea);

                    _subMethodList = _methodInstance.GetSubMethods();
                    RaisePropertyChanged("Measurements");
                },
                () => IsSpecAdmin);

            _openFile = new DelegateCommand(
                () =>
                {
                    System.Diagnostics.Process.Start(_selectedFile.Path);
                },
                () => _selectedFile != null);

            _openReport = new DelegateCommand(
                () =>
                {
                NavigationToken token = new NavigationToken((_selectedTest.TestRecord.RecordTypeID == 1) ? ReportViewNames.ReportEditView : ReportViewNames.ExternalReportEditView,
                                                            _selectedTest.TestRecord.Reports.First());

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                },
                () => _selectedTest != null);
            
            _openSpecification = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationEdit,
                                                                SelectedSpecification);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedSpecification != null);

            _removeFile = new DelegateCommand(
                () =>
                {
                    _selectedFile.Delete();
                    SelectedFile = null;
                },
                () => IsSpecAdmin && _selectedFile != null);

            _removeMeasurement = new DelegateCommand(
                () =>
                {
                    _selectedMeasurement.Delete();
                    SelectedMeasurement = null;

                    _subMethodList = _methodInstance.GetSubMethods();
                    RaisePropertyChanged("Measurements");
                },
                () => IsSpecAdmin && SelectedMeasurement != null);

            _save = new DelegateCommand(
                () =>
                {
                    _methodInstance.Update();
                    _specificationService.UpdateSubMethods(_subMethodList);

                    if (_selectedOrganization.ID != _methodInstance.Standard.OrganizationID)
                    {
                        _methodInstance.Standard.OrganizationID = _selectedOrganization.ID;
                        _methodInstance.Standard.Update();
                    }

                    EditMode = false;
                },
                () => _editMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => IsSpecAdmin && !_editMode);
        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
        }

        public DelegateCommand AddMeasurementCommand
        {
            get { return _addMeasurement; }
        }

        public bool CanModifyTests
        {
            get { return !EditMode; }
        }

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
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<StandardFile> FileList
        {
            get
            {
                return _methodInstance.GetFiles();
            }
        }

        private bool IsSpecAdmin
        {
            get { return _principal.IsInRole(UserRoleNames.SpecificationAdmin); }
        }

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
                _methodInstance.Load();

                SelectedOrganization = _organizationList.FirstOrDefault(org => org.ID == _methodInstance?.Standard.OrganizationID);
                _selectedProperty = _propertyList.FirstOrDefault(prp => prp.ID == _methodInstance?.PropertyID);
                _subMethodList = _methodInstance.GetSubMethods();

                RaisePropertyChanged("Duration");
                RaisePropertyChanged("FileList");
                RaisePropertyChanged("IssueList");
                RaisePropertyChanged("Measurements");
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

        public string Name
        {
            get => _methodInstance?.Standard.Name;
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public DelegateCommand OpenReportCommand
        {
            get { return _openReport; }
        }

        public DelegateCommand OpenSpecificationCommand
        {
            get { return _openSpecification; }
        }

        public IEnumerable<Organization> OrganizationList
        {
            get { return _organizationList; }
        }

        public IEnumerable<Property> PropertyList
        {
            get { return _propertyList; }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }
        
        public DelegateCommand RemoveMeasurementCommand
        {
            get { return _removeMeasurement; }
        }

        public IEnumerable<Test> ResultList
        {
            get
            {
                return _methodInstance.GetTests(); }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public StandardFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                _openFile.RaiseCanExecuteChanged();
                _removeFile.RaiseCanExecuteChanged();
            }
        }

        public SubMethod SelectedMeasurement
        {
            get 
            {
                return _selectedMeasurement;
            }
            set
            {
                _selectedMeasurement = value;
                _removeMeasurement.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedMeasurement");
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
                _openReport.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<Specification> SpecificationList
        {
            get
            {
                return _methodInstance.GetSpecifications();
            }
        }
        
        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }
    }
}
