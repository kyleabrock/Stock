using System;
using System.Collections.ObjectModel;
using System.Windows;
using Core.Domain;

namespace Stock.CardDialogs
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
		}

        public CardAddView(Card arg)
	    {
            InitializeComponent();

            ViewModel = new CardAddViewModel(arg);
            DataContext = ViewModel;
            SetActions();
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
	        var dialog = new StockUnitSearchDialog(ViewModel.DefaultCard);
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