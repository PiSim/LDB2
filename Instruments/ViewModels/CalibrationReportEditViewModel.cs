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
    public class CalibrationReportEditViewModel : BindableBase
    {
        private bool _editMode;
        private CalibrationReport _calibrationInstance;
        private DBPrincipal _principal;
        private DelegateCommand _delete,
                                _save,
                                _startEdit;
        private IEnumerable<Organization> _labList;
        private IEnumerable<Person> _techList;
        private Organization _selectedLab;
        private Person _selectedPerson;
        
        public CalibrationReportEditViewModel(DBPrincipal principal)
        {
            _editMode = false;
            _principal = principal;

            _labList = OrganizationService.GetOrganizations(OrganizationRoleNames.CalibrationLab);
            _techList = PeopleService.GetPeople(PersonRoleNames.CalibrationTech);

            _delete = new DelegateCommand(
                () =>
                {
                    _calibrationInstance.Delete();
                },
                () => _principal.IsInRole(UserRoleNames.InstrumentAdmin));

            _save = new DelegateCommand(
                () =>
                {
                    _calibrationInstance.Update();
                    EditMode = false;
                });

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !EditMode);
        }

        public CalibrationReport CalibrationInstance
        {
            get { return _calibrationInstance; }
            set
            {
                _calibrationInstance = value;

                SelectedLab = _labList.FirstOrDefault(lab => lab.ID == _calibrationInstance?.laboratoryID);
                SelectedTech = _techList.FirstOrDefault(tech => tech.ID == _calibrationInstance?.OperatorID);

                RaisePropertyChanged("SelectedLab");
                RaisePropertyChanged("SelectedTech");

                RaisePropertyChanged("CalibrationInstance");
                RaisePropertyChanged("ReportViewVisibility");
            }
        }

        public DelegateCommand DeleteCommand
        {
            get { return _delete; }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<Organization> LabList
        {
            get { return _labList; }
        }

        public Visibility ReportViewVisibility
        {
            get
            {
                if (_calibrationInstance == null)
                    return Visibility.Hidden;

                else
                    return Visibility.Visible;
            }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public Organization SelectedLab
        {
            get { return _selectedLab; }
            set
            {
                _selectedLab = value;
            }
        }

        public Person SelectedTech
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
            }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }

        public IEnumerable<Person> TechList
        {
            get { return _techList; }
        }
    }
}
