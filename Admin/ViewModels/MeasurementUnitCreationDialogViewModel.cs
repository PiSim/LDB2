using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Admin.ViewModels
{
    [Obsolete]
    public class MeasurementUnitCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private bool _canModifyQuantity;

        private string _name,
                        _symbol;

        private MeasurableQuantity _selectedMeasurableQuantity;

        #endregion Fields

        #region Constructors

        public MeasurementUnitCreationDialogViewModel()
        {
            _canModifyQuantity = false;

            CancelCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    MeasurementUnitInstance = new MeasurementUnit()
                    {
                        MeasurableQuantityID = _selectedMeasurableQuantity.ID,
                        Name = _name,
                        Symbol = _symbol
                    };

                    MeasurementUnitInstance.Create();

                    parentDialog.DialogResult = true;
                },
                parentDialog => !HasErrors);
        }

        #endregion Constructors

        #region Methods

        public void SetQuantity(MeasurableQuantity entry)
        {
            _selectedMeasurableQuantity = MeasurableQuantityList.First(meq => meq.ID == entry.ID);
            RaisePropertyChanged("SelectedMeasurableQuantity");
        }

        #endregion Methods

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _validationErrors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        #endregion INotifyDataErrorInfo interface elements

        #region Properties

        public DelegateCommand<Window> CancelCommand { get; }

        public bool CanModifyQuantity
        {
            get { return _canModifyQuantity; }
            set
            {
                _canModifyQuantity = value;
                RaisePropertyChanged("CanModifyQuantity");
            }
        }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public IEnumerable<MeasurableQuantity> MeasurableQuantityList { get; }

        public MeasurementUnit MeasurementUnitInstance { get; private set; }

        public MeasurableQuantity SelectedMeasurableQuantity
        {
            get { return _selectedMeasurableQuantity; }

            set
            {
                _selectedMeasurableQuantity = value;

                if (_selectedMeasurableQuantity != null)
                {
                    if (_validationErrors.ContainsKey("SelectedMeasurableQuantity"))
                    {
                        _validationErrors.Remove("SelectedMeasurableQuantity");
                        RaiseErrorsChanged("SelectedMeasurableQuantity");
                    }
                }
                else
                {
                    _validationErrors["SelectedMeasurableQuantity"] = new List<string>() { "Indicare una Grandezza" };
                    RaiseErrorsChanged("SelectedMeasurableQuantity");
                }
            }
        }

        public string UnitName
        {
            get { return _name; }
            set
            {
                _name = value;

                if (!string.IsNullOrEmpty(_name))
                {
                    if (_validationErrors.ContainsKey("UnitName"))
                    {
                        _validationErrors.Remove("UnitName");
                        RaiseErrorsChanged("UnitName");
                    }
                }
                else
                {
                    _validationErrors["UnitName"] = new List<string>() { "Nome non valido" };
                    RaiseErrorsChanged("UnitName");
                }
            }
        }

        public string UnitSymbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;

                if (!string.IsNullOrEmpty(_symbol))
                {
                    if (_validationErrors.ContainsKey("UnitSymbol"))
                    {
                        _validationErrors.Remove("UnitSymbol");
                        RaiseErrorsChanged("UnitSymbol");
                    }
                }
                else
                {
                    _validationErrors["UnitSymbol"] = new List<string>() { "Nome non valido" };
                    RaiseErrorsChanged("UnitSymbol");
                }
            }
        }

        #endregion Properties
    }
}