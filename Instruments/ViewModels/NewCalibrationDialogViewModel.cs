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
    public class NewCalibrationDialogViewModel : BindableBase
    {
        private DateTime _calibrationDate;
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _cancel, _confirm;
        private Instrument _instumentInstance, _selectedReference;
        private Person _selectedTech;
        private Organization _selectedLab;
        private Views.NewCalibrationDialog _parentDialog;

        public NewCalibrationDialogViewModel(DBEntities entities,
                                            DBPrincipal principal,
                                            Views.NewCalibrationDialog parentDialog) : base()
        {
            _entities = entities;
            _principal = principal;

            _cancel = new DelegateCommand(
                () =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    CalibrationReport output = new CalibrationReport();
                    output.Date = _calibrationDate;
                    output.Instrument = _instumentInstance;
                    output.Laboratory = _selectedLab;
                    if (!IsExternalLab)
                    {
                        output.Tech = _selectedTech;
                        output.Reference = _selectedReference;
                    }
                    _entities.CalibrationReports.Add(output);
                    _entities.SaveChanges();

                    _parentDialog.ReportInstance = output;
                    _parentDialog.DialogResult = true;
                });
        }

        public DateTime CalibrationDate
        {
            get { return _calibrationDate; }
            set
            {
                _calibrationDate = value;
            }
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public bool CanChangeTech
        {
            get
            {
                return _principal.IsInRole(UserRoleNames.ReportAdmin); 
            }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }

        public Instrument InstrumentInstance
        {
            get { return _instumentInstance; }
            set
            {
                _instumentInstance = _entities.Instruments.First(ins => ins.ID == value.ID);
            }
        }

        public bool IsExternalLab
        {
            get { return _selectedLab.Name != "Vulcaflex"; }
        }

        public List<Organization> LabList
        {
            get
            {
                OrganizationRole tempOr = _entities.OrganizationRoles.First(orr => orr.Name == OrganizationRoleNames.CalibrationLab);
                return new List<Organization>(tempOr.OrganizationMappings.Where(orm => orm.IsSelected)
                                                .Select(orm => orm.Organization));
            }
        }

        public Organization SelectedLab
        {
            get { return _selectedLab; }
            set
            {
                _selectedLab = value;
                OnPropertyChanged("SelectedLab");
                OnPropertyChanged("IsExternalLab");
            }
        }

        public Instrument SelectedReference
        {
            get { return _selectedReference; }
            set
            {
                _selectedReference = value;
            }
        }

        public Person SelectedTech
        {
            get { return _selectedTech; }
            set
            {
                _selectedTech = value;
            }
        }

        public List<Person> TechList
        {
            get
            {
                PersonRole tempRole = _entities.PersonRoles.First(prr => prr.Name == PersonRoleNames.CalibrationTech);
                return new List<Person>(tempRole.RoleMappings.Where(prm => prm.IsSelected)
                                            .Select(prm => prm.Person));
            }
        }
    }
}
