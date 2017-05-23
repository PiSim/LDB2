using DBManager;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Specifications.ViewModels
{
    internal class SpecificationCreationDialogViewModel : BindableBase
    {
        private DelegateCommand<Window> _cancel, _confirm;
        private Organization _oem;
        private Specification _specificationInstance;
        private string _name;

        internal SpecificationCreationDialogViewModel() : base()
        {

            _cancel = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parent =>
                {
                    Std tempStd = SpecificationService.GetStandard(_name);
                    if (tempStd == null)
                    {
                        tempStd = new Std();
                        tempStd.Name = Name;
                        tempStd.Organization = _oem;

                        StandardIssue tempIssue = new StandardIssue();
                        tempIssue.IsCurrent = true;
                        tempIssue.Issue = DateTime.Now.ToShortDateString();
                        tempIssue.Standard = tempStd;
                        tempStd.CurrentIssue = tempIssue;
                    }

                    SpecificationVersion tempMain = new SpecificationVersion();
                    tempMain.Name = "Generica";
                    tempMain.IsMain = true;
                    
                    ControlPlan tempControlPlan = new ControlPlan();
                    tempControlPlan.Name = "Completo";
                    tempControlPlan.IsDefault = true;

                    Specification tempSpec = new Specification();
                    tempSpec.Description = "";
                    tempSpec.Standard = tempStd;

                    tempSpec.ControlPlans.Add(tempControlPlan);
                    tempSpec.SpecificationVersions.Add(tempMain);

                    tempSpec.Create();
                    
                    parent.DialogResult = true;
                },
                parent => (_name != "" && _oem != null));
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public Organization Oem
        {
            get { return _oem; }
            set
            {
                _oem = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<Organization> OemList
        {
            get
            {
                return OrganizationService.GetOrganizations(OrganizationRoleNames.StandardPublisher);
            }
        }

        public Specification SpecificationInstance
        {
            get
            {
                return _specificationInstance;
            }
        }
    }
}
