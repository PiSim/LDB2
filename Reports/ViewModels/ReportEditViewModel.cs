using DBManager;
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
    internal class ReportEditViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _addFile, _openFile, _removeFile;
        private EventAggregator _eventAggregator;
        Report _instance;
        List<TestWrapper> _testList;
        ReportFile _selectedFile;
        
        public ReportEditViewModel(DBEntities entities,
                                    EventAggregator aggregator,
                                    Report target) : base()
        {
            if (!(target is Report))
                throw new InvalidOperationException("Not a Report Object");

            _entities = entities;
            _eventAggregator = aggregator;
            _instance = _entities.Reports.FirstOrDefault(rep => rep.ID == target.ID);

            _testList = new List<TestWrapper>();
            foreach (Test tst in _instance.Tests)
                _testList.Add(new TestWrapper(tst));

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

                        OnPropertyChanged("FileList");
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
                },
                () => _selectedFile != null);

        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
        }

        public string BatchNumber
        {
            get { return _instance.Batch.Number; }
        }

        public string Category
        {
            get { return _instance.Category; }
            set { _instance.Category = value; }
        }
        
        public string Description
        {
            get { return _instance.Description; }
            set { _instance.Description = value; }
        }

        public List<ReportFile> FileList
        {
            get { return new List<ReportFile>(_instance.ReportFiles); }
        }

        public Report Instance
        {
            get { return _instance; }
        }

        public Material Material
        {
            get { return _instance.Batch.Material; }
        }

        public string Number
        {
            get { return _instance.Number.ToString(); }
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
            get { return _instance.SpecificationVersion.Specification.Standard.Organization.Name + " " + 
                    _instance.SpecificationVersion.Specification.Standard.Name; }
        }

        public string SpecificationVersion
        {
            get { return _instance.SpecificationVersion.Name; }
        }

        public ReportFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                _openFile.RaiseCanExecuteChanged();
                _removeFile.RaiseCanExecuteChanged();
            }
        } 

        public List<TestWrapper> TestList
        {
            get { return _testList; }
        }
    }
}
