using System;
using System.Collections.ObjectModel;
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
            if (ViewModel.AddFunc == null)
                ViewModel.AddFunc = AddFunc;
        }

	    private ObservableCollection<StockUnit> AddFunc(Card card)
	    {
            var dialog = new StockUnitSearchView(card) { Owner = GetWindow(this) };
	        
            var items = new ObservableCollection<StockUnit>();
            dialog.Closed += (s, j) =>
            {
                if (dialog.Result != null)
                    items = new ObservableCollection<StockUnit>(dialog.Result);
            };

	        dialog.ShowDialog();
	        return items;
	    }
	}
}