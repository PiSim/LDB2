using LabDbContext;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Materials
{
    public class NewBatchWrapper : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private bool _isSelected = true;

        #endregion Fields

        #region Constructors

        public NewBatchWrapper(Batch instance)
        {
            BatchInstance = instance;
            Validate();
        }

        #endregion Constructors

        #region Events

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public event EventHandler<EventArgs> HasErrorsChanged, IsSelectedChanged, MaterialDataChanged, RecipeDataChanged;

        #endregion Events

        #region Properties

        public string AspectCode
        {
            get => BatchInstance.Material?.Aspect.Code;
            set
            {
                BatchInstance.Material.Aspect.Code = value;
                Validate();
                if (!HasErrors)
                    RaiseMaterialDataChanged();
                RaiseHasErrorsChanged();
            }
        }

        public Batch BatchInstance { get; }

        public Colour ColorInstance
        {
            get => BatchInstance.Material.Recipe.Colour;
            set
            {
                BatchInstance.Material.Recipe.Colour = value;
                RaisePropertyChanged("ColorInstance");
            }
        }

        public ExternalConstruction ConstructionInstance
        {
            get => BatchInstance.Material.ExternalConstruction;
            set
            {
                BatchInstance.Material.ExternalConstruction = value;
                RaisePropertyChanged("ConstructionInstance");
            }
        }

        public bool DoNotTest
        {
            get => BatchInstance.DoNotTest;
            set => BatchInstance.DoNotTest = value;
        }

        public bool HasErrors => _validationErrors.Count > 0;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaiseIsSelectedChanged();
            }
        }

        public string MaterialLineCode
        {
            get => BatchInstance.Material?.MaterialLine?.Code;
            set
            {
                BatchInstance.Material.MaterialLine.Code = value;
                Validate();
                if (!HasErrors)
                    RaiseMaterialDataChanged();
                RaiseHasErrorsChanged();
            }
        }

        public string MaterialTypeCode
        {
            get => BatchInstance.Material?.MaterialType?.Code;
            set
            {
                BatchInstance.Material.MaterialType.Code = value;
                Validate();
                if (!HasErrors)
                    RaiseMaterialDataChanged();
                RaiseHasErrorsChanged();
            }
        }

        public string Number => BatchInstance.Number;

        public Project ProjectInstance
        {
            get => BatchInstance.Material.Project;
            set
            {
                BatchInstance.Material.Project = value;
                RaisePropertyChanged("ProjectInstance");
            }
        }

        public string RecipeCode
        {
            get => BatchInstance.Material?.Recipe?.Code;
            set
            {
                BatchInstance.Material.Recipe.Code = value;
                Validate();
                if (!HasErrors)
                {
                    RaiseRecipeDataChanged();
                    RaiseMaterialDataChanged();
                }
                RaiseHasErrorsChanged();
            }
        }

        public TrialArea TrialAreaInstance
        {
            get => BatchInstance.TrialArea;
            set
            {
                BatchInstance.TrialAreaID = value.ID;
                BatchInstance.TrialArea = value;
            }
        }

        #endregion Properties

        #region Methods

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        /// <summary>
        /// Sets the IsSelectedProperty without raising OnBatchErrorsChanged in the Parent View Model
        /// </summary>
        /// <param name="value">the value to set</param>
        public void SetIsSelected(bool value)
        {
            _isSelected = value;
            RaisePropertyChanged("IsSelected");
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            HasErrorsChanged?.Invoke(this, new EventArgs());
        }

        private void RaiseHasErrorsChanged()
        {
            HasErrorsChanged?.Invoke(this, new EventArgs());
        }

        private void RaiseIsSelectedChanged()
        {
            IsSelectedChanged?.Invoke(this, new EventArgs());
        }

        private void RaiseMaterialDataChanged()
        {
            MaterialDataChanged?.Invoke(this, new EventArgs());
        }

        private void RaiseRecipeDataChanged()
        {
            RecipeDataChanged?.Invoke(this, new EventArgs());
        }

        private void Validate()
        {
            ///Validate Aspect Code

            if (AspectCode.Length == 3)
            {
                if (_validationErrors.ContainsKey("AspectCode"))
                    _validationErrors.Remove("AspectCode");
            }
            else
                _validationErrors["AspectCode"] = new List<string>() { "Codice Aspetto non valido" };

            RaiseErrorsChanged("AspectCode");

            ///Validate Line Code

            if (BatchInstance.Material.MaterialLine.Code.Length == 3)
            {
                if (_validationErrors.ContainsKey("MaterialLineCode"))
                    _validationErrors.Remove("MaterialLineCode");
            }
            else
                _validationErrors["MaterialLineCode"] = new List<string>() { "Codice Riga non valido" };

            RaiseErrorsChanged("MaterialLineCode");

            ///Validate Type Code

            if (BatchInstance.Material.MaterialType.Code.Length == 4)
            {
                if (_validationErrors.ContainsKey("MaterialTypeCode"))
                    _validationErrors.Remove("MaterialTypeCode");
            }
            else
                _validationErrors["MaterialTypeCode"] = new List<string>() { "Codice Tipo non Valido" };

            RaiseErrorsChanged("MaterialTypeCode");

            /// Validate Recipe code

            if (BatchInstance.Material.Recipe.Code.Length == 4)
            {
                if (_validationErrors.ContainsKey("RecipeCode"))
                    _validationErrors.Remove("RecipeCode");
            }
            else
                _validationErrors["RecipeCode"] = new List<string>() { "Codice colore non valido" };

            RaiseErrorsChanged("RecipeCode");
        }

        #endregion Methods
    }
}