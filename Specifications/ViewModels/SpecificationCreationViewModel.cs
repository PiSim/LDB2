using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications.ViewModels
{
    internal class SpecificationCreationViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private Organization _oem;
        private Views.SpecificationCreationDialog _parentDialog;
        private SpecificationServiceProvider _serviceProvider;
        private string _name;

        internal SpecificationCreationViewModel(DBEntities entities,
                                                SpecificationServiceProvider serviceProvider,
                                                Views.SpecificationCreationDialog parentDialog) : base()
        {
            _entities = entities;
            _parentDialog = parentDialog;
            _serviceProvider = serviceProvider;

            _cancel = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    StandardIssue tempIssue = new StandardIssue();
                    tempIssue.IsOld = 0;
                    tempIssue.Issue = "";

                    Std tempStd = new Std();
                    tempStd.CurrentIssue = tempIssue;
                    tempStd.Name = _name;
                    tempStd.Organization = _oem;

                    SpecificationVersion tempMain = new SpecificationVersion();
                    tempMain.Name = "Generica";
                    tempMain.IsMain = 1;

                    Specification tempSpec = new Specification();
                    tempSpec.Description = "";
                    tempSpec.Standard = tempStd;
                    tempSpec.SpecificationVersions.Add(tempMain);

                    _entities.Specifications.Add(tempSpec);
                    _entities.SaveChanges();

                    _parentDialog.SpecificationInstance = tempSpec;
                    _parentDialog.DialogResult = true;
                },
                () => (_name != "" && _oem != null));
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand ConfirmCommand
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

        public List<Organization> OemList
        {
            get
            {
                return new List<Organization>
                    (_entities.Organizations.Where(org => org.RoleMapping
                                            .Any(orm => orm.Role.Name == "OEM" && orm.IsSelected)));
            }
        }
    }
}
