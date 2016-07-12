﻿using System.Windows;
using System.Windows.Input;
using Microsoft.Windows.Controls;
using Stock.Core.Domain;
using Stock.UI.Views.Dialogs;

namespace Stock.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для ResourceFlowWindow.xaml
    /// </summary>
    public partial class StaffTableView
    {
        public StaffTableView()
        {
            InitializeComponent();
            SetActions();
        }

        public void Refresh()
        {
            if (RefreshButton.Command.CanExecute(null))
                RefreshButton.Command.Execute(null);
        }

        private void SetActions()
        {
            if (ViewModel.AddAction == null)
                ViewModel.AddAction = DisplayAddDialog;
            if (ViewModel.EditAction == null)
                ViewModel.EditAction = DisplayEditDialog;
        }

        private void DisplayAddDialog()
        {
            var dialog = new StaffAddView { Owner = Window.GetWindow(this) };
            dialog.Closed += (s, e) => dialog.Owner.Focus();
            dialog.Show();
        }

        private void DisplayEditDialog()
        {
            var item = ViewModel.SelectedItem as Staff;
            if (item != null)
            {
                var dialog = new StaffAddView(item) { Owner = Window.GetWindow(this) };
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
    }
}
    