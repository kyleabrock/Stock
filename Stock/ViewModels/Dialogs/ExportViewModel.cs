using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Report;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class ExportViewModel : ViewModelBase
    {
        public ExportViewModel(StockUnit stockUnit)
        {
            Items = new ObservableCollection<string>();
            SetTemplates();

            OkCommand = new RelayCommand(x => ExportMethod());
            CancelCommand = new RelayCommand(x => CloseAction());

            _stockUnit = stockUnit;
        }

        private void SetTemplates()
        {
            var templatesPath = Properties.Settings.Default.TemplatesFolderPath + "StockUnit\\";
            if (!Directory.Exists(templatesPath))
            {
                ShowInfoMessage(string.Format("Директория {0} не существует", templatesPath));
                return;
            }

            Items.Clear();
            var files = Directory.GetFiles(templatesPath, "*.docx");
            foreach (var file in files)
                Items.Add(file);

            if (Items.Count > 0)
                SelectedItem = Items[0];
        }

        public ObservableCollection<string> Items { get; set; }
        public object SelectedItem { get; set; }
        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public Action CloseAction { get; set; }
        public Action<string> ShowInfoMessage { get; set; }
        public Func<string> SelectSaveToFile { get; set; }

        private readonly StockUnit _stockUnit;

        private void ExportMethod()
        {
            if (SelectedItem == null)
            {
                ShowInfoMessage("Не выбран шаблон");
                return;
            }

            if (_stockUnit == null)
            {
                ShowInfoMessage("Ошибка! Экспортируемый элемент БД не выбран!");
                return;
            }

            var outPath = SelectSaveToFile();
            if (outPath == String.Empty) return;
            
            var templatePath = SelectedItem.ToString();
            var export = new StockUnitBaseReport();
            export.Export(_stockUnit, templatePath, outPath);
            
            CloseAction();
        }
    }
}
