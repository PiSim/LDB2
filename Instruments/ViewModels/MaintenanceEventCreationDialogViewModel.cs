using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading;
using System.Windows;

namespace Instruments.ViewModels
{
    public class MaintenanceEventCreationDialogViewModel : BindableBase
    {
        private IDataService<LabDbEntities> _labDbData;

        #region Constructors

        public MaintenanceEventCreationDialogViewModel(IDataService<LabDbEntities> labDbData) : base()
        {
            _labDbData = labDbData;
            Date = DateTime.Now.Date;

            Description = "";
            CancelCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    EventInstance = new InstrumentMaintenanceEvent();
                    EventInstance.Date = Date;
                    EventInstance.Description = Description;
                    EventInstance.InstrumentID = InstrumentInstance.ID;
                    EventInstance.PersonID = (Thread.CurrentPrincipal as DBPrincipal).CurrentPerson.ID;

                    _labDbData.Execute(new InsertEntityCommand(EventInstance));

                    parentDialog.DialogResult = true;
                });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public InstrumentMaintenanceEvent EventInstance { get; private set; }

        public Instrument InstrumentInstance { get; set; }

        #endregion Properties
    }
}