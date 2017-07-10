﻿using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
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

namespace Specifications.ViewModels
{
    public class SpecificationVersionEditViewModel : BindableBase
    {
        private bool _editMode;
        private DBPrincipal _principal;
        private DelegateCommand _save,
                                _startEdit;
        private List<RequirementWrapper> _requirementList;
        private SpecificationVersion _specificationVersionInstance;
        
        public SpecificationVersionEditViewModel(DBPrincipal principal)
        {
            _editMode = false;
            _principal = principal;

            _save = new DelegateCommand(
                () =>
                {
                    _specificationVersionInstance.Update();

                    if (_specificationVersionInstance == null)
                        return;

                    if (_specificationVersionInstance.IsMain)
                        SpecificationService.UpdateRequirements(_requirementList.Select(req => req.RequirementInstance));

                    else
                        SpecificationService.UpdateRequirements(_requirementList.Where(req => req.IsOverride)
                                                                                .Select(req => req.RequirementInstance));

                    EditMode = false;
                },
                () => _editMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => CanEdit && !_editMode);
        }
        
        private bool CanEdit
        {
            get { return _principal.IsInRole(UserRoleNames.SpecificationAdmin); }
        }

        private void GenerateRequirementList()
        {
            if (_specificationVersionInstance == null)
                _requirementList = new List<RequirementWrapper>();

            else
                _requirementList = new List<RequirementWrapper>(_specificationVersionInstance.GenerateRequirementList()
                                                                                            .Select(req => new RequirementWrapper(req, _specificationVersionInstance))
                                                                                            .ToList());
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                RaisePropertyChanged("IsOverrideVisibility");

                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public Visibility IsOverrideVisibility
        {
            get
            {
                if (_specificationVersionInstance == null)
                    return Visibility.Collapsed;

                else
                    return (_editMode && !_specificationVersionInstance.IsMain) 
                        ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public IEnumerable<RequirementWrapper> RequirementList
        {
            get { return _requirementList; }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public bool SelectedVersionIsNotMain
        {
            get
            {
                if (_specificationVersionInstance == null)
                    return false;

                return !_specificationVersionInstance.IsMain;
            }
        }


        public SpecificationVersion SpecificationVersionInstance
        {
            get { return _specificationVersionInstance; }
            set
            {
                EditMode = false;

                _specificationVersionInstance = value;
                _specificationVersionInstance.Load();
                
                GenerateRequirementList();

                RaisePropertyChanged("RequirementList");
                RaisePropertyChanged("SpecificationVersionEditViewVisibility");
                RaisePropertyChanged("SpecificationVersionName");
            }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }

        public Visibility SpecificationVersionEditViewVisibility
        {
            get
            {
                return (_specificationVersionInstance != null) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public string SpecificationVersionName
        {
            get
            {
                if (_specificationVersionInstance == null)
                    return null;

                else
                    return _specificationVersionInstance.Name;
            }

            set
            {
                _specificationVersionInstance.Name = value;
            }
        }
    }
}