using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments.ViewModels
{
    public class InstrumentMeasurablePropertyEditViewModel : BindableBase
    {
        private bool _editMode;
        private DelegateCommand _save,
                                _startEdit;
        private InstrumentMeasurableProperty _measurablePropertyInstance;


        public InstrumentMeasurablePropertyEditViewModel()
        {
            _editMode = false;

            _save = new DelegateCommand(
                () =>
                {
                    EditMode = false;
                },
                () => EditMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !EditMode);
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

        public InstrumentMeasurableProperty MeasurablePropertyInstance
        {
            get { return _measurablePropertyInstance; }
            set
            {
                _measurablePropertyInstance = value;
                EditMode = false;
            }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }
    }
}
