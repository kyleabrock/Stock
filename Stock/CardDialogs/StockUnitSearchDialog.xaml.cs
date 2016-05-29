using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Core.Domain;
using Core.Repository;

namespace Stock.CardDialogs
{
	/// <summary>
	/// Interaction logic for PrinterAddDialog.xaml
	/// </summary>
	public partial class StockUnitSearchDialog
	{
        public StockUnitSearchDialog()
		{
			InitializeComponent();
            _repository = new StockUnitRepository();
            _card = new Card();
            InitDialog();
		}

	    public StockUnitSearchDialog(Card card)
	    {
            InitializeComponent();
            _repository = new StockUnitRepository();
            _card = card;
            InitDialog();
	    }

        public IList<StockUnit> Result { get; set; }

	    private readonly Card _card;
        private readonly StockUnitRepository _repository;
        private List<StockUnit> _fullList;
	    private int _minStringLength;

        private void InitDialog()
        {
            _minStringLength = 1;
            MainDataGridRefresh();
            SearchTb.Focus();
        }

        private void MainDataGridRefresh()
        {
            if (_card.IsNew)
                _fullList = new List<StockUnit>(_repository.GetFromDefaultCard());
            else
            {
                if (_card.StockUnitList != null)
                    _fullList = new List<StockUnit>(_card.StockUnitList);
            }

            MainDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = _fullList;
        }

        private void SaveSelected()
        {
            Result = new List<StockUnit>();
            var selectedItems = MainDataGrid.SelectedItems;
            if (selectedItems.Count > 0)
            {
                foreach (var item in selectedItems)
                {
                    var result = item as StockUnit;
                    Result.Add(result);
                }
            }
        }

        private void Filter()
        {
            var itemSourceList = new CollectionViewSource { Source = _fullList };
            ICollectionView itemList = itemSourceList.View;
            var filter = new Predicate<object>(StockUnitFilter);
            itemList.Filter = filter;

            MainDataGrid.ItemsSource = itemList;
        }

        private bool StockUnitFilter(object obj)
        {
            if (!(obj is StockUnit))
                return false;

            var filterString = SearchTb.Text;
            var right = (StockUnit)obj;

            var culture = CultureInfo.GetCultureInfo("ru-RU");

            var containsCartridgeName =
                culture.CompareInfo.IndexOf(right.StockNumber, filterString, CompareOptions.IgnoreCase) >= 0;
            if (containsCartridgeName)
                return true;
            
            var containsPrinterName =
                culture.CompareInfo.IndexOf(right.StockName, filterString, CompareOptions.IgnoreCase) >= 0;
            return containsPrinterName;
        }

        private void OkBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SaveSelected();
            Close();
	    }

	    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
	    {
	        Close();
	    }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchTb.Text))
            {
                if (SearchTb.Text.Length > _minStringLength)
                    Filter();
            }
            else
            {
                MainDataGridRefresh();
            }
        }

	    private void SearchTb_OnPreviewKeyDown(object sender, KeyEventArgs e)
	    {
	        if (e.Key == Key.Up)
	        {
	            if (MainDataGrid.SelectedIndex != 0)
	                MainDataGrid.SelectedIndex -= 1;
	        }
	        if (e.Key == Key.Down)
	        {
                if (MainDataGrid.SelectedIndex != MainDataGrid.Items.Count - 1)
                    MainDataGrid.SelectedIndex += 1;
	        }
	    }
	}
}