using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace Controls.ViewModels
{
    public class MaterialCreationDialogViewModel : BindableBase
    {
        #region Constructors

        public MaterialCreationDialogViewModel()
            : base()
        {
            ConfirmCreationCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = true;
                },
                parent => IsValidInput);
        }

        #endregion Constructors

        #region Properties

        public string Aspect { get; set; }

        public DelegateCommand<Window> ConfirmCreationCommand { get; }

        public string Line { get; set; }
        public string Recipe { get; set; }

        public string Type { get; set; }

        private bool IsValidInput => true;

        #endregion Properties
    }
}