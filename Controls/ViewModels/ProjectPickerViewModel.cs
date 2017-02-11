using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controls.ViewModels
{
    internal class ProjectPickerViewModel : BindableBase
    {
        DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private Project _selectedProject;
        private Views.ProjectPickerDialog _parentDialog;

        internal ProjectPickerViewModel(DBEntities entities,
                                        Views.ProjectPickerDialog parentDialog) : base()
        {
            _cancel = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    _parentDialog.ProjectInstance = _selectedProject;
                    _parentDialog.DialogResult = true;
                });
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }

        public List<Project> ProjectList
        {
            get { return new List<Project>(_entities.Projects); }
        }

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set { _selectedProject = value; }
        }

    }
}
