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
    internal class RequirementWrapper
    {
        private bool _isSelected;
        private Requirement _instance;
        
        internal RequirementWrapper(Requirement instance)
        {
            _instance = instance;
            _isSelected = true;
        }
        
        public string Method
        {
            get { return _instance.Method.Standard.Organization.Name + " " + _instance.Method.Standard.Name; }
        }
                
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }
        
        public string Property
        {
            get { return _instance.Method.Property.Name; }
        }
    }
    
    internal class ReportCreationViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private Int32 _number;
        private ObservableCollection<RequirementWrapper> _requirementList;
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
                    
                    foreach (RequirementWrapper rw in _requirementList)
                    {
                        if (!rw.IsSelected)
                            continue;
                            
                        Test tt = new Test();
                        
                        tt.Batch = temp.Batch;
                        // tt.Method = rr.Method;
                        tt.Operator = temp.Author;
                        
                        temp.Tests.Add(tt);
                    }
                    
                    _parentDialog.ReportInstance = temp;
                    _parentDialog.DialogResult = true;
                },
                () => IsValidInput);
                
            _cancel = new DelegateCommand(
                () => {
                    _parentDialog.DialogResult = false;    
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
        
        public Int32 Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public Specification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set { _selectedSpecification = value; }
        }

        public SpecificationVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set { _selectedVersion = value; }
        }

        public ObservableCollection<RequirementWrapper> RequirementList
        {
            get { return _requirementList; }
            set { _requirementList = value; }
        }
    }
}