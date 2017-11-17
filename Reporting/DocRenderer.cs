using Reporting.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Reporting
{
    internal class DocRenderer
    {
        internal DocRenderer()
        {

        }

        internal void AddStandardHeader(Grid targetGrid)
        {
            targetGrid.Children.Add(new StandardHeader());
        }

        internal void AddBatchList(Grid targetGrid, IEnumerable<DBManager.Batch> batchList)
        {
            BatchList listElement = new BatchList();
            listElement.MainList.ItemsSource = batchList;

            targetGrid.Children.Add(listElement);
        }

        internal FixedPage GetNewPage()
        {
            FixedPage output = new FixedPage();

            output.Children.Add(new Grid()
            {
                Margin = new Thickness(20, 20, 20, 20)
            });

            return output;
        }
    }
}
