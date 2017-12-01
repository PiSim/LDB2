using DBManager;
using DBManager.EntityExtensions;
using Infrastructure;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reports.ViewModels
{
    public class AddTestDialogViewModel : BindableBase, IRequirementSelector
    {
        private DelegateCommand<Window> _cancel, _confirm;
        private IEnumerable<ReportItemWrapper> _testList;
        private Report _reportInstance;

        public AddTestDialogViewModel() : base()
        {

            _cancel = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = true;
                });
        }

        public void OnRequirementSelectionChanged()
        {

        }

        public string BatchNumber
        {
            get
            {
                if (_reportInstance == null || _reportInstance.Batch == null)
                    return null;
                return _reportInstance.Batch.Number;
            }
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public Report ReportInstance
        {
            get { return _reportInstance; }
            set
            {
                _reportInstance = value;
                _reportInstance.SpecificationVersion.Load();

                _testList = _reportInstance.SpecificationVersion.GenerateRequirementList()
                                                                .Select(req => new ReportItemWrapper(req, this))
                                                                .ToList();

                RaisePropertyChanged("BatchNumber");
                RaisePropertyChanged("ReportNumber");
                RaisePropertyChanged("SpecificationName");
                RaisePropertyChanged("TestList");
            }
        }

        public string ReportNumber
        {
            get
            {
                if (_reportInstance == null)
                    return null;

                return _reportInstance.Number.ToString();
            }
        }

        public string SpecificationName
        {
            get
            {
                if (_reportInstance == null)
                    return null;

                return _reportInstance.SpecificationVersion.Specification.Standard.Name
                    + " - " + _reportInstance.SpecificationVersion.Name;
            }
        }

        public IEnumerable<ReportItemWrapper> TestList
        {
            get { return _testList; }
        }
    }
}
