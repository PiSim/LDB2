using Infrastructure.Queries;
using LabDbContext;
using LInst;
using Prism.Commands;
using Prism.Mvvm;
using Specifications.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Specifications.ViewModels
{
    public class SpecificationCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private DataAccess.IDataService<LabDbEntities> _labDbData;
        private DataAccessCore.IDataService<LInstContext> _lInstData;

        private string _name;

        private Organization _oem;

        #endregion Fields

        #region Constructors

        public SpecificationCreationDialogViewModel(DataAccess.IDataService<LabDbEntities> labDbData,
                                                    DataAccessCore.IDataService<LInstContext> lInstData) : base()
        {
            _labDbData = labDbData;
            _lInstData = lInstData;
            CurrentIssue = "";
            Description = "";

            CancelCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parent =>
                {
                    SpecificationInstance = new Specification()
                    {
                        Description = Description,
                        Name = Name
                    };

                    Std tempStd = _labDbData.RunQuery(new StandardQuery() { Name = Name });

                    if (tempStd == null)
                        SpecificationInstance.Standard = new Std()
                        {
                            CurrentIssue = CurrentIssue,
                            Name = Name,
                            OrganizationID = _oem.ID
                        };
                    else
                        SpecificationInstance.StandardID = tempStd.ID;

                    SpecificationVersion tempMain = new SpecificationVersion();
                    tempMain.Name = "Generica";
                    tempMain.IsMain = true;

                    ControlPlan tempControlPlan = new ControlPlan();
                    tempControlPlan.Name = "Completo";
                    tempControlPlan.IsDefault = true;

                    SpecificationInstance.ControlPlans.Add(tempControlPlan);
                    SpecificationInstance.SpecificationVersions.Add(tempMain);

                    parent.DialogResult = true;
                },
                parent => !HasErrors);

            Oem = null;
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

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public string CurrentIssue { get; set; }

        public string Description { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                if (!string.IsNullOrEmpty(_name)
                    && _labDbData.RunQuery(new StandardQuery() { Name = _name }) == null)
                {
                    if (_validationErrors.ContainsKey("Name"))
                    {
                        _validationErrors.Remove("Name");
                        RaiseErrorsChanged("Name");
                    }
                }
                else
                {
                    _validationErrors["Name"] = new List<string>() { "Nome non valido" };
                    RaiseErrorsChanged("Name");
                }
            }
        }

        public Organization Oem
        {
            get { return _oem; }
            set
            {
                _oem = value;

                if (_oem != null)
                {
                    if (_validationErrors.ContainsKey("Oem"))
                    {
                        _validationErrors.Remove("Oem");
                        RaiseErrorsChanged("Oem");
                    }
                }
                else
                {
                    _validationErrors["Oem"] = new List<string>() { "Oem non valido" };
                    RaiseErrorsChanged("Oem");
                }
            }
        }

        public IEnumerable<Organization> OemList => _lInstData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.StandardPublisher })
                                                                        .ToList();

        public Specification SpecificationInstance { get; private set; }

        #endregion Properties
    }
}