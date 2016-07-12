using System;
using System.Collections.ObjectModel;
using System.Windows;
using Stock.Core.Domain;
using Stock.UI.ViewModels.Dialogs;

namespace Stock.UI.Views.Dialogs
{
	/// <summary>
	/// Interaction logic for CardAddView.xaml
	/// </summary>
	public partial class CardAddView
	{
		public CardAddView()
		{
		    InitializeComponent();

            SetDefaultValues();
            SetActions();

		    CardNumberTb.Focus();
		}

        public CardAddView(Card arg)
	    {
            InitializeComponent();

            ViewModel = new CardAddViewModel(arg);
            DataContext = ViewModel;
            SetActions();

            CardNumberTb.Focus();
	    }

        public CardAddView(int cardId)
        {
            InitializeComponent();

            ViewModel = new CardAddViewModel(cardId);
            DataContext = ViewModel;
            SetActions();

            CardNumberTb.Focus();
        }

	    private void SetDefaultValues()
	    {
	        if (StaffCb.Items.Count > 0) StaffCb.SelectedIndex = 0;
	        CreationDateDtPk.Value = DateTime.Now;
	    }
        
        private void SetActions()
        {
            if (ViewModel.CloseAction == null)
                ViewModel.CloseAction = Close;
        }

	    private void StockUnitAddButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        var dialog = new StockUnitSearchView(ViewModel.DefaultCard);
	        dialog.Closed += (s, j) =>
	        {
	            if (dialog.Result != null)
	            {
	                ViewModel.NewStockUnitList = new ObservableCollection<StockUnit>(dialog.Result);
                    ViewModel.AddStockUnit();
	            }
	        };

            dialog.ShowDialog();
	    }
	}
}