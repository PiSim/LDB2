using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public class GenericItemWrapper<T>
    {
        private bool _isSelected;
        private T _item;

        public GenericItemWrapper (T item)
        {
            _item = item;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => _isSelected = value;
        }

        public T Item => _item;
    }
}
