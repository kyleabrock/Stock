using System;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.UI.ViewModels.Dialogs;

namespace Stock.UI.Views.Dialogs
{
	/// <summary>
	/// Interaction logic for PrinterAddDialog.xaml
	/// </summary>
	public partial class StockUnitAddView
	{
		public StockUnitAddView()
		{
		    InitializeComponent();
            SetDefaultValues();
            SetActions();

            StockNumberTb.Focus();
		}

        public StockUnitAddView(StockUnit arg)
	    {
            InitializeComponent();
            ViewModel = new StockUnitViewModel(arg);
            DataContext = ViewModel;
            SetActions();

            StockNumberTb.Focus();
	    }

        private void SetDefaultValues()
        {
            if (StatusCb.Items.Count > 0) StatusCb.SelectedIndex = 0;
            if (OwnerCb.Items.Count > 0) OwnerCb.SelectedIndex = 0;
            CreationDateDtPk.Value = DateTime.Now;
        }

	    private void SetActions()
	    {
            if (ViewModel.CloseAction == null)
                ViewModel.CloseAction = Close;
	        if (ViewModel.ShowReportsAction == null)
	            ViewModel.ShowReportsAction = ShowReports;
	    }

        private void ShowReports()
        {
            var dialog = new ReportSelectView(ViewModel.StockUnit) { Owner = System.Windows.Window.GetWindow(this) };
            dialog.ShowDialog();
        }

        private void MainDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Delete)
            //    ViewModel.RemoveUnit();
        }
	}
}