namespace Stock.UI
{
    public class CheckBoxItem<T>
    {
        private T _item;
        public T Item 
        {
            get { return _item; }
            set { _item = value; }
        }

        private bool _isChecked;
        public bool IsChecked 
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }
    }
}
