using LabDbContext;
using Prism.Events;
using Prism.Mvvm;

namespace Admin.ViewModels
{
    public class PropertyEditViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;

        #endregion Fields

        #region Constructors

        public PropertyEditViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        #endregion Constructors

        #region Properties

        public Property PropertyInstance { get; set; }

        #endregion Properties
    }
}