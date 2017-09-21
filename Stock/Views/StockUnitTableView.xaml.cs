﻿using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Windows.Controls;
using Stock.Core.Domain;
using Stock.UI.Views.Dialogs;

namespace Stock.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для ResourceFlowWindow.xaml
    /// </summary>
    public partial class StockUnitTableView : ITableView
    {
        public StockUnitTableView()
        {
            InitializeComponent();
            SetActions();
        }

        public void Refresh()
        {
            if (RefreshButton.Command.CanExecute(null))
                RefreshButton.Command.Execute(null);

            if (!_settingsLoaded)
                LoadSettings();
            else
            {
                SaveSettings();
            }
        }

        private bool _settingsLoaded = false;

        private void SetActions()
        {
            if (ViewModel.AddAction == null)
                ViewModel.AddAction = DisplayAddDialog;
            if (ViewModel.EditAction == null)
                ViewModel.EditAction = DisplayEditDialog;
            if (ViewModel.CopyAction == null)
                ViewModel.CopyAction = DisplayCopyDialog;
            if (ViewModel.ShowCardAction == null)
                ViewModel.ShowCardAction = ShowCard;
            if (ViewModel.ShowInfoMessage == null)
                ViewModel.ShowInfoMessage = (text, caption) => MessageBox.Show(text, caption);
            if (ViewModel.ExportAction == null)
                ViewModel.ExportAction = ExportStockUnit;
            if (ViewModel.ShowDialogMessage == null)
            {
                const MessageBoxButton buttons = MessageBoxButton.OKCancel;
                const MessageBoxResult result = MessageBoxResult.OK;
                ViewModel.ShowDialogMessage =
                    (text, caption) => MessageBox.Show(text, caption, buttons) == result;
            }
        }

        private void ExportStockUnit(StockUnit stockUnit)
        {
            var exportDialog = new ExportView(stockUnit) { Owner = Window.GetWindow(this) };
            exportDialog.ShowDialog();
        }

        private void ShowCard(Card card)
        {
            var dialog = new CardAddView(card.Id) { Owner = Window.GetWindow(this) };
            dialog.Closed += (s, e) => dialog.Owner.Focus();
            dialog.Show();
        }

        private void DisplayCopyDialog(StockUnit stockUnit)
        {
            var dialog = new StockUnitAddView(stockUnit) { Owner = Window.GetWindow(this) };
            dialog.Closed += (s, e) => dialog.Owner.Focus();
            dialog.Show();
        }

        private void DisplayAddDialog()
        {
            var dialog = new StockUnitAddView { Owner = Window.GetWindow(this) };
            dialog.Closed += (s, e) => dialog.Owner.Focus();
            dialog.Show();
        }

        private void DisplayEditDialog()
        {
            var item = ViewModel.SelectedItem as StockUnit;
            if (item != null)
            {
                var dialog = new StockUnitAddView(item) { Owner = Window.GetWindow(this) };
                dialog.Closed += (s, e) => dialog.Owner.Focus();
                dialog.Show();
            }
        }

        private void MainDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var element = e.MouseDevice.DirectlyOver as FrameworkElement;
            if (element != null && element.Parent is DataGridCell)
            {
                var datagrid = sender as DataGrid;
                if (datagrid != null)
                    DisplayEditDialog();
            }
        }
        
        private void FilterButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (FilterGridColumn.Width.IsAbsolute && Math.Abs(FilterGridColumn.Width.Value) <= 0.0)
                FilterGridColumn.Width = new GridLength(1, GridUnitType.Auto);
            else
            {
                FilterGridColumn.Width = new GridLength(0, GridUnitType.Pixel);
            }
        }

        private void DataGrid_OnCopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            //e.ClipboardRowContent.Clear();
            //var cells = DataGrid.SelectedCells;
        }

        private void LoadSettings()
        {
            for (int i = 0; i < DataGrid.Columns.Count; i++)
            {
                var columnWidth = string.Format("StockUnitTableColumn{0}Width", i);
                DataGrid.Columns[i].Width =
                    new DataGridLength(AppSettings.GetAsDouble(columnWidth));

                var columnDisplayIndex = string.Format("StockUnitTableColumn{0}DisplayIndex", i);
                DataGrid.Columns[i].DisplayIndex = AppSettings.GetAsInt(columnDisplayIndex);
            }

            _settingsLoaded = true;
        }

        private void SaveSettings()
        {
            for (int i = 0; i < DataGrid.Columns.Count; i++)
            {
                var columnWidth = string.Format("StockUnitTableColumn{0}Width", i);
                AppSettings.SetValue(columnWidth, DataGrid.Columns[i].Width.Value);

                var columnDisplayIndex = string.Format("StockUnitTableColumn{0}DisplayIndex", i);
                if (DataGrid.Columns[i].DisplayIndex != -1)
                    AppSettings.SetValue(columnDisplayIndex, DataGrid.Columns[i].DisplayIndex);
            }

            AppSettings.Save();
        }

        private void StockUnitTableView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Closing += (s, j) => SaveSettings();
        }
    }
}
