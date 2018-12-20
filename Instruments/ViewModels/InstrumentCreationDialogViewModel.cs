using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure.Queries;
using Instruments.Queries;
using LInst;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Instruments.ViewModels
{
    internal class InstrumentCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private string _code;
        private bool _isUnderControl;
        private IDataService<LInstContext> _lInstData;
        private Organization _selectedCalibrationLab;

        #endregion Fields

        #region Constructors

        public InstrumentCreationDialogViewModel(IDataService<LInstContext> lInstData) : base()
        {
            _lInstData = lInstData;
            ControlPeriod = 12;
            Notes = "";
            Model = "";
            SerialNumber = "";
            CalibrationLabList = _lInstData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.CalibrationLab })
                                            .ToList();

            CancelCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parent =>
                {
                    InstrumentInstance = new Instrument();
                    InstrumentInstance.Code = _code;
                    InstrumentInstance.Description = Notes;
                    InstrumentInstance.InstrumentTypeID = SelectedType.ID;
                    InstrumentInstance.IsInService = true;
                    InstrumentInstance.IsUnderControl = _isUnderControl;
                    InstrumentInstance.CalibrationInterval = ControlPeriod;
                    InstrumentInstance.UtilizationAreaID = SelectedArea.ID;
                    InstrumentInstance.CalibrationDueDate = (_isUnderControl) ? DateTime.Now : (DateTime?)null;
                    InstrumentInstance.CalibrationResponsibleID = _selectedCalibrationLab?.ID;
                    InstrumentInstance.ManufacturerID = SelectedManufacturer.ID;
                    InstrumentInstance.Model = Model;
                    InstrumentInstance.SerialNumber = SerialNumber;

                    _lInstData.Execute(new InsertEntityCommand<LInstContext>(InstrumentInstance));

                    parent.DialogResult = true;
                },
                parent => !HasErrors);

            SelectedCalibrationLab = null;
        }

        #endregion Constructors

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

        public IEnumerable<InstrumentUtilizationArea> AreaList => _lInstData.RunQuery(new InstrumentUtilizationAreasQuery()).ToList();
        public IEnumerable<Organization> CalibrationLabList { get; }
        public DelegateCommand<Window> CancelCommand { get; }

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;

                if (string.IsNullOrEmpty(_code))
                    InstrumentInstance = null;
                else
                    _lInstData.RunQuery(new InstrumentQuery() { Code = _code });

                if (InstrumentInstance == null &&
                    _validationErrors.ContainsKey("Code"))
                {
                    _validationErrors.Remove("Code");
                    RaiseErrorsChanged("Code");
                }
                else if (InstrumentInstance != null)
                {
                    _validationErrors["Code"] = new List<string>() { "Lo strumento " + _code + " esiste già" };
                    RaiseErrorsChanged("Code");
                }
            }
        }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public int ControlPeriod { get; set; }

        public Instrument InstrumentInstance { get; private set; }

        public bool IsUnderControl
        {
            get { return _isUnderControl; }
            set
            {
                _isUnderControl = value;
                RaisePropertyChanged("IsUnderControl");
                if (value)
                    SelectedCalibrationLab = CalibrationLabList.First(lab => lab.Name == "Vulcaflex");
                else
                    SelectedCalibrationLab = null;
            }
        }

        public IEnumerable<Organization> ManufacturerList => _lInstData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.Manufacturer })
                                                                        .ToList();

        public string Model { get; set; }

        public string Notes { get; set; }

        public InstrumentUtilizationArea SelectedArea { get; set; }

        public Organization SelectedCalibrationLab
        {
            get
            {
                return _selectedCalibrationLab;
            }

            set
            {
                _selectedCalibrationLab = value;
                if (_selectedCalibrationLab == null && IsUnderControl)
                    _validationErrors["SelectedCalibrationLab"] = new List<string>() { "Selezionare un laboratorio" };
                else
                    _validationErrors.Remove("SelectedCalibrationLab");

                RaiseErrorsChanged("SelectedCalibrationLab");
                RaisePropertyChanged("SelectedCalibrationLab");
            }
        }

        public Organization SelectedManufacturer { get; set; }
        public InstrumentType SelectedType { get; set; }
        public string SerialNumber { get; set; }
        public IEnumerable<InstrumentType> TypeList => _lInstData.RunQuery(new InstrumentTypesQuery()).ToList();
        private bool IsValidInput => true;

        #endregion Properties
    }
}