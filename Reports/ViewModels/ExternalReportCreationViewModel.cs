using DBManager;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.ViewModels
{
    internal class ExternalReportCreationViewModel
    {
        private Batch _selectedBatch;
        private DBEntities _entities;
        private DelegateCommand _addBatch, _cancel, _confirm, _removeBatch;
        private Int32 _number;
        private Project _selectedProject;
        private string _sampleDescription, _testDescription;
        private Views.ExternalReportCreationDialog _parentDialog;
        
        internal ExternalReportCreationViewModel(DBEntities entities,
                                                Views.ExternalReportCreationDialog parentDialog)
        {
            
            _entities = entities;
            _parentDialog = parentDialog;
            
            _addBatch = new DelegateCommand(
                () => 
                {
                        
                });
            
            _cancel = new DelegateCommand(
                () => 
                {
                    
                });
                
           _confirm = new DelegateCommand(
                () => 
                {
                    
                });
                
            _removeBatch = new DelegateCommand(
                () => 
                {
                    
                },
                () => _selectedBatch != null);
        }
        
        
        public DelegateCommand AddBatchCommand
        {
            get { return _addBatch; }
        }
        
        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }
        
        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }
        
        public DelegateCommand RemoveBatchCommand
        {
            get { return _removeBatch; }
        }
        
        public Int32 Number 
        {
            get { return _number; }
            set { _number}
        }
        
        public string SampleDescription
        {
            get { return _sampleDescription; }
            set 
            {
                _sampleDescription = value;
            }
        }
        
        public Batch SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;
                RemoveBatchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedBatch");
            }
        }
        
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
            }
        }
        
        public string TestDescription
        {
            get { return _testDescription; }
            set 
            {
                _testDescription = value;    
            }
        }
    }
}
