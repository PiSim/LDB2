using LabDbContext;
using System.Windows;
using System.Windows.Controls;

namespace Controls.Views
{
    /// <summary>
    /// Logica di interazione per MaterialCodeBox.xaml
    /// </summary>
    public partial class MaterialCodeBox : UserControl
    {
        #region Fields

        public static readonly DependencyProperty MaterialInstanceProperty =
            DependencyProperty.Register("MaterialInstance", typeof(Material),
                typeof(MaterialCodeBox), new PropertyMetadata(null));

        #endregion Fields

        #region Constructors

        public MaterialCodeBox()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Material MaterialInstance
        {
            get { return (Material)GetValue(MaterialInstanceProperty); }
            set
            {
                SetValue(MaterialInstanceProperty, value);
            }
        }

        #endregion Properties
    }
}