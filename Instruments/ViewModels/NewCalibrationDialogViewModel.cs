using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure.Commands;
using Infrastructure.Queries;
using Instruments.Queries;
using LInst;
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
        private IDataService<LInstContext> _lInstData;
        private string _referenceCode;
        private Organization _selectedLab;

        #endregion Fields

        #region Constructors

        public NewCalibrationDialogViewModel(IEventAggregator eventAggregator,
                                            InstrumentService instrumentService,
                                            IDataService<LInstContext> lInstData) : base()
        {
            _lInstData = lInstData;
            _instrumentService = instrumentService;
            IsVerificationOnly = false;
            LabList = _lInstData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.CalibrationLab })
                                                                        .ToList();
            _eventAggregator = eventAggregator;

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
                    ReportInstance.InstrumentID = _instumentInstance.ID;
                    ReportInstance.LaboratoryID = _selectedLab.ID;
                    ReportInstance.Notes = "";
                    ReportInstance.CalibrationResultID = 1;

                    if (IsNotExternalLab)
                    {
                        ReportInstance.TechID = SelectedTech.ID;

                    }

                    _lInstData.Execute(new InsertEntityCommand<LInstContext>(ReportInstance));

                    parentDialog.DialogResult = true;
                });
        }

        #endregion Constructors

        #region Properties

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
                _instumentInstance = _lInstData.RunQuery(new InstrumentQuery() { ID = value.ID });
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
        public Person SelectedTech { get; set; }

        public List<Person> TechList => _lInstData.RunQuery(new PeopleQuery() { Role = PeopleQuery.PersonRoles.CalibrationTech })
                                                            .ToList();

        #endregion Properties
    }
}