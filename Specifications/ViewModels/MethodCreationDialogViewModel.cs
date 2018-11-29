using DataAccess;
using DataAccessCore;
using Infrastructure.Commands;
using Infrastructure.Queries;
using LabDbContext;
using LInst;
using Prism.Commands;
using Prism.Mvvm;
using Specifications.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Specifications.ViewModels
{
    internal class MethodCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private DataAccess.IDataService<LabDbEntities> _labDbData;
        DataAccessCore.IDataService<LInstContext> _lInstContext;
        private string _name;
        private Organization _selectedOem;
        private Property _selectedProperty;
        private Std _standardInstance;

        #endregion Fields

        #region Constructors

        public MethodCreationDialogViewModel(DataAccess.IDataService<LabDbEntities> labDbData,
                                            DataAccessCore.IDataService<LInstContext> lInstContext) : base()
        {
            _lInstContext = lInstContext;
            _labDbData = labDbData;
            OemList = _lInstContext.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.StandardPublisher })
                                                                        .ToList(); ;
            PropertiesList = _labDbData.RunQuery(new PropertiesQuery()).ToList();

            CancelCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parent =>
                {
                    MethodInstance = new Method()
                    {
                        Duration = WorkHours,
                        Description = Description,
                        Name = "",
                        PropertyID = _selectedProperty.ID,
                        ShortDescription = ShortDescription,
                        TBD = ""
                    };

                    if (_standardInstance == null)
                    {
                        _standardInstance = new Std
                        {
                            Name = Name,
                            OrganizationID = _selectedOem.ID,
                            CurrentIssue = ""
                        };

                        MethodInstance.Standard = _standardInstance;
                    }
                    else
                    {
                        if (_selectedOem.ID != _standardInstance.OrganizationID)
                        {
                            _standardInstance.OrganizationID = _selectedOem.ID;
                            _labDbData.Execute(new UpdateEntityCommand(_standardInstance));
                        }

                        MethodInstance.StandardID = _standardInstance.ID;
                    }

                    int subMethodPositioncounter = 0;
                    foreach (SubMethod subm in SubMethodList)
                    {
                        subm.Position = subMethodPositioncounter++;
                        MethodInstance.SubMethods.Add(subm);
                    }

                    MethodInstance.MethodVariants
                                    .Add(new MethodVariant()
                                    {
                                        Description = "",
                                        Name = "Standard"
                                    });

                    parent.DialogResult = true;
                },
                parent => !HasErrors);

            Description = "";
            Name = "";
            SelectedOem = null;
            SelectedProperty = null;
            ShortDescription = "";
            SubMethodList = new ObservableCollection<SubMethod>();
            WorkHours = 0;

            SubMethodList.CollectionChanged += OnSubMethodListChanged;
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

        #region Methods

        private void OnSubMethodListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (SubMethodList != null && SubMethodList.Count != 0)
            {
                if (_validationErrors.ContainsKey("SubMethodList"))
                    _validationErrors.Remove("SubMethodList");
            }
            else
                _validationErrors["SubMethodList"] = new List<string>() { "La Lista prove non può essere vuota" };

            RaisePropertyChanged("SubMethodList");
            RaiseErrorsChanged("SubMethodList");
        }

        #endregion Methods

        #region Properties

        public DelegateCommand AddSubMethodCommand { get; }

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public string Description { get; set; }

        public Method MethodInstance { get; private set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                _standardInstance = _labDbData.RunQuery(new StandardQuery() { Name = _name });

                if (_standardInstance != null)
                    SelectedOem = OemList.FirstOrDefault(oem => oem.ID == _standardInstance.OrganizationID);

                if (!string.IsNullOrEmpty(_name))
                {
                    if (_validationErrors.ContainsKey("Name"))
                        _validationErrors.Remove("Name");
                }
                else
                    _validationErrors["Name"] = new List<string>() { "Nome non valido" };

                RaiseErrorsChanged("Name");
            }
        }

        public IEnumerable<Organization> OemList { get; }

        public IEnumerable<Property> PropertiesList { get; }

        public DelegateCommand<SubMethod> RemoveSubMethodCommand { get; }

        public Organization SelectedOem
        {
            get { return _selectedOem; }
            set
            {
                _selectedOem = value;

                if (_selectedOem != null)
                {
                    if (_validationErrors.ContainsKey("SelectedOem"))
                        _validationErrors.Remove("SelectedOem");
                }
                else
                    _validationErrors["SelectedOem"] = new List<string>() { "Oem non valido" };

                RaisePropertyChanged("SelectedOem");
                RaiseErrorsChanged("SelectedOem");
            }
        }

        public Property SelectedProperty
        {
            get { return _selectedProperty; }
            set
            {
                _selectedProperty = value;

                if (_selectedProperty != null)
                {
                    if (_validationErrors.ContainsKey("SelectedProperty"))
                        _validationErrors.Remove("SelectedProperty");
                }
                else
                    _validationErrors["SelectedProperty"] = new List<string>() { "Proprietà non valida" };

                RaiseErrorsChanged("SelectedProperty");
            }
        }

        public string ShortDescription { get; set; }

        public ObservableCollection<SubMethod> SubMethodList { get; private set; }

        public double WorkHours { get; set; }

        #endregion Properties
    }
}