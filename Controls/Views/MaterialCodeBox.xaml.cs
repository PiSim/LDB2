using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Controls.Views
{
    /// <summary>
    /// Logica di interazione per MaterialCodeBox.xaml
    /// </summary>
    public partial class MaterialCodeBox : UserControl
    {
        public MaterialCodeBox()
        {
            InitializeComponent();
        }

        public Material MaterialInstance
        {
            get { return (Material)GetValue(MaterialInstanceProperty); }
            set
            {
                SetValue(MaterialInstanceProperty, value);
            }
        }

        public static readonly DependencyProperty MaterialInstanceProperty =
            DependencyProperty.Register("MaterialInstance", typeof(Material),
                typeof(MaterialCodeBox), new PropertyMetadata(null));
                
    }
}
