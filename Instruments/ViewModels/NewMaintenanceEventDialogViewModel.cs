using DBManager;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments.ViewModels
{
    public class NewMaintenanceEventDialogViewModel : BindableBase
    {
        private DateTime _date;
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _cancel, _confirm;
        private Person _selectedTech;
        private string _description;
        private Views.NewMaintenanceEventDialog _parentDialog;

        public NewMaintenanceEventDialogViewModel(DBEntities entities,
                                                DBPrincipal principal,
                                                Views.NewMaintenanceEventDialog parentDialog) : base()
        {
            _date = DateTime.Now.Date;
            _entities = entities;
            _principal = principal;
            _parentDialog = parentDialog;

            _cancel = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = true;
                });
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand ConfirmCommand
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

        public List<Person> TechList
        {
            get
            {
                return new List<Person>(_entities.People.Where(per => per.Role == "TL"));
            }
        }

        public Person SelectedTech
        {
            get { return _selectedTech; }
            set { _selectedTech = value; }
        }
    }
}
