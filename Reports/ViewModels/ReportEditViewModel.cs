using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reports.ViewModels
{
    public class ReportEditViewModel : BindableBase
    {
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _addFile, _openFile, _removeFile;
        private EventAggregator _eventAggregator;
        private Report _instance;
        private List<TestWrapper> _testList;
        private ReportFile _selectedFile;
        
        public ReportEditViewModel(DBEntities entities,
                                    DBPrincipal principal,
                                    EventAggregator aggregator) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<CommitRequested>()
                .Subscribe(() =>
                {
                    if (!_testList.Any(tst => !tst.IsComplete))
                    {
                        _instance.IsComplete = true;
                        _instance.EndDate = DateTime.Now.Date;
                    }
                    _entities.SaveChanges();
                }, true);

            _addFile = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = true;
                    
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            ReportFile temp = new ReportFile();
                            temp.Path = pth;
                            temp.Description = "";
                            _instance.ReportFiles.Add(temp);
                        }

                        RaisePropertyChanged("FileList");
                    }
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
                    _instance.ReportFiles.Remove(_selectedFile);
                    RaisePropertyChanged("FileList");
                },
                () => _selectedFile != null);

        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
        }

        public string BatchNumber
        {
            get 
            { 
                if (_instance == null)
                    return null;
                else
                    return _instance.Batch.Number; 
            }
        }

        public string Category
        {
            get 
            { 
                if (_instance == null)
                    return null;
                else
                    return _instance.Category; 
            }
        }
        
        public string Description
        {
            get 
            { 
                if (_instance == null)
                    return null;
                else
                    return _instance.Description; 
            }
            set { _instance.Description = value; }
        }

        public List<ReportFile> FileList
        {
            get 
            { 
                if (_instance == null)
                    return null;
                else
                     return new List<ReportFile>(_instance.ReportFiles); 
            }
        }

        public Report Instance
        {
            get { return _instance; }
            set
            {
                _instance = _entities.Reports.FirstOrDefault(rep => rep.ID == value.ID);

                _testList = new List<TestWrapper>();
                foreach (Test tst in _instance.Tests)
                    _testList.Add(new TestWrapper(tst));

                RaisePropertyChanged("BatchNumber");
                RaisePropertyChanged("Category");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("FileList");
                RaisePropertyChanged("Material");
                RaisePropertyChanged("Number");
                RaisePropertyChanged("Project");
                RaisePropertyChanged("Specification");
                RaisePropertyChanged("SpecificationVersion");
                RaisePropertyChanged("TestList");
            }
        }

        public Material Material
        {
            get 
            { 
                if (_instance == null)
                    return null;
                else
                     return _instance.Batch.Material; 
            }
        }

        public string Number
        {
            get 
            { 
                if (_instance == null)
                    return null;
                else
                      return _instance.Number.ToString(); 
            }
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public string Project
        {
            get
            {
                try
                {
                    return _instance.Batch.Material.Construction.Project.Name;
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }

        public string Specification
        {
            get 
            { 
                if (_instance == null)
                    return null;
                else
                     return _instance.SpecificationVersion.Specification.Standard.Organization.Name + " " + 
                            _instance.SpecificationVersion.Specification.Standard.Name; 
            }
        }

        public string SpecificationVersion
        {
            get 
            { 
                if (_instance == null)
                    return null;
                else
                     return _instance.SpecificationVersion.Name; 
            }
        }

        public ReportFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                _openFile.RaiseCanExecuteChanged();
                _removeFile.RaiseCanExecuteChanged();

                RaisePropertyChanged("SelectedFile");
            }
        } 

        public List<TestWrapper> TestList
        {
            get { return _testList; }
        }
    }
}
