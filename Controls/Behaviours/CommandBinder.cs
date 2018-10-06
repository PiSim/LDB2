using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Controls.Behaviours
{
    public class CommandBinder
    {
        #region Fields

        public static readonly DependencyProperty DataGridDoubleClickCommand =
            DependencyProperty.RegisterAttached("DataGridDoubleClickCommand", typeof(ICommand), typeof(CommandBinder),
                new PropertyMetadata(new PropertyChangedCallback(AttachOrRemoveDataGridDoubleClickEvent)));

        #endregion Fields

        #region Methods

        public static void AttachOrRemoveDataGridDoubleClickEvent(DependencyObject obj,
                                                                DependencyPropertyChangedEventArgs args)
        {
            DataGrid dataGrid = obj as DataGrid;
            if (dataGrid != null)
            {
                ICommand cmd = (ICommand)args.NewValue;

                if (args.OldValue == null && args.NewValue != null)
                    dataGrid.MouseDoubleClick += ExecuteDataGridDoubleClick;
                else if (args.OldValue != null && args.NewValue == null)
                    dataGrid.MouseDoubleClick += ExecuteDataGridDoubleClick;
            }
        }

        public static void ExecuteDataGridDoubleClick(object sender,
                                                    MouseButtonEventArgs args)
        {
            DependencyObject obj = sender as DependencyObject;
            ICommand cmd = (ICommand)obj.GetValue(DataGridDoubleClickCommand);

            if (cmd != null)
                if (cmd.CanExecute(obj))
                    cmd.Execute(obj);
        }

        public static ICommand GetDataGridDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DataGridDoubleClickCommand);
        }

        public static void SetDataGridDoubleClickCommand(DependencyObject obj,
                                                        ICommand value)
        {
            obj.SetValue(DataGridDoubleClickCommand, value);
        }

        #endregion Methods
    }
}