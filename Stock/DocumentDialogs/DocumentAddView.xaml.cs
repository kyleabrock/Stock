using System;
using System.Collections.ObjectModel;
using System.Windows;
using Core.Domain;

namespace Stock.DocumentDialogs
{
	/// <summary>
    /// Interaction logic for DocumentAddView.xaml
	/// </summary>
	public partial class DocumentAddView
	{
		public DocumentAddView()
		{
		    InitializeComponent();

            SetDefaultValues();
            SetActions();
		}

        public DocumentAddView(Document arg)
	    {
            InitializeComponent();

            ViewModel = new DocumentAddViewModel(arg);
            DataContext = ViewModel;
            SetActions();
	    }

	    private void SetDefaultValues()
	    {
	        if (OwnerCb.Items.Count > 0) OwnerCb.SelectedIndex = 0;
	        DocumentDateDtPk.Value = DateTime.Now;
	    }
        
        private void SetActions()
        {
            if (ViewModel.CloseAction == null)
                ViewModel.CloseAction = Close;
        }

	    private void StockUnitAddButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        var dialog = new StockUnitSearchDialog();
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