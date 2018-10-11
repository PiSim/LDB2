using DataAccess;
using Infrastructure.Commands;
using Infrastructure.Queries;
using Instruments.Queries;
using LabDbContext;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Instruments.ViewModels
{
    public class NewCalibrationDialogViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private InstrumentService _instrumentService;
        private Instrument _instumentInstance, _selectedReference;
        private IDataService<LabDbEntities> _labDbData;
        private string _referenceCode;
        private Organization _selectedLab;

        #endregion Fields

        #region Constructors

        public NewCalibrationDialogViewModel(IEventAggregator eventAggregator,
                                            InstrumentService instrumentService,
                                            IDataService<LabDbEntities> labDbData) : base()
        {
            _labDbData = labDbData;
            _instrumentService = instrumentService;
            IsVerificationOnly = false;
            ReferenceList = new ObservableCollection<Instrument>();
            LabList = _labDbData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.CalibrationLab })
                                                                        .ToList();
            _eventAggregator = eventAggregator;

            CalibrationDate = DateTime.Now.Date;

            AddReferenceCommand = new DelegateCommand<string>(
                code =>
                {
                    Instrument tempRef = _labDbData.RunQuery(new InstrumentQuery() { Code = code  } );
                    if (tempRef != null)
                    {
                        ReferenceList.Add(tempRef);
                        ReferenceCode = "";
                    }
                });

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
                    ReportInstance.instrumentID = _instumentInstance.ID;
                    ReportInstance.IsVerification = IsVerificationOnly;
                    ReportInstance.laboratoryID = _selectedLab.ID;
                    ReportInstance.Notes = "";
                    ReportInstance.ResultID = 1;

                    if (IsNotExternalLab)
                    {
                        ReportInstance.OperatorID = SelectedTech.ID;

                        foreach (Instrument refInstrument in ReferenceList)
                            ReportInstance.ReferenceInstruments.Add(refInstrument);
                    }

                    foreach (InstrumentMeasurableProperty imp in _instumentInstance.GetMeasurableProperties())
                    {
                        CalibrationReportInstrumentPropertyMapping cripm = new CalibrationReportInstrumentPropertyMapping()
                        {
                            ExtendedUncertainty = 0,
                            LowerRangeValue = imp.RangeLowerLimit,
                            MeasurablePropertyID = imp.ID,
                            MeasurementUnitID = imp.UnitID,
                            UpperRangeValue = imp.RangeUpperLimit
                        };

                        ReportInstance.InstrumentMeasurablePropertyMappings.Add(cripm);
                    }

                    _labDbData.Execute(new InsertEntityCommand(ReportInstance));

                    parentDialog.DialogResult = true;
                });

            RemoveReference = new DelegateCommand(
                () =>
                {
                    ReferenceList.Remove(_selectedReference);
                    SelectedReference = null;
                },
                () => _selectedReference != null);
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand<string> AddReferenceCommand { get; }

        public DateTime CalibrationDate { get; set; }

        public string CalibrationNotes { get; set; }

        public string CalibrationResult { get; set; }

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public string InstrumentCode
        {
            get
            {
                if (_instumentInstance == null)
                    return null;

                return _instumentInstance.Code;
            }
        }

        public Instrument InstrumentInstance
        {
            get { return _instumentInstance; }
            set
            {
                _instumentInstance = _labDbData.RunQuery(new InstrumentQuery() { ID = value.ID });
                SelectedLab = LabList.FirstOrDefault(lab => lab.ID == _instumentInstance.CalibrationResponsibleID);
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

        public bool IsVerificationOnly { get; set; }
        public IEnumerable<Organization> LabList { get; }

        public string ReferenceCode
        {
            get { return _referenceCode; }
            set
            {
                _referenceCode = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Instrument> ReferenceList { get; }

        public DelegateCommand RemoveReference { get; }

        public CalibrationReport ReportInstance { get; private set; }

        public Organization SelectedLab
        {
            get { return _selectedLab; }
            set
            {
                _selectedLab = value;
                RaisePropertyChanged("SelectedLab");
                RaisePropertyChanged("IsNotExternalLab");
            }
        }

        public Instrument SelectedReference
        {
            get
            {
                return _selectedReference;
            }

            set
            {
                _selectedReference = value;
                RemoveReference.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        public Person SelectedTech { get; set; }

        public List<Person> TechList => _labDbData.RunQuery(new PeopleQuery() { Role = PeopleQuery.PersonRoles.CalibrationTech })
                                                            .ToList();

        #endregion Properties
    }
}