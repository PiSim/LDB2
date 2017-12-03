using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Admin.ViewModels
{
    public class MeasurementUnitCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        private bool _canModifyQuantity;
        private DelegateCommand<Window> _cancel,
                                        _confirm;

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private IEnumerable<MeasurableQuantity> _measurableQuantityList;
        private IDataService _dataService;
        private MeasurableQuantity _selectedMeasurableQuantity;
        private MeasurementUnit _measurementUnitInstance;
        private string _name,
                        _symbol;


        public MeasurementUnitCreationDialogViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _canModifyQuantity = false;
            _measurableQuantityList = _dataService.GetMeasurableQuantities();

            _cancel = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parentDialog =>
                {
                    _measurementUnitInstance = new MeasurementUnit()
                    {
                        MeasurableQuantityID = _selectedMeasurableQuantity.ID,
                        Name = _name,
                        Symbol = _symbol
                    };

                    _measurementUnitInstance.Create();

                    parentDialog.DialogResult = true;
                },
                parentDialog => !HasErrors);
        }

        #region Service Methods

        public void SetQuantity(MeasurableQuantity entry)
        {
            _selectedMeasurableQuantity = _measurableQuantityList.First(meq => meq.ID == entry.ID);
            RaisePropertyChanged("SelectedMeasurableQuantity");
        }

        #endregion



        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            _confirm.RaiseCanExecuteChanged();
        }

        #endregion

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public bool CanModifyQuantity
        {
            get { return _canModifyQuantity; }
            set
            {
                _canModifyQuantity = value;
                RaisePropertyChanged("CanModifyQuantity");
            }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public IEnumerable<MeasurableQuantity> MeasurableQuantityList
        {
            get { return _measurableQuantityList; }
        }

        public MeasurementUnit MeasurementUnitInstance
        {
            get { return _measurementUnitInstance; }
        }

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
    }
}
