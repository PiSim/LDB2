using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Reporting
{
    public class ReportingEngine
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;

        public ReportingEngine(DBEntities entities,
                                EventAggregator eventAggregator)
        {
            _entities = entities;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<GenerateReportDataSheetRequested>()
                            .Subscribe(
                            report =>
                            {
                                GenerateReportRawDataSheet(report);
                            });
        }

        public void GenerateReportRawDataSheet(Report target)
        {
            Formats.ReportRawDataSheet dataSheet = new Formats.ReportRawDataSheet(target);




            PrintDialog printer = new PrintDialog();

            DocumentPaginator paginator = ((IDocumentPaginatorSource)dataSheet).DocumentPaginator;

            if (printer.ShowDialog() == true)
                printer.PrintDocument(paginator, "Report");
        }

        public static void PrintBatchStatusList()
        {
            Formats.BatchStatusListReport batchReport = new Formats.BatchStatusListReport();

            batchReport.DateBox.Text = DateTime.Now.ToShortDateString();

            IEnumerable<Batch> statusList = MaterialService.GetBatches(100);

            Table batchTable = batchReport.BatchTable;
            batchTable.FontSize = 16;

            double[] columnWidths = new double[]{ 2,
                                                    2,
                                                    2,
                                                    2,
                                                    2,
                                                    3,
                                                    2,
                                                    2,
                                                    2,
                                                    2,
                                                    2,
                                                    2};

            TableColumn currentColumn;

            foreach (double colWidth in columnWidths)
            {
                currentColumn = new TableColumn();
                currentColumn.Width = new GridLength(colWidth, GridUnitType.Star);
                batchTable.Columns.Add(currentColumn);
            }

            TableRow currentRow;

            batchTable.RowGroups.Add(new TableRowGroup());

            currentRow = new TableRow();

            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("Odp"))));

            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("Tipo"))));

            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("Riga"))));

            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("Aspetto"))));

            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("Cod.Colore"))));

            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("Colore"))));

            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("OEM"))));
            
            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("Construction"))));

            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("Tipo batch"))));

            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("Ricevuto"))));

            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("Data arrivo"))));

            currentRow.Cells.Add(new TableCell
                                (new Paragraph
                                (new Run("Report Basic"))));

            currentRow.Background = Brushes.LightBlue;

            batchTable.RowGroups[0].Rows.Add(currentRow);

            bool isBlueRow = false;

            foreach (Batch btc in statusList)
            {
                currentRow = new TableRow();

                currentRow.Cells.Add(new TableCell
                                    (new Paragraph
                                    (new Run(btc.Number))));

                currentRow.Cells.Add(new TableCell
                                    (new Paragraph
                                    (new Run((btc.Material != null) ? btc.Material.MaterialType.Code : ""))));

                currentRow.Cells.Add(new TableCell
                                    (new Paragraph
                                    (new Run((btc.Material != null) ? btc.Material.MaterialLine.Code : ""))));

                currentRow.Cells.Add(new TableCell
                                    (new Paragraph
                                    (new Run((btc.Material != null) ? btc.Material.Aspect.Code : ""))));

                currentRow.Cells.Add(new TableCell
                                    (new Paragraph
                                    (new Run((btc.Material != null) ? btc.Material.Recipe.Code : ""))));

                currentRow.Cells.Add(new TableCell
                                    (new Paragraph
                                    (new Run((btc.Material != null && btc.Material.Recipe.Colour != null) ? btc.Material.Recipe.Colour.Name : ""))));
                
                currentRow.Cells.Add(new TableCell
                                    (new Paragraph
                                    (new Run((btc.Material != null && btc.Material.ExternalConstruction != null) ? btc.Material.ExternalConstruction.Oem?.Name : ""))));

                currentRow.Cells.Add(new TableCell
                                    (new Paragraph
                                    (new Run((btc.Material != null && btc.Material.ExternalConstruction != null) ? btc.Material.ExternalConstruction.Name : ""))));

                currentRow.Cells.Add(new TableCell
                                    (new Paragraph
                                    (new Run((btc.TrialArea != null) ? btc.TrialArea.Name : ""))));

                currentRow.Cells.Add(new TableCell
                                    (new Paragraph
                                    (new Run((btc.FirstSampleArrived) ? "X" : " "))));

                currentRow.Cells.Add(new TableCell
                                    (new Paragraph
                                    (new Run((btc.FirstSample != null) ? btc.FirstSample.Date.ToShortDateString() : ""))));

                if (btc.DoNotTest)
                    currentRow.Cells.Add(new TableCell
                                        (new Paragraph
                                        (new Run("NO REPORT"))));

                else
                    currentRow.Cells.Add(new TableCell
                                        (new Paragraph
                                        (new Run((btc.BasicReport != null) ? btc.BasicReport.Number.ToString() : ""))));

                if (isBlueRow)
                    currentRow.Background = Brushes.Azure;

                isBlueRow = !isBlueRow;

                batchTable.RowGroups[0]
                            .Rows
                            .Add(currentRow);
            }

            PrintDialog printer = new PrintDialog();

            if (printer.ShowDialog() == true)
            {
                printer.PrintTicket.PageOrientation = PageOrientation.Landscape;
                batchReport.PageWidth = printer.PrintableAreaWidth;
                batchReport.PagePadding = new Thickness(10);
                batchReport.ColumnWidth = printer.PrintableAreaWidth;
                DocumentPaginator paginator = ((IDocumentPaginatorSource)batchReport).DocumentPaginator;
                printer.PrintDocument(paginator, "Report");
            }
        }
    }
}
