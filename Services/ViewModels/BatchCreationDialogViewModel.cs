using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services.ViewModels
{
    public class BatchCreationDialogViewModel : BindableBase
    {
        private Aspect _aspectInstance;
        private Batch _batchInstance;
        private Colour _selectedColour;
        private DelegateCommand<Window> _cancel, _confirm;
        private MaterialLine _lineInstance;
        private MaterialType _typeInstance;
        private Project _selectedProject;
        private Recipe _recipeInstance;
        private string _aspectCode,
                        _lineCode,
                        _recipeCode,
                        _trialScope,
                        _typeCode;
        private TrialArea _selectedTrialArea;

        public BatchCreationDialogViewModel() : base()
        {
            _cancel = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = true;
                });
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

    }
}
