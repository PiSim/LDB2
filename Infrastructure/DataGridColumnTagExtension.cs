using System.Windows;
using System.Windows.Controls;

namespace Infrastructure
{
    public static class DataGridColumnTagExtension
    {
        #region Fields

        public static readonly System.Windows.DependencyProperty TagProperty = DependencyProperty.RegisterAttached(
            "Tag",
            typeof(object),
            typeof(DataGridColumn),
            new FrameworkPropertyMetadata(null));

        #endregion Fields

        #region Methods

        public static object GetTag(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(TagProperty);
        }

        public static void SetTag(DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(TagProperty, value);
        }

        #endregion Methods
    }
}