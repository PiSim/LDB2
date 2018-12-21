using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure.Queries;
using Instruments.Queries;
using LInst;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Instruments.ViewModels
{
    public class NewCalibrationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private IEventAggregator _eventAggregator;
        private InstrumentService _instrumentService;
        private Instrument _instrumentInstance;
        private IDataService<LInstContext> _lInstData;
        private Person _selectedTech;
        private Organization _selectedLab;

        #endregion Fields

        #region Constructors

        public NewCalibrationDialogViewModel(IEventAggregator eventAggregator,
                                            InstrumentService instrumentService,
                                            IDataService<LInstContext> lInstData) : base()
        {
            _lInstData = lInstData;
            _instrumentService = instrumentService;
            LabList = _lInstData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.CalibrationLab })
                                                                        .ToList();
            _eventAggregator = eventAggregator;
            _validationErrors = new Dictionary<string, ICollection<string>>();
            CalibrationDate = DateTime.Now.Date;

            CancelCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    ReportInstance = new CalibrationReport();
                    ReportInstance.Date = CalibrationDate;
                    ReportInstance.Year = DateTime.Now.Year - 2000;
                    ReportInstance.Number = _instrumentService.GetNextCalibrationNumber(ReportInstance.Year);
                    ReportInstance.InstrumentID = _instrumentInstance.ID;
                    ReportInstance.LaboratoryID = _selectedLab.ID;
                    ReportInstance.Notes = "";
                    ReportInstance.CalibrationResultID = 1;

                    if (IsNotExternalLab)
                    {
                        ReportInstance.TechID = SelectedTech.ID;
                    }

                    foreach (InstrumentProperty iProperty in _lInstData.RunQuery(new InstrumentPropertiesQuery(ReportInstance.InstrumentID) { ExcludeNonCalibrationProperties = true }))
                        ReportInstance.CalibrationReportProperties.Add(new CalibrationReportProperty()
                        {
                            Name = iProperty.Name,
                            TargetValue = iProperty.TargetValue,
                            UpperLimit = iProperty.UpperLimit,
                            LowerLimit = iProperty.LowerLimit,
                            ParentPropertyID = iProperty.ID,
                            UM = iProperty.UM
                        });

                    _lInstData.Execute(new InsertEntityCommand<LInstContext>(ReportInstance));

                    parentDialog.DialogResult = true;
                },
                parentDialog => !HasErrors);
        }

        #endregion Constructors

        private void UpdateSelectedLabErrors()
        {
            if (SelectedLab == null)
                _validationErrors["SelectedTech"] = new string[] { "E' necessario selezionare un laboratorio." };
            else
                _validationErrors.Remove("SelectedLab");

            RaiseErrorsChanged("SelectedLab");
        }

        private void UpdateSelectedTechErrors()
        {
            if (SelectedLab == null)
                return;

            if (!IsNotExternalLab || SelectedTech != null)
                _validationErrors.Remove("SelectedTech");

            else
                _validationErrors["SelectedTech"] = new string[] { "E' Necessario Selezionare un Tecnico." };

            RaiseErrorsChanged("SelectedTech");
        }

        #region Properties

        public DateTime CalibrationDate { get; set; }

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public string InstrumentCode
        {
            get
            {
                if (_instrumentInstance == null)
                    return null;

                return _instrumentInstance.Code;
            }
        }

        public Instrument InstrumentInstance
        {
            get { return _instrumentInstance; }
            set
            {
                _instrumentInstance = _lInstData.RunQuery(new InstrumentQuery() { ID = value.ID });

                SelectedLab = LabList.FirstOrDefault(lab => lab.ID == _instrumentInstance.CalibrationResponsibleID);
                UpdateSelectedTechErrors();
                RaisePropertyChanged("InstrumentCode");
                RaisePropertyChanged("PropertyList");
                RaisePropertyChanged("SelectedLab");
            }
        }

        public bool IsNotExternalLab
        {
            get
            {
                if (_selectedLab == null)
                    return false;

                return _selectedLab.Name == "Vulcaflex";
            }
        }

        public IEnumerable<Organization> LabList { get; }

        public CalibrationReport ReportInstance { get; private set; }

        public Organization SelectedLab
        {
            get { return _selectedLab; }
            set
            {
                _selectedLab = value;
                UpdateSelectedLabErrors();
                if (IsNotExternalLab)
                    SelectedTech = null;

                RaisePropertyChanged("SelectedLab");
                RaisePropertyChanged("IsNotExternalLab");
            }
        }

        public Person SelectedTech
        { 
            get => _selectedTech;
            set
            {
                _selectedTech = value;
                UpdateSelectedTechErrors();
                RaisePropertyChanged("SelectedTech");
            }
        }

        public List<Person> TechList => _lInstData.RunQuery(new PeopleQuery() { Role = PeopleQuery.PersonRoles.CalibrationTech })
                                                            .ToList();

        public bool HasErrors => _validationErrors.Values.Count != 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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

        #endregion Properties
    }
}