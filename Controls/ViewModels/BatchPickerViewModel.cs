using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.ViewModels
{
    internal class BatchPickerViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private string _number;
        private Views.BatchPickerDialog _parentDialog;
        
        internal BatchPickerViewModel(DBEntities entities,
                                    Views.BatchPickerDialog parentDialog) : base()
        {
            _entities = entities;
            _parentDialog = parentDialog;
            
            _cancel = new DelegateCommand(
                () => 
                {                    
                    _parentDialog.DialogResult = false;
                });
                
            _confirm = new DelegateCommand(
                () => 
                {
                    Batch output = GetBatch(_number);
                    if (output != null)
                    {
                        _parentDialog.BatchInstance = output;
                        _parentDialog.DialogResult = true;                        
                    }
                    else 
                        _parentDialog.DialogResult = false;
                });
        }
        
        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }
        
        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }
        
        public string Number
        {
            get { return _number; }
            set 
            {
                _number = value;
            }
        }        
        
        public Batch GetBatch(string batchNumber)
        {
            Batch temp = _entities.GetBatchByNumber(batchNumber);

            if (temp.Material == null)
                temp.Material = CreateNewMaterial();

            if (temp.Material != null)
            {
                if (temp.Material.Construction.Project == null)
                {
                    Views.ProjectPickerDialog prjDialog = new Views.ProjectPickerDialog(_entities);
                    if (prjDialog.ShowDialog() == true)
                        temp.Material.Construction.Project = prjDialog.ProjectInstance;
                }
                if (temp.Material.Recipe.Colour == null)
                {
                    Views.ColorPickerDialog colourPicker = new Views.ColorPickerDialog(_entities);
                    if (colourPicker.ShowDialog() == true)
                        temp.Material.Recipe.Colour = colourPicker.ColourInstance;
                }
            }
            
            return temp;
        }

        public Material CreateNewMaterial()
        {
            Views.MaterialCreationDialog matDialog = new Views.MaterialCreationDialog(_entities);
            if (matDialog.ShowDialog() == true)
                return matDialog.ValidatedMaterial;

            else
                return null;
        }
    }
}
