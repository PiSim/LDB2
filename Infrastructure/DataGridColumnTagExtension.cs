using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Infrastructure
{
    public static class DataGridColumnTagExtension
    {

        public static readonly System.Windows.DependencyProperty TagProperty = DependencyProperty.RegisterAttached(
            "Tag",
            typeof(object),
            typeof(DataGridColumn),
            new FrameworkPropertyMetadata(null));

        public static object GetTag(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(TagProperty);
        }

        public static void SetTag(DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(TagProperty, value);
        }

    }
}
