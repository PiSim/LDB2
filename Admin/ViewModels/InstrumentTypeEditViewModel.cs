using LabDbContext;
using LabDbContext.EntityExtensions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Admin.ViewModels
{
    public class InstrumentTypeEditViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private InstrumentType _instrumentTypeInstance;
        
        #endregion Fields

        #region Constructors

        public InstrumentTypeEditViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        #endregion Constructors

        #region Properties
        
        public InstrumentType InstrumentTypeInstance
        {
            get { return _instrumentTypeInstance; }
            set
            {
                _instrumentTypeInstance = value;
            }
        }

        #endregion Properties
    }
}