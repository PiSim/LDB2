using DBManager;
using DBManager.EntityExtensions;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Specifications.ViewModels
{
    public class StandardIssueEditViewModel : BindableBase
    {
        private bool _editMode;
        private DelegateCommand _addFile,
                                _openFile,
                                _removeFile,
                                _save,
                                _setCurrent,
                                _startEdit;
        private EventAggregator _eventAggregator;
        private StandardFile _selectedFile;
        private StandardIssue _standardIssueInstance;

        public StandardIssueEditViewModel(EventAggregator eventAggregator)
        {
            _editMode = false;
            _eventAggregator = eventAggregator;


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
                            temp.IssueID = _standardIssueInstance.ID;

                            temp.Create();
                        }

                        RaisePropertyChanged("FileList");
                    }
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

            _save = new DelegateCommand(
                () =>
                {
                    _standardIssueInstance.Update();
                    EditMode = false;
                },
                () => _editMode);
            
            _setCurrent = new DelegateCommand(
                () =>
                {
                    _standardIssueInstance.Standard.SetCurrentIssue(_standardIssueInstance);
                    RaisePropertyChanged("IsCurrent");
                },
                () => !_standardIssueInstance.IsCurrent);

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
                return _standardIssueInstance.GetIssueFiles();
            }
        }

        public string Issue
        {
            get
            {
                if (_standardIssueInstance == null)
                    return null;

                return _standardIssueInstance.Issue;
            }

            set
            {
                _standardIssueInstance.Issue = value;
            }
        }

        public bool IsCurrent
        {
            get
            {
                if (_standardIssueInstance == null)
                    return false;

                return _standardIssueInstance.IsCurrent;
            }
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
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
                RaisePropertyChanged("SelectedFile");
                OpenFileCommand.RaiseCanExecuteChanged();
                RemoveFileCommand.RaiseCanExecuteChanged();
            }
        }
        
        public DelegateCommand SetCurrentCommand
        {
            get { return _setCurrent; }
        }

        public Visibility StandardIssueEditViewVisibility
        {
            get
            {
                return (_standardIssueInstance != null) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public StandardIssue StandardIssueInstance
        {
            get { return _standardIssueInstance; }

            set
            {
                _standardIssueInstance = value;
                _standardIssueInstance.Load();

                RaisePropertyChanged("FileList");
                RaisePropertyChanged("Issue");
                RaisePropertyChanged("IsCurrent");
                RaisePropertyChanged("StandardIssueEditViewVisibility");
            }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }
    }
}
