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

        internal ReportEditViewModel(Report target) : base()
        {
            _instance = target;
            _testList = new List<Test>(_instance.Tests);
            _fileList = new List<ReportFile>(_instance.ReportFiles);
        }

        internal Batch Batch
        {
            get { return _instance.Batch; }
        }

        internal List<ReportFile> FileList
        {
            get { return _fileList; }
        }

        internal Report Instance
        {
            get { return _instance; }
        }

        internal string Number
        {
            get { return _instance.Number.ToString(); }
        }

        internal ReportFile SelectedFile
        {
            get { return _selectedFile; }
            set { _selectedFile = value; }
        } 

        internal List<Test> TestList
        {
            get { return _testList; }
        }
    }
}
