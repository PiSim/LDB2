using DBManager;
using DBManager.EntityExtensions;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services.ViewModels
{
    public class AddTestDialogViewModel : BindableBase
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

                _testList = _reportInstance.GetAddableTests()
                                            .Select(req => new ReportItemWrapper(req));

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
                    + " - " + _reportInstance.SpecificationVersion.Name
                        + " : " + ((_reportInstance.SpecificationIssues == null) ? null : _reportInstance.SpecificationIssues.Issue);
            }
        }

        public IEnumerable<ReportItemWrapper> TestList
        {
            get { return _testList; }
        }
    }
}
