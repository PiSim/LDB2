using Prism.Mvvm;

namespace LabDB2.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        #region Fields

        private string _title = "LabDB2";

        #endregion Fields

        #region Constructors

        public ShellViewModel()
        {
        }

        #endregion Constructors

        #region Properties

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Properties
    }
}