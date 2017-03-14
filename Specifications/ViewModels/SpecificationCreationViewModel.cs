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
                    Std tempStd = _entities.Stds.FirstOrDefault(std => std.Name == _name);
                    if (tempStd == null)
                    {
                        tempStd = new Std();
                        tempStd.Name = Name;
                        tempStd.Organization = _oem;

                        StandardIssue tempIssue = new StandardIssue();
                        tempIssue.IsCurrent = true;
                        tempIssue.Issue = DateTime.Now.ToShortDateString();

                        tempStd.CurrentIssue = tempIssue;
                    }

                    SpecificationVersion tempMain = new SpecificationVersion();
                    tempMain.Name = "Generica";
                    tempMain.IsMain = true;

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
                                            .Any(orm => orm.Role.Name == "STD_PUB" && orm.IsSelected)));
            }
        }
    }
}
