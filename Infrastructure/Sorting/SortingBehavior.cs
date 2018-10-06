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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Infrastructure.Sorting
{
    public class SortingBehavior : Behavior<ListView>
    {
        #region Fields

        private Sorting _sorting;

        #endregion Fields

        #region Constructors

        public SortingBehavior()
        {
            _sorting = new Sorting();
        }

        #endregion Constructors

        #region Methods

        protected override void OnAttached()
        {
            AssociatedObject.AddHandler(GridViewColumnHeader.ClickEvent,
                            new RoutedEventHandler(OnColumnHeaderClicked));
        }

        protected override void OnDetaching()
        {
            AssociatedObject.RemoveHandler(GridViewColumnHeader.ClickEvent,
                            new RoutedEventHandler(OnColumnHeaderClicked));
        }

        private void OnColumnHeaderClicked(object sender, RoutedEventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView == null)
            {
                return;
            }

            _sorting.Sort(e.OriginalSource, listView.Items);
        }

        #endregion Methods
    }
}