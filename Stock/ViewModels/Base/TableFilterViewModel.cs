using System;
using System.Globalization;
using System.Windows.Input;
using Stock.Core.Domain;

namespace Stock.UI.ViewModels.Base
{
    public class TableFilterViewModel<T> : TableViewModel<T> where T : EntityBase
    {
        public TableFilterViewModel()
        {
            FindCommand = new RelayCommand(x => FindMethod());
        }

        private string _searchString;
        public virtual string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                OnPropertyChanged("SearchString");
            }
        }

        public bool Filter { get; set; }

        public ICommand FindCommand { get; set; }

        protected int MinSearchString = 1;

        protected virtual void RefreshMethod()
        {
            throw new NotImplementedException();
        }

        protected virtual bool FilterItems(object obj)
        {
            if (!(obj is T))
                return false;

            var filterString = SearchString;
            var right = (T)obj;

            if (StringContains(right.Id.ToString(), filterString))
                return true;
            return false;
        }

        protected bool StringContains(string arg, string compareString)
        {
            var culture = CultureInfo.GetCultureInfo("ru-RU");
            return culture.CompareInfo.IndexOf(arg, compareString, CompareOptions.IgnoreCase) >= 0;
        }

        private void FindMethod()
        {
            Filter = SearchString.Length > MinSearchString;
            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
        }
    }
}
