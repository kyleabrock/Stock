using System.Globalization;
using System.Windows.Input;
using Stock.Core.Domain;

namespace Stock.UI.ViewModels.Base
{
    public class TableSearchViewModel<T> : TableViewModel<T>, ITableSearchViewModel where T : EntityBase
    {
        protected TableSearchViewModel()
        {
            SearchCommand = new RelayCommand(x => FindMethod());
        }

        private string _searchString;
        public virtual string SearchString
        {
            get { return _searchString; }
            set { _searchString = value; OnPropertyChanged("SearchString"); }
        }

        public int SearchStringMininumLength { get; set; }

        public bool IsSearched { get; set; }
        public ICommand SearchCommand { get; set; }

        protected bool StringContains(string arg, string compareString)
        {
            var culture = CultureInfo.GetCultureInfo("ru-RU");
            return culture.CompareInfo.IndexOf(arg, compareString, CompareOptions.IgnoreCase) >= 0;
        }

        private void FindMethod()
        {
            IsSearched = SearchString.Length > SearchStringMininumLength;
            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
        }
    }
}
