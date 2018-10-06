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
using System.Windows.Documents;
using System.Windows.Media;

namespace Infrastructure.Sorting
{
    public class SortingAdorner : Adorner
    {
        #region Fields

        private static Geometry _arrowDown = Geometry.Parse("M 5,0 10,5 15,0 5,0");
        private static Geometry _arrowUp = Geometry.Parse("M 5,5 15,5 10,0 5,5");
        private Geometry _sortDirection;

        #endregion Fields

        #region Constructors

        public SortingAdorner(GridViewColumnHeader adornedElement, ListSortDirection sortDirection)
            : base(adornedElement)
        {
            _sortDirection = sortDirection == ListSortDirection.Ascending ? _arrowUp : _arrowDown;
        }

        #endregion Constructors

        #region Methods

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            double x = AdornedElement.RenderSize.Width - 20;
            double y = (AdornedElement.RenderSize.Height - 5) / 2;

            if (x >= 20)
            {
                // Right order of the statements is important
                drawingContext.PushTransform(new TranslateTransform(x, y));
                drawingContext.DrawGeometry(Brushes.Black, null, _sortDirection);
                drawingContext.Pop();
            }
        }

        #endregion Methods
    }
}