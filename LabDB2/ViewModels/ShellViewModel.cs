using Prism.Mvvm;

namespace LabDB2.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private string _title = "LabDB2";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ShellViewModel()
        {

        }
    }
}
