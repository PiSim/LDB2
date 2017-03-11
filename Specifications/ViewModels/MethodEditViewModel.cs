using DBManager;
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
 
namespace Specifications.ViewModels
{
    public class MethodEditViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _addFile, _addIssue, _addMeasurement, _openFile, _removeFile, _removeIssue, _removeMeasurement, _setCurrent;
        private EventAggregator _eventAggregator;
        private Method _methodInstance;
        private SubMethod _selectedMeasurement;
        private ObservableCollection<SubMethod>_measurementList;
        private ObservableCollection<StandardIssue> _issueList;
        private StandardIssue _selectedIssue;
        private StandardFile _selectedFile;

        public MethodEditViewModel(DBEntities entities, EventAggregator aggregator) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(() => _entities.SaveChanges());

            _addFile = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = true;

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            StandardFile temp = new StandardFile();
                            temp.Path = pth;
                            temp.Description = "";
                            _selectedIssue.StandardFiles.Add(temp);
                        }

                        OnPropertyChanged("FileList");
                    }
                },
                () => SelectedIssue != null);

            _addIssue = new DelegateCommand(
                () =>
                {
                    StandardIssue temp = new StandardIssue();
                    temp.IsCurrent = false;
                    temp.Issue = DateTime.Now.ToShortDateString();
                    temp.Standard = _methodInstance.Standard;

                    _methodInstance.Standard.StandardIssues.Add(temp);

                    _issueList.Add(temp);

                    SelectedIssue = temp;
                });

            _addMeasurement = new DelegateCommand(
                () => 
                {
                    SubMethod tempMea = new SubMethod();
                    tempMea.Name = "Nuova Prova";
                    tempMea.UM = "";

                    foreach (Requirement req in MethodInstance.Requirements)
                    {
                        SubRequirement tempSR = new SubRequirement();
                        tempSR.RequiredValue = "";
                        tempMea.SubRequirements.Add(tempSR);
                        req.SubRequirements.Add(tempSR);
                    }
                    
                    Measurements.Add(tempMea);                   
                    _methodInstance.SubMethods.Add(tempMea);
                });

            _openFile = new DelegateCommand(
                () =>
                {
                    System.Diagnostics.Process.Start(_selectedFile.Path);
                },
                () => _selectedFile != null);

            _removeFile = new DelegateCommand(
                () =>
                {
                    _selectedIssue.StandardFiles.Remove(_selectedFile);
                    SelectedFile = null;
                },
                () => _selectedFile != null);

            _removeIssue = new DelegateCommand(
                () =>
                {
                    _methodInstance.Standard.StandardIssues.Remove(_selectedIssue);
                    _issueList.Remove(_selectedIssue);
                    SelectedIssue = null;
                },
                () => _selectedIssue != null);

            _removeMeasurement = new DelegateCommand(
                () => 
                {
                    if (SelectedMeasurement.SubRequirements.Any())
                        throw new NotImplementedException();

                    Measurements.Remove(SelectedMeasurement);
                    SelectedMeasurement = null;
                },
                () => SelectedMeasurement != null );

            _setCurrent = new DelegateCommand(
                () =>
                {
                    _methodInstance.Standard.CurrentIssue.IsCurrent = false;
                    _selectedIssue.IsCurrent = true;
                    _methodInstance.Standard.CurrentIssue = _selectedIssue;
                },
                () => _selectedIssue != null
                );
        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
        }
        
        public DelegateCommand AddIssueCommand
        {
            get { return _addIssue; }
        }
        
        public DelegateCommand AddMeasurementCommand
        {
            get { return _addMeasurement; }
        }

        public List<StandardFile> FileList
        {
            get { return new List<StandardFile>(_selectedIssue.StandardFiles); }
        }

        public ObservableCollection<StandardIssue> IssueList
        {
            get
            {
                return _issueList;
            }

            private set
            {
                _issueList = value;
                OnPropertyChanged("IssueList");
            }
        }

        public Method MethodInstance
        {
            get { return _methodInstance; }
            set
            {
                _methodInstance = value;
                Measurements = new ObservableCollection<SubMethod>(_methodInstance.SubMethods);
                IssueList = new ObservableCollection<StandardIssue>(_methodInstance.Standard.StandardIssues);
                OnPropertyChanged("Name");
                OnPropertyChanged("Oem");
                OnPropertyChanged("Property");
                OnPropertyChanged("SpecificationList");
                OnPropertyChanged("ResultList");
            }
        }

        public ObservableCollection<SubMethod> Measurements
        {
            get 
            { 
                return _measurementList;
            }
            
            private set 
            {
                _measurementList = value;
                OnPropertyChanged("Measurements");
            }
        }

        public string Name
        {
            get
            {
                return (_methodInstance != null) ? _methodInstance.Standard.Name : null;
            }
        }

        public string Oem
        {
            get
            {
                return (_methodInstance != null) ? _methodInstance.Standard.Organization.Name : null;
            }
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public string Property
        {
            get
            {
                return (_methodInstance != null) ? _methodInstance.Property.Name : null;
            }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }
        
        public DelegateCommand RemoveMeasurementCommand
        {
            get { return _removeMeasurement; }
        }

        public List<Test> ResultList
        {
            get
            {
                if (_methodInstance == null)
                    return null;
                else
                    return new List<Test>(_methodInstance.Tests); }
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

        public StandardIssue SelectedIssue
        {
            get { return _selectedIssue; }
            set
            {
                _selectedIssue = value;
                OnPropertyChanged("FileList");
                _removeIssue.RaiseCanExecuteChanged();
                _setCurrent.RaiseCanExecuteChanged();
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
                OnPropertyChanged("SelectedMeasurement");
            }
        }

        public DelegateCommand SetCurrentCommand
        {
            get { return _setCurrent; }
        }

        public List<Specification> SpecificationList
        {
            get
            {
                if (_methodInstance == null)
                    return null;
                else
                    return new List<Specification>(_methodInstance.Requirements
                        .Select(req => (req.SpecificationVersions !=  null) 
                            ? req.SpecificationVersions.Specification : null));
            }
        }
        
    }
}
