using System.Collections.Generic;
using System.ComponentModel;
using Stock.Core.Domain;

namespace Stock.UI.ViewModels.Base
{
    interface ITableBaseViewModel<T> where T : EntityBase
    {
        ICollectionView TableItemListView { get; set; }
        IEnumerable<T> TableItemList { get; set; }
        object SelectedItem { get; set; }

        AsyncCommand RefreshCommand { get; set; }
    }
}
