using System;
using System.Windows;
using Microsoft.Win32;
using Stock.Core.Domain;
using Stock.UI.ViewModels.Dialogs;

namespace Stock.UI.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ExportView.xaml
    /// </summary>
    public partial class ExportView
    {
        public ExportView(StockUnit stockUnit)
        {
            InitializeComponent();

            var viewModel = new ExportViewModel(stockUnit);
            if (viewModel.CloseAction == null)
                viewModel.CloseAction = Close;
            if (viewModel.ShowInfoMessage == null)
                viewModel.ShowInfoMessage = s => MessageBox.Show(s);
            if (viewModel.SelectSaveToFile == null)
                viewModel.SelectSaveToFile = SelectSaveToFile;

            DataContext = viewModel;
        }

        private string SelectSaveToFile()
        {
            var dlg = new SaveFileDialog
                {
                    DefaultExt = ".docx",
                    Filter = "Документ Word (.docx)|*.docx"
                };

            var result = dlg.ShowDialog();
            if (result == true)
                return dlg.FileName;
            return String.Empty;
        }
    }
}
