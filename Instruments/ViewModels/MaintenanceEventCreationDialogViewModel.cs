using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Instruments.ViewModels
{
    public class MaintenanceEventCreationDialogViewModel : BindableBase
    {
        private DateTime _date;
        private DBPrincipal _principal;
        private DelegateCommand<Window> _cancel, _confirm;
        private IDataService _dataService;
        private Instrument _instrumentInstance;
        private InstrumentMaintenanceEvent _eventInstance;
        private string _description;

        public MaintenanceEventCreationDialogViewModel(DBPrincipal principal,
                                                        IDataService dataService) : base()
        {
            _dataService = dataService;
            _date = DateTime.Now.Date;
            _principal = principal;
            _description = "";
            _cancel = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parentDialog =>
                {
                    _eventInstance = new InstrumentMaintenanceEvent();
                    _eventInstance.Date = _date;
                    _eventInstance.Description = _description;
                    _eventInstance.InstrumentID = _instrumentInstance.ID;
                    _eventInstance.PersonID = _principal.CurrentPerson.ID;

                    _eventInstance.Create();

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

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
            }
        }

        public InstrumentMaintenanceEvent EventInstance
        {
            get { return _eventInstance; }
        }

        public Instrument InstrumentInstance
        {
            get { return _instrumentInstance; }
            set
            {
                _instrumentInstance = value;
            }
        }

        public IEnumerable<Person> TechList => _dataService.GetPeople(PersonRoleNames.CalibrationTech);
        
    }
}
