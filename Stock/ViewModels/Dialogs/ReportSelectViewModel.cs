using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Report;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class ReportSelectViewModel : ViewModelBase
    {
        public ReportSelectViewModel(EntityBase arg)
        {
            _entityBase = arg;
            if (arg is StockUnit)
                _report = new StockUnitReport();
            
            InitViewModel();
        }

        public ObservableCollection<string> ReportList
        {
            get { return _reportList; }
            set { _reportList = value; OnPropertyChanged("ReportList"); }
        }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        public bool OpenFolderAfterExport
        {
            get { return _openFolderAfterExport; }
            set { _openFolderAfterExport = value; OnPropertyChanged("OpenFolderAfterExport"); }
        }

        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public Action CloseAction { get; set; }

        private ObservableCollection<string> _reportList;
        private object _selectedItem;
        private bool _openFolderAfterExport;
        private readonly IReport _report;
        private readonly EntityBase _entityBase;

        private void InitViewModel()
        {
            var templatesPath = ApplicationState.GetValue<string>("TemplatesFolderPath");
            ReportList = new ObservableCollection<string>(_report.GetTemplates(templatesPath));
            
            OkCommand = new RelayCommand(x => OkMethod());
            CancelCommand = new RelayCommand(x => CancelMethod());
        }

        private void OkMethod()
        {
            var selectedTemplate = _selectedItem as string;
            if (!string.IsNullOrEmpty(selectedTemplate))
            {
                var exportDirectoryPath = ApplicationState.GetValue<string>("ExportFolderPath");
                var result = _report.Export(_entityBase, selectedTemplate, exportDirectoryPath);
                if (result)
                {
                    MessageBox.Show("Отчет выгружен");
                    if (OpenFolderAfterExport)
                        Process.Start("explorer.exe", string.Format("/select,\"{0}\"", _report.LastExportedFileName));
                    CloseAction();
                }
                else
                {
                    MessageBox.Show(_report.LastError);
                }
            }
        }

        private void CancelMethod()
        {
            CloseAction();
        }
    }
}
