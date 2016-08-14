using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class DocumentTableViewModel : TableNavigationViewModel<Document>
    {
        //TODO: IsSearched results
        public DocumentTableViewModel()
        {
            InitViewModel();
        }

        private IEnumerable<DocumentType> _filterDocumentTypeList;
        public IEnumerable<DocumentType> FilterDocumentTypeList
        {
            get { return _filterDocumentTypeList; }
            set { _filterDocumentTypeList = value; OnPropertyChanged("FilterDocumentTypeList"); }
        }

        private IEnumerable<Owner> _filterOwnerList;
        public IEnumerable<Owner> FilterOwnerList
        {
            get { return _filterOwnerList; }
            set { _filterOwnerList = value; OnPropertyChanged("FilterOwnerList"); }
        }

        private DocumentRepository _documentRepository;
        private bool _initFilterStatus = false;

        private void InitViewModel()
        {
            _documentRepository = new DocumentRepository();

            AddCommand = new RelayCommand(x => AddMethod());
            EditCommand = new RelayCommand(x => EditMethod());
            DeleteCommand = new RelayCommand(x => DeleteMethod());
            RefreshCommand = new AsyncCommand(x => RefreshMethod());
            RefreshCommand.RunWorkerCompleted += RefreshCommand_RunWorkerCompleted;

            ComplexFilter = new DocumentFilter();
        }

        private void InitFilter()
        {
            var ownerRepository = new OwnerRepository();
            FilterOwnerList = ownerRepository.GetAllOrdered();
            var documentTypeRepository = new Repository<DocumentType>();
            FilterDocumentTypeList = documentTypeRepository.GetAll();

            _initFilterStatus = true;
        }

        private void RefreshCommand_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!_initFilterStatus)
                InitFilter();
            
            SaveTableSortOrder();

            if (!IsSearched)
                TableItemListView = CollectionViewSource.GetDefaultView(TableItemList);
            else
            {
                //Get all units
                var itemList = CollectionViewSource.GetDefaultView(TableItemList);
                var filter = new Predicate<object>(FilterItems);
                itemList.Filter = filter;
                TableItemListView = itemList;
            }

            LoadTableSortOrder();
        }

        private void RefreshMethod()
        {
            TableItemList = ComplexFilterStatus ? 
              _documentRepository.GetAllByComplexFilter(ComplexFilter)
                : _documentRepository.GetAll(x=> x.DocumentNumber.Date, false, false);
        }

        private bool FilterItems(object obj)
        {
            if (!(obj is Document))
                return false;

            var filterString = SearchString;
            var right = (Document)obj;

            if (StringContains(right.DocumentNumber.FullNumber, filterString))
                return true;
            if (StringContains(right.DocumentType.TypeName, filterString))
                return true;
            if (StringContains(right.Owner.Name.DisplayName, filterString))
                return true;
            return StringContains(right.Comments, filterString);
        }

        private void AddMethod()
        {
            AddAction();
        }

        private void EditMethod()
        {
            EditAction();
        }

        private void DeleteMethod()
        {
            var item = SelectedItem as Document;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Все устройства будут удалены.";
                const MessageBoxButton buttons = MessageBoxButton.OKCancel;

                if (MessageBox.Show(text, caption, buttons) == MessageBoxResult.OK)
                {
                    DeleteDocument(item);
                    if (RefreshCommand != null)
                        RefreshCommand.Execute(null);
                }
            }
        }

        private void DeleteDocument(Document item)
        {
            _documentRepository.Delete(item);
        }
    }
}
