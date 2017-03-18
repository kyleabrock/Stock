using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Repository;

namespace Stock.UI.ViewModels.Base
{
    public class TableViewModel<T> : ViewModelBase where T : EntityBase
    {
        protected TableViewModel()
        {
            RefreshCommand = new AsyncCommand(x => RefreshMethod());
            RefreshCommand.RunWorkerCompleted += RefreshCommand_RunWorkerCompleted;
            SearchCommand = new RelayCommand(x => FindMethod());
        }

        private string _searchString;
        public virtual string SearchString
        {
            get { return _searchString; }
            set { _searchString = value; OnPropertyChanged("SearchString"); }
        }

        private ICollectionView _itemList;
        public ICollectionView ItemList
        {
            get { return _itemList; }
            set { _itemList = value; OnPropertyChanged("ItemList"); }
        }

        private IList<T> _itemListCollection;
        public IList<T> ItemListCollection
        {
            get { return _itemListCollection; }
            set { _itemListCollection = value; OnPropertyChanged("ItemListCollection"); }
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        public ICommand SearchCommand { get; set; }
        public AsyncCommand RefreshCommand { get; set; }

        protected IRepository<T> Repository;
        protected IFilter Filter;
        protected int SearchStringMininumLength { get; set; }
        protected bool IsSearched { get; set; }
        
        protected virtual void RefreshMethod()
        {
            if (!IsSearched)
                ItemListCollection = Repository.GetAllAsTableView();
            else
            {
                if (!string.IsNullOrEmpty(SearchString))
                    Filter.SearchString = SearchString;
                Filter.CreateFilter();
                ItemListCollection = Repository.Find(Filter);
            }
            
            var item = SelectedItem as EntityBase;
            if (item != null) SelectedItem = Repository.GetById(item.Id);
        }

        protected virtual void RefreshCommand_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SaveTableSortOrder();
            ItemList = CollectionViewSource.GetDefaultView(ItemListCollection);
            LoadTableSortOrder();
        }

        private SortDescriptionCollection _sortDescriptionCollection;

        private void FindMethod()
        {
            if (SearchString == null) SearchString = string.Empty;

            IsSearched = SearchString.Length >= SearchStringMininumLength;
            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
        }

        private void SaveTableSortOrder()
        {
            if (_itemList == null) return;
            if (_itemList.SortDescriptions.Count <= 0) return;

            if (_sortDescriptionCollection == null)
                _sortDescriptionCollection = new SortDescriptionCollection();

            var sortDescriptionArray = new SortDescription[_itemList.SortDescriptions.Count];
            _itemList.SortDescriptions.CopyTo(sortDescriptionArray, 0);

            _sortDescriptionCollection.Clear();
            foreach (var sortDescription in sortDescriptionArray)
                _sortDescriptionCollection.Add(sortDescription);
        }

        private void LoadTableSortOrder()
        {
            if (_itemList == null) return;
            if (_sortDescriptionCollection == null) return;

            _itemList.SortDescriptions.Clear();
            foreach (var sortDescription in _sortDescriptionCollection)
                _itemList.SortDescriptions.Add(sortDescription);
        }
    }
}
