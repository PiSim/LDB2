using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.ViewModels
{
    internal class ReportEditViewModel : BindableBase
    {
        DelegateCommand _addFile, _openFile, _removeFile;
        Report _instance;
        List<ReportFile> _fileList;
        List<Test> _testList;
        ReportFile _selectedFile;

        public ReportEditViewModel(Report target) : base()
        {
            _instance = target;
            _testList = new List<Test>(_instance.Tests);
            _fileList = new List<ReportFile>(_instance.ReportFiles);
        }

        public string BatchNumber
        {
            get { return _instance.Batch.Number; }
        }

        public string Category
        {
            get { return _instance.Category; }
        }

        public List<ReportFile> FileList
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

        public string Project
        {
            get { return _instance.Batch.Material.Construction.Project.Name; }
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
            set { _selectedFile = value; }
        } 

        public List<Test> TestList
        {
            get { return _testList; }
        }
    }
}
