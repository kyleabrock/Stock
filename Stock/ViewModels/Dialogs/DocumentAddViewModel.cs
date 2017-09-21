﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Windows;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Factory;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class DocumentAddViewModel : ViewModelBase
    {
        public DocumentAddViewModel()
        {
            _documentRepository = new DocumentRepository();
            InitViewModel(new Document());
        }

        public DocumentAddViewModel(Document arg)
        {
            _documentRepository = new DocumentRepository();
            InitViewModel(_documentRepository.GetById(arg.Id, true));
        }

        public void AddStockUnit()
        {
            foreach (var item in NewStockUnitList)
            {
                if (!StockUnitList.Contains(item))
                    StockUnitList.Add(item);
            }
            
            NewStockUnitList = new ObservableCollection<StockUnit>();
        }

        private Document _document;
        public Document Document
        {
            get { return _document; }
            set 
            { 
                _document = value;
                OnPropertyChanged("Document"); 
            }
        }

        private ObservableCollection<StockUnit> _stockUnitList;
        public ObservableCollection<StockUnit> StockUnitList
        {
            get { return _stockUnitList; }
            set
            {
                _stockUnitList = value; 
                OnPropertyChanged("StockUnitList");
            }
        }

        private ObservableCollection<StockUnit> _newStockUnitList;
        public ObservableCollection<StockUnit> NewStockUnitList
        {
            get { return _newStockUnitList; }
            set
            {
                _newStockUnitList = value;
                OnPropertyChanged("NewStockUnitList");
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

        public ObservableCollection<Owner> OwnerList { get; private set; }
        public ObservableCollection<DocumentType> DocumentTypes { get; private set; }
        public ICommand RemoveStockUnitCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public Action CloseAction { get; set; }

        private readonly DocumentRepository _documentRepository;

        private void InitViewModel(Document document)
        {
            Document = document;
            if (Document.IsNew)
                Document.StockUnitList = new List<StockUnit>();

            InitLists();
            StockUnitList = new ObservableCollection<StockUnit>(Document.StockUnitList);

            RemoveStockUnitCommand = new RelayCommand(x => RemoveMethod());
            SaveCommand = new RelayCommand(x => SaveMethod());
            CloseCommand = new RelayCommand(x => CloseMethod());
        }

        private void InitLists()
        {
            var ownerRepository = new OwnerRepository();
            var ownerList = ownerRepository.GetAll(x => x.Name.DisplayName);
            OwnerList = new ObservableCollection<Owner>(ownerList);

            IRepository<DocumentType> documentTypeRepository = new Repository<DocumentType>();
            var documentTypes = documentTypeRepository.GetAll(x => x.TypeName);
            DocumentTypes = new ObservableCollection<DocumentType>(documentTypes);
        }

        private void RemoveMethod(bool removeFromCollection = true)
        {
            var item = SelectedItem as StockUnit;
            if (item == null) return;

            if (removeFromCollection)
                StockUnitList.Remove(item);
        }

        private void SaveMethod()
        {
            if (!CheckValues()) return;

            Document.StockUnitList = StockUnitList;
            _documentRepository.Save(Document);

            var user = AppSettings.User;
            ILogFactory logFactory = new LogFactory();
            var logEntity = logFactory.CreateMessage(user, Document);
            var repository = new Repository<Log>();
            repository.Save(logEntity);
            
            CloseAction();
        }

        private bool CheckValues()
        {
            if (Document.DocumentNumber.Date.Year < SqlDateTime.MinValue.Value.Year)
            {
                MessageBox.Show("Дата создания не может быть меньше 1 января 1753 г.");
                return false;
            }
            if (string.IsNullOrEmpty(Document.DocumentNumber.Number))
            {
                MessageBox.Show("Укажите номер документа (в случае отсутствия укажите б/н)");
                return false;
            }
            if (Document.Owner == null)
            {
                MessageBox.Show("Укажите ответственное лицо");
                return false;
            }
            return true;
        }

        private void CloseMethod()
        {
            CloseAction();
        }
    }
}
