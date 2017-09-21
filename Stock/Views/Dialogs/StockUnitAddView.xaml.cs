using System;
using System.Windows.Forms;
using Stock.Core.Domain;
using Stock.UI.ViewModels.Dialogs;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

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
	        if (ViewModel.ShowInfoMessage == null)
	            ViewModel.ShowInfoMessage = (text, caption) => MessageBox.Show(text, caption);
            if (ViewModel.ChooseFileFunc == null)
                ViewModel.ChooseFileFunc = ChooseFileFunc;
	    }

	    private string ChooseFileFunc()
	    {
            var dialog = new OpenFileDialog { Multiselect = false, Filter = "Все файлы (*.*)|*.*" };
	        DialogResult result = dialog.ShowDialog();

            return result == System.Windows.Forms.DialogResult.OK ? dialog.FileName : null;
	    }

	    private void ShowReports()
        {
            var dialog = new ReportSelectView(ViewModel.StockUnit) { Owner = System.Windows.Window.GetWindow(this) };
            dialog.ShowDialog();
        }

        private void MainDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.SettingKey == SettingKey.Delete)
            //    ViewModel.RemoveUnit();
        }
	}
}