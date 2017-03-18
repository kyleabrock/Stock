using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.UI.ViewModels.Dialogs;

namespace Stock.UI.Views.Dialogs
{
	/// <summary>
	/// Interaction logic for PrinterAddDialog.xaml
	/// </summary>
	public partial class StockUnitSearchView
	{
        public StockUnitSearchView()
		{
			InitializeComponent();
            SetActions();
            
            SearchTb.Focus();
		}

        public StockUnitSearchView(Card card)
	    {
            InitializeComponent();

            ViewModel = new StockUnitSearchViewModel(card);
            DataContext = ViewModel;
            SetActions();

            SearchTb.Focus();
	    }

        private void SetActions()
        {
            if (ViewModel.CloseAction == null)
                ViewModel.CloseAction = Close;
            if (ViewModel.GetSelectedItems == null)
                ViewModel.GetSelectedItems = GetSelectedItems;
        }

        public IList<StockUnit> Result { get { return ViewModel.Result; } }

        private ObservableCollection<object> GetSelectedItems()
        {
            if (MainDataGrid.SelectedItems.Count > 0)
            {
                var result = new ObservableCollection<object>();
                foreach (var item in MainDataGrid.SelectedItems)
                    result.Add(item);
                
                return new ObservableCollection<object>(result);
            }
            
            return new ObservableCollection<object>();
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