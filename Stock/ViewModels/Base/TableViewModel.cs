using System.Collections.Generic;
using System.ComponentModel;
using Stock.Core.Domain;

namespace Stock.UI.ViewModels.Base
{
    public class TableViewModel<T> : ViewModelBase, ITableBaseViewModel<T> where T : EntityBase 
    {
        private ICollectionView _tableItemListView;
        public ICollectionView TableItemListView
        {
            get { return _tableItemListView; }
            set { _tableItemListView = value; OnPropertyChanged("TableItemListView"); }
        }

        private IEnumerable<T> _tableItemList;
        public IEnumerable<T> TableItemList
        {
            get { return _tableItemList; }
            set { _tableItemList = value; OnPropertyChanged("TableItemList"); }
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        public AsyncCommand RefreshCommand { get; set; }

        public void SaveTableSortOrder()
        {
            if (_tableItemListView == null) return;
            if (_tableItemListView.SortDescriptions.Count <= 0) return;

            if (_sortDescriptionCollection == null)
                _sortDescriptionCollection = new SortDescriptionCollection();

            var sortDescriptionArray = new SortDescription[_tableItemListView.SortDescriptions.Count];
            _tableItemListView.SortDescriptions.CopyTo(sortDescriptionArray, 0);
            
            _sortDescriptionCollection.Clear();
            foreach (var sortDescription in sortDescriptionArray)
                _sortDescriptionCollection.Add(sortDescription);
        }

        public void LoadTableSortOrder()
        {
            if (_tableItemListView == null) return;
            if (_sortDescriptionCollection == null) return;

            _tableItemListView.SortDescriptions.Clear();
            foreach (var sortDescription in _sortDescriptionCollection)
                _tableItemListView.SortDescriptions.Add(sortDescription);
        }

        private SortDescriptionCollection _sortDescriptionCollection;
    }
}
