using DBManager;
using DBManager.Services;
using Microsoft.Practices.Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Materials.ViewModels
{
    public class AspectCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        private Aspect _aspectInstance;
        private readonly DelegateCommand<Window> _cancel,
                                                _confirm;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        public AspectCreationDialogViewModel()
        {
            _aspectInstance = new Aspect();

            _cancel = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = true;
                },
                parent => !HasErrors);

            AspectCode = "";
            AspectName = "";
        
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
            _confirm.RaiseCanExecuteChanged();
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged("HasErrors");
        }

        #endregion


        public Aspect AspectInstance => _aspectInstance;

        public string AspectCode
        {
            get { return _aspectInstance.Code; }
            set
            {
                _aspectInstance.Code = value;

                if (value.Length == 3 && DBManager.Services.MaterialService.GetAspect(value) == null)
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

        public string AspectName
        {
            get { return _aspectInstance.Name; }
            set
            {
                _aspectInstance.Name = value;
            }
        }

        public DelegateCommand<Window> CancelCommand => _cancel;

        public DelegateCommand<Window> ConfirmCommand => _confirm;
    }
}
