using Prism.Commands;

namespace Materials.ViewModels
{
    internal class BatchesNavigationItemViewModel : Prism.Mvvm.BindableBase
    {
        #region Fields

        private DelegateCommand _onSelected;

        #endregion Fields

        #region Constructors

        public BatchesNavigationItemViewModel() : base()
        {
            _onSelected = new DelegateCommand(
                () => { });
        }

        #endregion Constructors
    }
}