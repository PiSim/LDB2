﻿using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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
        List<ReportFile> _fileList;
        List<Test> _testList;
        ReportFile _selectedFile;
        
        public ReportEditViewModel(Report target) : base()
        {
            _addFile = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ReportFile temp = new ReportFile();
                        temp.Path = fileDialog.FileName;
                        _instance.ReportFiles.Add(temp);
                    }
                });

            _fileList = new List<ReportFile>(_instance.ReportFiles);
            _instance = target;

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

            _testList = new List<Test>(_instance.Tests);
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
            set { _selectedFile = value; }
        } 

        public List<Test> TestList
        {
            get { return _testList; }
        }
    }
}
