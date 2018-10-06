/*
   Copyright 2014 Christoph Gattnar

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Infrastructure.Sorting
{
    public class Sorting
    {
        #region Fields

        private GridViewColumnHeader _sortColumn;
        private ListSortDirection _sortDirection;

        #endregion Fields

        #region Methods

        public void Sort(object columnHeader, CollectionView list)
        {
            string column = SetAdorner(columnHeader);

            if (column == null)
                return;

            list.SortDescriptions.Clear();
            list.SortDescriptions.Add(
                new SortDescription(column, _sortDirection));
        }

        private string SetAdorner(object columnHeader)
        {
            GridViewColumnHeader column = columnHeader as GridViewColumnHeader;
            if (column == null)
            {
                return null;
            }

            // Remove arrow from previously sorted header
            if (_sortColumn != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(_sortColumn);
                try { adornerLayer.Remove((adornerLayer.GetAdorners(_sortColumn))[0]); }
                catch { }
            }

            if (_sortColumn == column)
            {
                // Toggle sorting direction
                _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                                                   ListSortDirection.Descending :
                                                   ListSortDirection.Ascending;
            }
            else
            {
                _sortColumn = column;
                _sortDirection = ListSortDirection.Ascending;
            }

            var sortingAdorner = new SortingAdorner(column, _sortDirection);
            AdornerLayer.GetAdornerLayer(column).Add(sortingAdorner);

            string header = string.Empty;

            // if binding is used and property name doesn't match header content
            Binding b = _sortColumn.Column.DisplayMemberBinding as Binding;
            if (b != null)
            {
                header = b.Path.Path;
            }

            return header;
        }

        #endregion Methods
    }
}