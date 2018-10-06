using LabDbContext;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace Specifications.ViewModels
{
    public class ModifyMethodSubMethodListDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private Method _oldVersion;

        #endregion Fields

        #region Constructors

        public ModifyMethodSubMethodListDialogViewModel()
        {
            CancelCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = true;
                });
        }

        #endregion Constructors

        #region Commands

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        #endregion Commands

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

        private void OnSubMethodCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("IsMoreThanOneSubMethod");
        }

        #endregion Methods

        #region Properties

        public bool IsMoreThanOneSubMethod => SubMethodList?.Count != 0;

        public Method OldVersion
        {
            get => _oldVersion;
            set
            {
                _oldVersion = value;
                _oldVersion.LoadSubMethods();

                SubMethodList = new ObservableCollection<SubMethod>();

                foreach (SubMethod subm in _oldVersion.SubMethods)
                    SubMethodList.Add(new SubMethod()
                    {
                        Name = subm.Name,
                        OldVersionID = subm.ID,
                        UM = subm.UM
                    });

                SubMethodList.CollectionChanged += OnSubMethodCollectionChanged;
                RaisePropertyChanged("IsMoreThanOneSubMethod");
                RaisePropertyChanged("SubMethodList");
            }
        }

        public ObservableCollection<SubMethod> SubMethodList { get; private set; }

        #endregion Properties
    }
}