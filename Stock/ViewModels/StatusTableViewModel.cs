using System;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class StatusTableViewModel : TableNavigationViewModel<Status>
    {
        //TODO: IsSearched results
        public StatusTableViewModel()
        {
            InitViewModel();
        }

        protected override void RefreshMethod()
        {
            if (!IsFilterInitialized)
            {
                Filter = new StatusFilter();

                IsFilterInitialized = true;
            }

            base.RefreshMethod();
        }

        private bool IsFilterInitialized { get; set; }

        private void InitViewModel()
        {
            Repository = new StatusRepository();
            
            AddCommand = new RelayCommand(x => AddMethod());
            EditCommand = new RelayCommand(x => EditMethod());
            DeleteCommand = new RelayCommand(x => DeleteMethod());
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
            var item = SelectedItem as Status;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Все устройства будут удалены.";
                
                if (ShowDialogMessage(text, caption))
                {
                    DeleteStatus(item);
                    if (RefreshCommand != null)
                        RefreshCommand.Execute(null);
                }
            }
        }

        private void DeleteStatus(Status item)
        {
            try
            {
                Repository.Delete(item);
            }
            catch (Exception ex)
            {
                ShowInfoMessage(ex.Message, "Ошибка");
            }
            
        }
    }
}
