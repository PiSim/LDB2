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
        private DelegateCommand _addFile,
                                _addIssue,
                                _addMeasurement,
                                _openFile,
                                _removeFile,
                                _removeIssue,
                                _removeMeasurement,
                                _save,
                                _setCurrent,
                                _startEdit;
        private EventAggregator _eventAggregator;
        private IEnumerable<SubMethod> _subMethodList;
        private Method _methodInstance;
        private SubMethod _selectedMeasurement;
        private StandardIssue _selectedIssue;
        private StandardFile _selectedFile;

        public MethodEditViewModel(EventAggregator aggregator) : base()
        {
            _editMode = false;
            _eventAggregator = aggregator;
            _subMethodList = new List<SubMethod>();

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
                            temp.IssueID = _selectedIssue.ID;

                            temp.Create();
                        }

                        RaisePropertyChanged("FileList");
                    }
                },
                () => SelectedIssue != null);

            _addIssue = new DelegateCommand(
                () =>
                {
                    StandardIssue temp = new StandardIssue();
                    temp.IsCurrent = false;
                    temp.Issue = DateTime.Now.ToShortDateString();
                    temp.StandardID = _methodInstance.Standard.ID;

                    temp.Create();

                    RaisePropertyChanged("IssueList");
                });

            _addMeasurement = new DelegateCommand(
                () =>
                {
                    SubMethod tempMea = new SubMethod();
                    tempMea.Name = "Nuova Prova";
                    tempMea.UM = "";

                    _methodInstance.AddSubMethod(tempMea);

                    _subMethodList = _methodInstance.GetSubMethods();
                    RaisePropertyChanged("Measurements");
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
                    _selectedFile.Delete();
                    SelectedFile = null;
                },
                () => _selectedFile != null);

            _removeIssue = new DelegateCommand(
                () =>
                {
                    _methodInstance.Standard.StandardIssues.Remove(_selectedIssue);
                    RaisePropertyChanged("IssueList");
                    SelectedIssue = null;
                },
                () => _selectedIssue != null);

            _removeMeasurement = new DelegateCommand(
                () =>
                {
                    _selectedMeasurement.Delete();
                    SelectedMeasurement = null;

                    _subMethodList = _methodInstance.GetSubMethods();
                    RaisePropertyChanged("Measurements");
                },
                () => SelectedMeasurement != null);

            _save = new DelegateCommand(
                () =>
                {
                    _methodInstance.Update();
                    SpecificationService.UpdateSubMethods(_subMethodList);
                    EditMode = false;
                },
                () => _editMode);

            _setCurrent = new DelegateCommand(
                () =>
                {
                    _methodInstance.Standard.SetCurrentIssue(_selectedIssue);
                },
                () => _selectedIssue != null
                );

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !_editMode);
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

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<StandardFile> FileList
        {
            get
            {
                return _selectedIssue.GetIssueFiles();
            }
        }

        public IEnumerable<StandardIssue> IssueList
        {
            get
            {
                return _methodInstance.GetIssues();
            }
        }

        public Method MethodInstance
        {
            get { return _methodInstance; }
            set
            {
                EditMode = false;

                _methodInstance = value;
                _methodInstance.Load();

                _subMethodList = _methodInstance.GetSubMethods();

                RaisePropertyChanged("IssueList");
                RaisePropertyChanged("Measurements");
                RaisePropertyChanged("Name");
                RaisePropertyChanged("Oem");
                RaisePropertyChanged("Property");
                RaisePropertyChanged("SpecificationList");
                RaisePropertyChanged("ResultList");
            }
        }

        public string MethodIssueEditRegionName
        {
            get { return RegionNames.MethodIssueEditRegion; }
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

        public IEnumerable<Test> ResultList
        {
            get
            {
                if (_methodInstance == null)
                    return null;
                else
                    return _methodInstance.Tests; }
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

        public StandardIssue SelectedIssue
        {
            get { return _selectedIssue; }
            set
            {
                _selectedIssue = value;
                RaisePropertyChanged("FileList");
                _addFile.RaiseCanExecuteChanged();
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
                RaisePropertyChanged("SelectedMeasurement");
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
        
        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }
    }
}
