﻿using Stock.Core.Domain;
using Stock.UI.ViewModels.Dialogs;

namespace Stock.UI.Views.Dialogs
{
	/// <summary>
	/// Interaction logic for PrinterAddDialog.xaml
	/// </summary>
	public partial class StatusAddView
	{
		public StatusAddView()
		{
			InitializeComponent();
            SetDefaultValues();
            SetActions();
		}

        public StatusAddView(Status arg)
	    {
	        InitializeComponent();
            ViewModel = new StatusAddViewModel(arg);
            DataContext = ViewModel;
            SetActions();
	    }

	    private void SetDefaultValues()
	    {
            StatusTypeCb.SelectedIndex = 0;
            StatusNameTb.Focus();
	    }

        private void SetActions()
        {
            if (ViewModel.CloseAction == null)
                ViewModel.CloseAction = Close;
        }
	}
}