using DataAccess;
using LabDbContext;
using Materials.Queries;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Materials.ViewModels
{
    public class AspectCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public AspectCreationDialogViewModel(IDataService<LabDbEntities> labDbData)
        {
            _labDbData = labDbData;
            AspectInstance = new Aspect();

            CancelCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = true;
                },
                parent => !HasErrors);

            AspectCode = "";
            AspectName = "";
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
            ConfirmCommand.RaiseCanExecuteChanged();
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged("HasErrors");
        }

        #endregion INotifyDataErrorInfo interface elements

        #region Properties

        public string AspectCode
        {
            get { return AspectInstance.Code; }
            set
            {
                AspectInstance.Code = value;

                if (value.Length == 3 && _labDbData.RunQuery(new AspectQuery() { AspectCode = value }) == null)
                {
                    if (_validationErrors.ContainsKey("AspectCode"))
                    {
                        _validationErrors.Remove("AspectCode");
                        RaiseErrorsChanged("AspectCode");
                    }
                }
                else
                    _validationErrors["AspectCode"] = new List<string>() { "Codice aspetto non valido" };
            }
        }

        public Aspect AspectInstance { get; }

        public string AspectName
        {
            get { return AspectInstance.Name; }
            set
            {
                AspectInstance.Name = value;
            }
        }

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        #endregion Properties
    }
}