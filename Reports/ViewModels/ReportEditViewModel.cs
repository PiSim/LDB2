using DBManager;
using Prism.Commands;
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
        DelegateCommand _addFile, _openFile, _removeFile;
        Report _instance;
        ObservableCollection<ReportFile> _fileList;
        List<Test> _testList;
        ReportFile _selectedFile;
        
        public ReportEditViewModel(Report target) : base()
        {

            _instance = target;
            _fileList = new ObservableCollection<ReportFile>(_instance.ReportFiles);
            _testList = new List<Test>(_instance.Tests);

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
                            _instance.ReportFiles.Add(temp);   
                        }
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
        }

        public ObservableCollection<ReportFile> FileList
        {
            get { return _fileList; }
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

        public List<Test> TestList
        {
            get { return _testList; }
        }
    }
}
