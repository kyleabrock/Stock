using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Factory;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class StockUnitViewModel : ViewModelBase
    {
        public StockUnitViewModel()
        {
            _stockUnitRepository = new StockUnitRepository();
            _stockUnitNoteRepository = new StockUnitNoteRepository();
            InitViewModel(new StockUnit());
        }

        public StockUnitViewModel(StockUnit arg)
        {
            _stockUnitRepository = new StockUnitRepository();
            _stockUnitNoteRepository = new StockUnitNoteRepository();

            InitViewModel(!arg.IsNew ? _stockUnitRepository.GetById(arg.Id, true) : arg);
        }

        public void RemoveUnit()
        {
            RemoveMethod(false);
        }

        private StockUnit _stockUnit;
        public StockUnit StockUnit
        {
            get { return _stockUnit; }
            set { _stockUnit = value; OnPropertyChanged("StockUnit"); }
        }

        private ObservableCollection<Unit> _unitList;
        public ObservableCollection<Unit> UnitList
        {
            get { return _unitList; }
            set
            {
                _unitList = value;
                OnPropertyChanged("UnitList");
            }
        }

        private ObservableCollection<StockUnitFile> _stockUnitFiles;
        public ObservableCollection<StockUnitFile> StockUnitFiles
        {
            get { return _stockUnitFiles; }
            set { _stockUnitFiles = value; OnPropertyChanged("StockUnitFiles"); }
        }

        private object _selectedFileItem;
        public object SelectedFileItem
        {
            get
            {
                return _selectedFileItem;
            }
            set
            {
                _selectedFileItem = value;
                OnPropertyChanged("SelectedFileItem");
            }
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public ObservableCollection<UnitType> UnitTypes { get; private set; }
        public ObservableCollection<Owner> OwnerList { get; private set; }
        public ObservableCollection<Status> StatusList { get; private set; }
        public ICommand AddUnitCommand { get; set; }
        public ICommand RemoveUnitCommand { get; set; }
        public ICommand ReportsCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand AddFileCommand { get; set; }
        public ICommand RemoveFileCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand OpenFolderCommand { get; set; }
        public Action CloseAction { get; set; }
        public Action ShowReportsAction { get; set; }
        public Func<string> ChooseFileFunc { get; set; }
        public Action<string, string> ShowInfoMessage { get; set; }

        public ObservableCollection<Document> DocumnetsList { get; set; }
        public ObservableCollection<Repair> RepairList { get; set; }
        public ObservableCollection<StockUnitNote> StockUnitNoteList { get; set; }

        public IEnumerable<string> ManufactureList { get; set; }
        public IEnumerable<string> ModelList { get; set; }

        private readonly StockUnitRepository _stockUnitRepository;
        private readonly StockUnitNoteRepository _stockUnitNoteRepository;
        private List<Unit> _itemsToDelete;
        private List<string> _addedFiles;
        private List<string> _deletedFiles;
        private List<StockUnitFile> _deletedStockUnitFiles;

        private void InitViewModel(StockUnit stockUnit)
        {
            _itemsToDelete = new List<Unit>();
            _addedFiles = new List<string>();
            _deletedFiles = new List<string>();
            _deletedStockUnitFiles = new List<StockUnitFile>();

            StockUnit = stockUnit;
            if (StockUnit.IsNew && StockUnit.UnitList == null)
                StockUnit.UnitList = new List<Unit>();

            InitLists();
            UnitList = new ObservableCollection<Unit>(StockUnit.UnitList);
            UnitList.CollectionChanged += UnitList_CollectionChanged;

            var documentRepository = new DocumentRepository();
            DocumnetsList = new ObservableCollection<Document>(documentRepository.GetByStockUnit(StockUnit));
            var repairRepository = new RepairRepository();
            RepairList = new ObservableCollection<Repair>(repairRepository.GetAllByStockUnit(StockUnit));
            var filesRepository = new StockUnitFileRepository();
            StockUnitFiles = new ObservableCollection<StockUnitFile>(filesRepository.GetByStockUnitId(StockUnit));
            
            StockUnitNoteList = !StockUnit.IsNew ? 
                new ObservableCollection<StockUnitNote>(_stockUnitNoteRepository.GetByStockUnitId(StockUnit)) 
                : new ObservableCollection<StockUnitNote>();

            var repository = new UnitRepository();
            ManufactureList = repository.GetManufactureList();
            ModelList = repository.GetModelList();

            AddUnitCommand = new RelayCommand(x => UnitList.Add(new Unit()));
            RemoveUnitCommand = new RelayCommand(x => RemoveMethod());
            ReportsCommand = new RelayCommand(x => ReportsMethod());
            SaveCommand = new RelayCommand(x => SaveMethod());
            CloseCommand = new RelayCommand(x => CloseMethod());
            AddFileCommand = new RelayCommand(x => AddFileMethod());
            RemoveFileCommand = new RelayCommand(x => RemoveFileMethod());
            OpenFileCommand = new RelayCommand(x => OpenFileMethod());
            OpenFolderCommand = new RelayCommand(x => OpenFolderMethod());
        }

        private void OpenFileMethod()
        {
            var item = SelectedFileItem as StockUnitFile;
            if (item == null)
            {
                ShowInfoMessage("Ошибка", "Не выбран файл");
                return;
            }
            if (item.IsNew)
            {
                ShowInfoMessage("Ошибка", "Файл не сохранен в системе");
                return;
            }
            
            var path = GetFileFolderPath() + item.FileName;
            if (!File.Exists(path))
            {
                ShowInfoMessage("Ошибка", "Файл не найден");
                return;
            }
            
            Process.Start(path);
        }

        private void OpenFolderMethod()
        {
            var item = SelectedFileItem as StockUnitFile;
            if (item == null)
            {
                ShowInfoMessage("Ошибка", "Не выбран файл");
                return;
            }
            if (item.IsNew)
            {
                ShowInfoMessage("Ошибка", "Файл не сохранен в системе");
                return;
            }

            var path = GetFileFolderPath() + item.FileName;
            if (!File.Exists(path))
            {
                ShowInfoMessage("Ошибка", "Файл не найден");
                return;
            }

            Process.Start("explorer.exe", "/select,\"" + path + "\"");
        }

        private void InitLists()
        {
            var unitTypeRepository = new UnitTypeRepository();
            UnitTypes = new ObservableCollection<UnitType>(unitTypeRepository.GetAllOrdered());
            
            var ownerRepository = new OwnerRepository();
            OwnerList = new ObservableCollection<Owner>(ownerRepository.GetAllOrdered());

            var statusRepository = new StatusRepository();
            StatusList = new ObservableCollection<Status>(statusRepository.GetAllOrdered());
        }

        private void RemoveMethod(bool removeFromCollection = true)
        {
            var item = SelectedItem as Unit;
            if (item == null) return;
            
            if (!item.IsNew)
                _itemsToDelete.Add(item);

            if (removeFromCollection)
                UnitList.Remove(item);
        }

        private void ReportsMethod()
        {
            if (StockUnit.IsNew) return;
            ShowReportsAction();
        }

        private void AddFileMethod()
        {
            var file = ChooseFileFunc();
            if (string.IsNullOrEmpty(file)) return;

            var fileName = Path.GetFileName(file);
            _addedFiles.Add(file);
            var stockUnitFile = new StockUnitFile { StockUnit = StockUnit, FileName = fileName };
            StockUnitFiles.Add(stockUnitFile);
        }

        private void RemoveFileMethod()
        {
            var item = SelectedFileItem as StockUnitFile;
            if (item == null) return;

            if (item.IsNew)
            {
                var fileItem = _addedFiles.Find(x => x.Contains(item.FileName));
                _addedFiles.Remove(fileItem);
            }
            else
            {
                var fileItem = GetFileFolderPath() + item.FileName;
                _deletedFiles.Add(fileItem);
            }

            _deletedStockUnitFiles.Add(item);
            StockUnitFiles.Remove(item);
        }

        private bool SaveStockUnitFiles()
        {
            var destFolder = GetFileFolderPath();
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            foreach (var file in _addedFiles)
            {
                var fileName = Path.GetFileName(file);
                var destFileName = destFolder + fileName;
                try
                {
                    //BUG: Show Message overwrite or not
                    File.Copy(file, destFileName, true);
                }
                catch (Exception ex)
                {
                    const string caption = "Ошибка";
                    var text = "Ошибка при копировании файла.\r\nПодробные сведения об ошибке: " + ex.Message;
                    ShowInfoMessage(text, caption);
                    
                    return false;
                }
            }

            foreach (var file in _deletedFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    const string caption = "Ошибка";
                    var text = "Ошибка при удалении файла.\r\nПодробные сведения об ошибке: " + ex.Message;
                    ShowInfoMessage(text, caption);

                    return false;
                }
            }
            
            return true;
        }

        //BUG: Make this function more crear
        private string GetFileFolderPath()
        {
            var filePath = ApplicationState.GetValue<string>("StockUnitFilesFolder");
            if (!filePath.EndsWith("\\"))
                filePath += "\\";

            if (!StockUnit.IsNew && !string.IsNullOrEmpty(StockUnit.StockNumber))
            {
                filePath += StockUnit.StockNumber + "\\";
                return filePath;
            }
            return "";
        }

        private void SaveMethod()
        {
            StockUnit.UnitList = UnitList;
            if (!SetCard()) return;

            foreach (var unit in UnitList)
                unit.StockUnit = StockUnit;

            if (!CheckValues()) return;

            _stockUnitRepository.Save(StockUnit);
            
            var unitRepository = new UnitRepository();
            unitRepository.Save(UnitList);
            unitRepository.Delete(_itemsToDelete);

            foreach (var note in StockUnitNoteList)
                note.StockUnit = StockUnit;
            _stockUnitNoteRepository.Save(StockUnitNoteList);
            
            var fileRepository = new StockUnitFileRepository();
            if (!SaveStockUnitFiles()) return;
            fileRepository.Save(StockUnitFiles);
            fileRepository.Delete(_deletedStockUnitFiles);

            var user = ApplicationState.GetValue<UserAcc>("User");
            ILogFactory logFactory = new LogFactory();
            var logEntity = logFactory.CreateMessage(user, StockUnit);
            var repository = new Repository<Log>();
            repository.Save(logEntity);

            CloseAction();
        }

        private bool CheckValues()
        {
            if (StockUnit.CreationDate.Year < SqlDateTime.MinValue.Value.Year)
            {
                ShowInfoMessage("Дата создания не может быть меньше 1 января 1753 г.", "Ошибка");
                return false;
            }
            if (string.IsNullOrEmpty(StockUnit.StockNumber))
            {
                ShowInfoMessage("Укажите инвентарный номер (в случае отсутствия укажите б/н)", "Ошибка");
                return false;
            }
            if (StockUnit.Owner == null)
            {
                ShowInfoMessage("Укажите ответственное лицо", "Ошибка");
                return false;
            }
            foreach (var unit in UnitList)
            {
                if (unit.UnitType == null)
                {
                    ShowInfoMessage("Укажите тип устройства для всех записей", "Ошибка");
                    return false;
                }
                if (string.IsNullOrEmpty(unit.Manufacture) && string.IsNullOrEmpty(unit.ModelName) && string.IsNullOrEmpty(unit.Serial))
                {
                    ShowInfoMessage("Добавлено пустое устройство (отсутсвуют производитель, модель, сер. №)", "Ошибка");
                    return false;
                }
            }
            return true;
        }

        private bool SetCard()
        {
            if (!_stockUnit.IsNew) return true;

            var cardRepository = new CardRepository();
            var defaultCard = cardRepository.GetDefaultCard();
            if (!defaultCard.IsNew)
                _stockUnit.Card = defaultCard;
            else
            {
                ShowInfoMessage("Не найдена карточка по-умолчанию.\r\nСоздайте карточку и повторите попытку.", "Ошибка");
                return false;
            }
            return true;
        }

        private void CloseMethod()
        {
            CloseAction();
        }

        private void UnitList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems == null) return;
            
            foreach (var item in e.OldItems.OfType<Unit>())
                _itemsToDelete.Add(item);
        }
    }
}
