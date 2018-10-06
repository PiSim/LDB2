namespace Infrastructure.Wrappers
{
    public class GenericItemWrapper<T>
    {
        #region Constructors

        public GenericItemWrapper(T item)
        {
            Item = item;
        }

        #endregion Constructors

        #region Properties

        public bool IsSelected { get; set; }

        public T Item { get; }

        #endregion Properties
    }
}