using Infrastructure;
using LabDbContext;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Reports.ViewModels
{
    public static class ExternalReportExtension
    {
        #region Methods

        /// <summary>
        /// Extension Method used to generate an ObservableCollection of DataGridColumns
        /// that will be used to correctly visualize the results in a DataGrid
        /// </summary>
        /// <returns>An ObservableCollection of DataGridColumns</returns>
        public static ObservableCollection<DataGridColumn> GetResultPresentationColumns(this ExternalReport report)
        {
            ObservableCollection<DataGridColumn> output = new ObservableCollection<DataGridColumn>();

            output.Add(new DataGridTextColumn()
            {
                Header = "Metodo",
                Binding = new Binding("MethodName"),
                IsReadOnly = true,
                Width = 100
            });

            output.Add(new DataGridTextColumn()
            {
                Header = "Variante",
                Binding = new Binding("MethodVariantName"),
                IsReadOnly = true,
                Width = 150
            });

            output.Add(new DataGridTemplateColumn()
            {
                Header = "Prove",
                CellTemplate = (DataTemplate)Application.Current.Resources[ResourceKeys.DataGridSubTestNameCellTemplate],
                Width = 150
            });

            output.Add(new DataGridTemplateColumn()
            {
                Header = "UM",
                CellTemplate = (DataTemplate)Application.Current.Resources[ResourceKeys.DataGridSubTestUMCellTemplate],
                Width = 150
            });

            foreach (TestRecord tstr in report.TestRecords)
            {
                DataGridColumn resultColumn = new DataGridTemplateColumn()
                {
                    Header = tstr.Batch.Number,
                    CellTemplate = (DataTemplate)Application.Current.Resources[ResourceKeys.DataGridSubTestExternalResultCellTemplate],
                    CellStyle = (Style)Application.Current.Resources[ResourceKeys.DataGridCellEnabledOnEditModeStyle],
                    Width = 150
                };

                DataGridColumnTagExtension.SetTag(resultColumn, tstr.BatchID);

                output.Add(resultColumn);
            }

            return output;
        }

        #endregion Methods
    }
}