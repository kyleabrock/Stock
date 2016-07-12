using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;
using System.Windows;
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
            InitViewModel(_stockUnitRepository.GetById(arg.Id, true));
        }

        public void RemoveUnit()
        {
            RemoveMethod(false);
        }

        private StockUnit _stockUnit;
        public StockUnit StockUnit
        {
            get
            {
                return _stockUnit;
            }
            set
            {
                _stockUnit = value;
                OnPropertyChanged("StockUnit");
            }
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
        public Action CloseAction { get; set; }
        public Action ShowReportsAction { get; set; }

        public ObservableCollection<Document> DocumnetsList { get; set; }
        public ObservableCollection<Repair> RepairList { get; set; }
        public ObservableCollection<StockUnitNote> StockUnitNoteList { get; set; }

        public IEnumerable<string> ManufactureList { get; set; }
        public IEnumerable<string> ModelList { get; set; }

        private readonly StockUnitRepository _stockUnitRepository;
        private readonly StockUnitNoteRepository _stockUnitNoteRepository;
        private readonly List<Unit> _itemsToDelete = new List<Unit>();

        private void InitViewModel(StockUnit stockUnit)
        {
            StockUnit = stockUnit;
            if (StockUnit.IsNew)
                StockUnit.UnitList = new List<Unit>();

            InitLists();
            UnitList = new ObservableCollection<Unit>(StockUnit.UnitList);
            UnitList.CollectionChanged += UnitList_CollectionChanged;

            var documentRepository = new DocumentRepository();
            DocumnetsList = new ObservableCollection<Document>(documentRepository.GetByStockUnit(StockUnit));
            var repairRepository = new RepairRepository();
            RepairList = new ObservableCollection<Repair>(repairRepository.GetAllByStockUnit(StockUnit));
            
            StockUnitNoteList = !StockUnit.IsNew ? 
                new ObservableCollection<StockUnitNote>(_stockUnitNoteRepository.GetByStockUnitId(StockUnit)) 
                : new ObservableCollection<StockUnitNote>();

            var repository = new UnitRepository();
            ManufactureList = repository.GetManufactureList();
            ModelList = repository.GetModelList();

            AddUnitCommand = new RelayCommand(x => AddMethod());
            RemoveUnitCommand = new RelayCommand(x => RemoveMethod());
            ReportsCommand = new RelayCommand(x => ReportsMethod());
            SaveCommand = new RelayCommand(x => SaveMethod());
            CloseCommand = new RelayCommand(x => CloseMethod());
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

        private void AddMethod()
        {
            var unit = new Unit();
            UnitList.Add(unit);
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

            var user = ApplicationState.GetValue<UserAcc>("User");
            ILogFactory logFactory = new LogFactory();
            var logEntity = logFactory.CreateLogMessage(user, StockUnit);
            var repository = new Repository<Log>();
            repository.Save(logEntity);

            CloseAction();
        }

        private bool CheckValues()
        {
            if (StockUnit.CreationDate.Year < SqlDateTime.MinValue.Value.Year)
            {
                MessageBox.Show("Дата создания не может быть меньше 1 января 1753 г.");
                return false;
            }
            if (string.IsNullOrEmpty(StockUnit.StockNumber))
            {
                MessageBox.Show("Укажите инвентарный номер (в случае отсутствия укажите б/н)");
                return false;
            }
            if (StockUnit.Owner == null)
            {
                MessageBox.Show("Укажите ответственное лицо");
                return false;
            }
            foreach (var unit in UnitList)
            {
                if (unit.UnitType == null)
                {
                    MessageBox.Show("Укажите тип устройства для всех записей");
                    return false;
                }
                if (string.IsNullOrEmpty(unit.Manufacture) && string.IsNullOrEmpty(unit.ModelName) && string.IsNullOrEmpty(unit.Serial))
                {
                    MessageBox.Show("Добавлено пустое устройство (отсутсвуют производитель, модель, сер. №)");
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
                MessageBox.Show("Не найдена карточка по-умолчанию.\r\nСоздайте карточку и повторите попытку.");
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
