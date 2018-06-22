using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Specifications.ViewModels
{
    internal class MethodCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        private bool _isCreationFromOldVersion;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private IDataService _dataService;
        private Method _oldVersion;
        private Organization _selectedOem;
        private Property _selectedProperty;
        private Std _standardInstance;
        private string _name;

        public MethodCreationDialogViewModel(IDataService dataService) : base()
        {
            _dataService = dataService;
            OemList = _dataService.GetOrganizations(OrganizationRoleNames.StandardPublisher);
            PropertiesList = _dataService.GetProperties();

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
                            _standardInstance.Update();
                        } 


                        MethodInstance.StandardID = _standardInstance.ID;
                    }

                    foreach (SubMethod subm in SubMethodList)
                        MethodInstance.SubMethods.Add(subm);

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

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        #endregion

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

        #endregion

        public DelegateCommand AddSubMethodCommand { get; }

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public bool CanEditFields => !_isCreationFromOldVersion;

        public string Description { get; set; }

        public Method MethodInstance { get; private set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                _standardInstance = _dataService.GetStandard(_name);

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

        public Method OldVersion
        {
            get => _oldVersion;
            set 
            {
                _oldVersion = value;
                _isCreationFromOldVersion = value != null;

                SelectedOem = OemList.FirstOrDefault(oem => oem.ID == _oldVersion?.Standard?.OrganizationID);
                SelectedProperty = PropertiesList.FirstOrDefault(pro => pro.ID == _oldVersion?.PropertyID);

                SubMethodList.Clear();

                if (_isCreationFromOldVersion)
                {
                    Description = _oldVersion.Description;
                    ShortDescription = _oldVersion.Description;
                    Name = _oldVersion.Standard?.Name;

                    foreach (SubMethod subm in _oldVersion.SubMethods)
                        SubMethodList.Add(new SubMethod()
                        {
                            Name = subm.Name,
                            OldVersionID = subm.ID,
                            UM = subm.UM
                        });
                }

                else
                {
                    Description = "";
                    ShortDescription = "";
                    Name = "";
                }

                RaisePropertyChanged("CanEditFields");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("Name");
                RaisePropertyChanged("SelectedProperty");
                RaisePropertyChanged("ShortDescription");

            }
        }

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
                    _validationErrors["SelectedProperty"] = new List<string>() { "Proprietä non valida" };

                RaiseErrorsChanged("SelectedProperty");
            }
        }

        public string ShortDescription { get; set; }

        public ObservableCollection<SubMethod> SubMethodList { get; private set; }

        public double WorkHours { get; set; }
    }
}
