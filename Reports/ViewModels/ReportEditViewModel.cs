﻿using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Services;
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
        private DBPrincipal _principal;
        private DelegateCommand _addFile, _addTests, _generateRawDataSheet, _openFile, _removeFile;
        private DelegateCommand<Test> _removeTest;
        private EventAggregator _eventAggregator;
        private Report _instance;
        private IEnumerable<TestWrapper> _testList;
        private ReportFile _selectedFile;
        
        public ReportEditViewModel(DBPrincipal principal,
                                    EventAggregator aggregator) : base()
        {
            _eventAggregator = aggregator;
            _principal = principal;

            _eventAggregator.GetEvent<CommitRequested>()
                .Subscribe(() =>
                {
                    _testList.Select(tiw => tiw.TestInstance)
                            .Update();
                    _eventAggregator.GetEvent<ReportStatusCheckRequested>().Publish(_instance);
                });

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
                            temp.reportID = _instance.ID;
                            temp.Create();
                        }

                        RaisePropertyChanged("FileList");
                    }
                });

            _addTests = new DelegateCommand(
                () =>
                {
                    if (CommonProcedures.AddTestsToReport(_instance))
                    {
                        TestList = new List<TestWrapper>(_instance.GetTests().Select(tst => new TestWrapper(tst)));
                        _eventAggregator.GetEvent<ReportStatusCheckRequested>()
                                        .Publish(_instance);
                    }
                    
                });

            _generateRawDataSheet = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<GenerateReportDataSheetRequested>().Publish(_instance);
                });

            _openFile = new DelegateCommand(
                () =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start(_selectedFile.Path);
                    }
                    catch (Exception)
                    {
                        _eventAggregator.GetEvent<StatusNotificationIssued>().Publish("File non trovato");
                    }
                },
                () => _selectedFile != null);

            _removeFile = new DelegateCommand(
                () =>
                {
                    _selectedFile.Delete();
                    SelectedFile = null;

                    RaisePropertyChanged("FileList");
                },
                () => _selectedFile != null);

            _removeTest = new DelegateCommand<Test>(
                testItem =>
                {
                    testItem.Load();
                    TaskItem tempTaskItem = testItem.ParentTaskItem;

                    testItem.Delete();

                    if (tempTaskItem != null)
                    {
                        tempTaskItem.IsAssignedToReport = false;
                        tempTaskItem.Update();
                        _eventAggregator.GetEvent<TaskStatusCheckRequested>()
                                        .Publish(tempTaskItem.Task);
                    }

                    _eventAggregator.GetEvent<ReportStatusCheckRequested>()
                                    .Publish(_instance);

                    TestList = new List<TestWrapper>(_instance.GetTests().Select(tst => new TestWrapper(tst)));

                });

        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
        }

        public DelegateCommand AddTestsCommand
        {
            get { return _addTests; }
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

        public bool CanModify
        {
            get 
            {
                if (_instance == null)
                    return false;

                else if (_instance.Author.ID == _principal.CurrentPerson.ID && _principal.IsInRole(UserRoleNames.ReportEdit))
                    return true;
                
                else if (_principal.IsInRole(UserRoleNames.ReportAdmin))
                    return true;
                
                else 
                    return false;
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

        public IEnumerable<ReportFile> FileList
        {
            get 
            {
                return _instance.GetReportFiles();
            }
        }

        public DelegateCommand GenerateRawDataSheetCommand
        {
            get { return _generateRawDataSheet; }
        }

        public Report Instance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                _instance.Load();

                _testList = new List<TestWrapper>(_instance.GetTests().Select(tst => new TestWrapper(tst)));

                RaisePropertyChanged("BatchNumber");
                RaisePropertyChanged("CanModify");
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

        public DelegateCommand<Test> RemoveTestCommand
        {
            get { return _removeTest; }
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

        public IEnumerable<TestWrapper> TestList
        {
            get { return _testList; }
            private set
            {
                _testList = value;
                RaisePropertyChanged("TestList");
            }
        }
    }
}
