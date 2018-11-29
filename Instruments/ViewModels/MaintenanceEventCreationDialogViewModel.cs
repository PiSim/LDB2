using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure;
using Infrastructure.Commands;
using LInst;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading;
using System.Windows;

namespace Instruments.ViewModels
{
    public class MaintenanceEventCreationDialogViewModel : BindableBase
    {
        private IDataService<LInstContext> _lInstData;

        #region Constructors

        public MaintenanceEventCreationDialogViewModel(IDataService<LInstContext> lInstData) : base()
        {
            _lInstData = lInstData;
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
                    EventInstance.TechID = (Thread.CurrentPrincipal as DBPrincipal).CurrentPerson.ID;

                    _lInstData.Execute(new InsertEntityCommand<LInstContext>(EventInstance));

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