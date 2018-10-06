using System;
using System.Windows.Controls;

namespace Reporting.Controls
{
    /// <summary>
    /// Logica di interazione per StandardHeader.xaml
    /// </summary>
    public partial class StandardHeader : UserControl
    {
        #region Constructors

        public StandardHeader()
        {
            InitializeComponent();
            DateField.Text = DateTime.Now.ToShortDateString();
        }

        #endregion Constructors

        #region Properties

        public string Title
        {
            get => TitleBox.Text;
            set => TitleBox.Text = value;
        }

        #endregion Properties
    }
}