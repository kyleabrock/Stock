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
    public partial class RepairTableView : ITableView
    {
        public RepairTableView()
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
            if (ViewModel.ShowInfoMessage == null)
                ViewModel.ShowInfoMessage = (text, caption) => MessageBox.Show(text, caption);
            if (ViewModel.ShowDialogMessage == null)
            {
                const MessageBoxButton buttons = MessageBoxButton.OKCancel;
                const MessageBoxResult result = MessageBoxResult.OK;
                ViewModel.ShowDialogMessage =
                    (text, caption) => MessageBox.Show(text, caption, buttons) == result;
            }
        }

        private void DisplayAddDialog()
        {
            var dialog = new RepairAddView { Owner = Window.GetWindow(this) };
            dialog.Closed += (s, e) => dialog.Owner.Focus();
            dialog.Show();
        }

        private void DisplayEditDialog()
        {
            var item = ViewModel.SelectedItem as Repair;
            if (item != null)
            {
                var dialog = new RepairAddView(item) { Owner = Window.GetWindow(this) };
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

        private void LoadSettings()
        {
            for (int i = 0; i < DataGrid.Columns.Count; i++)
            {
                var columnWidth = string.Format("RepairTableColumn{0}Width", i);
                DataGrid.Columns[i].Width =
                    new DataGridLength(AppSettings.GetAsDouble(columnWidth));

                var columnDisplayIndex = string.Format("RepairTableColumn{0}DisplayIndex", i);
                DataGrid.Columns[i].DisplayIndex = AppSettings.GetAsInt(columnDisplayIndex);
            }

            _settingsLoaded = true;
        }

        private void SaveSettings()
        {
            for (int i = 0; i < DataGrid.Columns.Count; i++)
            {
                var columnWidth = string.Format("RepairTableColumn{0}Width", i);
                AppSettings.SetValue(columnWidth, DataGrid.Columns[i].Width.Value);

                var columnDisplayIndex = string.Format("RepairTableColumn{0}DisplayIndex", i);
                if (DataGrid.Columns[i].DisplayIndex != -1)
                    AppSettings.SetValue(columnDisplayIndex, DataGrid.Columns[i].DisplayIndex);
            }

            AppSettings.Save();
        }

        private void RepairTableView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Closing += (s, j) => SaveSettings();
        }
    }
}
