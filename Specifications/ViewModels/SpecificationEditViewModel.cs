using DBManager;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications.ViewModels
{
    internal class RequirementWrapper : BindableBase
    {
        private DBEntities _entities;
        private Requirement _requirementInstance;
        private SpecificationVersion _versionInstance;

        internal RequirementWrapper(Requirement instance,
                                    SpecificationVersion version, 
                                    DBEntities entities) : base()
        {
            _entities = entities;
            _requirementInstance = instance;
            _versionInstance = version;
        }

        public bool CanSetOverride
        {
            get { return (_versionInstance.IsMain == 1) ? false : true; }
        }

        public bool IsOverride
        {
            get { return (_requirementInstance.IsOverride == 1) ? true : false ; }

            set
            {
                if (value)
                    _requirementInstance = _entities.AddOverride(_versionInstance, _requirementInstance);
                else
                    _requirementInstance = _entities.RemoveOverride(_requirementInstance);
                    
                OnPropertyChanged("SubRequirements");
            }
        }

        public List<SubRequirement> SubRequirements
        {
            get { return new List<SubRequirement>(_requirementInstance.SubRequirements); }
        }
        
        public string Standard
        {
            get { return _requirementInstance.Method.Standard.Organization.Name + " " +
                    _requirementInstance.Method.Standard.Name; }
        }

        public string Test
        {
            get { return _requirementInstance.Method.Property.Name; }
        }
    }

    internal class SpecificationEditViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _addTest, _removeTest;
        private ObservableCollection<RequirementWrapper> _requirementList;
        private Method _selectedToAdd;
        private Property _filterProperty;
        private Requirement _selectedToRemove;
        private Specification _instance;
        private SpecificationVersion _selectedVersion;

        internal SpecificationEditViewModel(DBEntities entities, Specification instance) 
            : base()
        {
            _addTest = new DelegateCommand(
                () =>
                {
                    _entities.AddTest(_instance, _selectedToAdd);
                    OnPropertyChanged("MainVersionRequirements");
                },
                () => _selectedToAdd != null);

            _removeTest = new DelegateCommand(
                () =>
                {
                    MainVersion.Requirements.Remove(_selectedToRemove);
                    OnPropertyChanged("MainVersionRequirements");
                },

                () => _selectedToRemove != null);

            _requirementList = new ObservableCollection<RequirementWrapper>();
            _instance = instance;
            _entities = entities;
        }

        public DelegateCommand AddTestCommand
        {
            get { return _addTest; }
        }

        public Property FilterProperty
        {
            get { return _filterProperty; }
            set
            {
                _filterProperty = value;
                OnPropertyChanged("FilteredMethods");
            }
        }

        public List<Method> FilteredMethods
        {
            get
            {
                if (_filterProperty == null)
                    return new List<Method>(_entities.Methods);
                else
                    return new List<Method>(
                        _entities.Methods.Where(mtd => mtd.Property.ID == FilterProperty.ID));
            }
        }

        public Specification Instance
        {
            get { return _instance; }
        }

        public SpecificationVersion MainVersion
        {
            get { return _instance.SpecificationVersions.First(ver => ver.IsMain == 1); }
        }

        public ObservableCollection<Requirement> MainVersionRequirements
        {
            get { return new ObservableCollection<Requirement>(MainVersion.Requirements); }
        }

        public List<Property> Properties
        {
            get { return new List<Property>(_entities.Properties); }
        }

        public DelegateCommand  RemoveTestCommand
        {
            get { return _removeTest; }
        }

        public ObservableCollection<RequirementWrapper> RequirementList
        {
            get { return _requirementList; }
        }

        public SpecificationVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                _selectedVersion = value;
                OnPropertyChanged("SelectedVersionIsNotMain");
                List<Requirement> tempReqList = _entities.GenerateRequirementList(_selectedVersion);
                _requirementList.Clear();
                foreach (Requirement rr in tempReqList)
                    _requirementList.Add(new RequirementWrapper(rr, _selectedVersion, _entities));
            }
        }

        public bool SelectedVersionIsNotMain
        {
            get { return !(_selectedVersion.IsMain == 1); }
        }

        public Method SelectedToAdd
        {
            get { return _selectedToAdd; }
            set
            {
                _selectedToAdd = value;
                _addTest.RaiseCanExecuteChanged();
            }
        }

        public Requirement SelectedToRemove
        {
            get { return _selectedToRemove; }
            set
            {
                _selectedToRemove = value;
                _removeTest.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedToRemove");
            }
        }

        public string Standard
        {
            get
            {
                return _instance.Standard.Organization.Name + " " +
                  _instance.Standard.Name;
            }
        }

        public List<SpecificationVersion> VersionList
        {
            get { return new List<SpecificationVersion>(_instance.SpecificationVersions); }
        }
    }
}
