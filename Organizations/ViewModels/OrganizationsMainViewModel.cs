using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizations.ViewModels
{
    internal class OrganizationsMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private Organization _selectedOrganization;

        internal OrganizationsMainViewModel(DBEntities entities) : base()
        {
            _entities = entities;
        }

        public Organization SelectedOrganization
        {
            get { return _selectedOrganization; }
            set
            {
                _selectedOrganization = value;
                OnPropertyChanged("SelectedOrganization");
            }
        }

        public ObservableCollection<Organization> OrganizationList
        {
            get { return new ObservableCollection<Organization>(_entities.Organizations); }
        }

    }
}
