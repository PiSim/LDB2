using Infrastructure;
using Infrastructure.Wrappers;
using LabDbContext;
using LabDbContext.EntityExtensions;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Reports.ViewModels
{
    public class AddTestDialogViewModel : BindableBase, IRequirementSelector
    {
        #region Fields

        private Report _reportInstance;

        #endregion Fields

        #region Constructors

        public AddTestDialogViewModel() : base()
        {
            CancelCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = true;
                });
        }

        #endregion Constructors

        #region Properties

        public string BatchNumber
        {
            get
            {
                if (_reportInstance == null || _reportInstance.Batch == null)
                    return null;
                return _reportInstance.Batch.Number;
            }
        }

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public Report ReportInstance
        {
            get { return _reportInstance; }
            set
            {
                _reportInstance = value;

                TestList = _reportInstance.SpecificationVersion.GenerateRequirementList()
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

        public IEnumerable<ReportItemWrapper> TestList { get; private set; }

        #endregion Properties

        #region Methods

        public void OnRequirementSelectionChanged()
        {
        }

        #endregion Methods
    }
}