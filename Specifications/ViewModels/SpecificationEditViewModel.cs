using DBManager;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications.ViewModels
{
    internal class RequirementWrapper
    {
        private EventAggregator _eventAggregator;
        private Requirement _requirementInstance;

        internal RequirementWrapper(Requirement instance, EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _requirementInstance = instance;
        }

        public bool IsOverride
        {
            get { return (_requirementInstance.IsOverride == 1) ? true : false ; }

            set
            {
                _requirementInstance.IsOverride = (value) ? 1 : 0;
                if (value)
            }
        }
    }

    internal class SpecificationEditViewModel : BindableBase
    {
        private DBEntities _entities;
        private Property _filterProperty;
        private Specification _instance;
        private SpecificationVersion _selectedVersion;

        internal SpecificationEditViewModel(DBEntities entities, Specification instance) 
            : base()
        {
            _instance = instance;
            _entities = entities;
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
            get { return new List<Method>(
                    _entities.Methods.Where(mtd => mtd.Property.ID == FilterProperty.ID)); }
        }

        public Specification Instance
        {
            get { return _instance; }
        }

        public List<Requirement> MainVersionRequirements
        {
            get { return new List<Requirement>(
                _instance.SpecificationVersions.First(ver => ver.IsMain == 1).Requirements); }
        }

        public SpecificationVersion SelectedVersion
        {
            get { return _selectedVersion; }
        }

        public List<SpecificationVersion> VersionList
        {
            get { return new List<SpecificationVersion>(_instance.SpecificationVersions); }
        }
    }
}
