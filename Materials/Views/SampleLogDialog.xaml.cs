using System.Windows;
using System.Windows.Input;

namespace Materials.Views
{
    /// <summary>
    /// Logica di interazione per SampleLogDialog.xaml
    /// </summary>
    public partial class SampleLogDialog : Window
    {
        #region Constructors

        public SampleLogDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ConfirmButton.Command.CanExecute(null))
                ConfirmButton.Command.Execute(null);
        }

        #endregion Methods
    }
}