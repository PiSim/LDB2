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
    internal class ReportCreationViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private Person _author;
        private Specification _selectedSpecification;
        private SpecificationVersion _selectedVersion;
        private string _batchNumber, _category;
        private Views.ReportCreationDialog _parentDialog;
        
        public ReportCreationViewModel(Views.ReportCreationDialog parent, DBEntities entities) : base()
        {
            _entities = entities;
            _parentDialog = parent;
            
            _confirm = new DelegateCommand(
                () => {
                    Report temp = new Report();
                    temp.Author = _author;
                    temp.Batch = _entities.GetBatchByNumber(_batchNumber);
                    temp.Category = _category;
                    temp.Number = _number;
                    temp.SpecificationVersion = _selectedVersion;
                    
                    
                    _parentDialog.ReportInstance = temp;
                    _parentDialog.DialogResult = DialogResults.OK;
                },
                () => IsValidInput);
                
            _cancel = new DelegateCommand(
                () => {
                    _parentDialog.DialogResult = DialogResult.Cancel;    
                });
        }
        
        public Person Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set { _batchNumber = value; }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
        
        public bool IsValidInput
        {
            get { return true; }
        }
        
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public string SelectedSpecification
        {
            get { return _selectedSpecification; }
            set { _selectedSpecification = value; }
        }

        public string SelectedVersion
        {
            get { return _selectedVersion; }
            set { _selectedVersion = value; }
        }

        public List<Test> TestList
        {
            get { return _testList; }
            set { _testList = value; }
        }
    }
}