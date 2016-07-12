using System;
using System.ComponentModel;
using System.Windows.Input;
using Stock.UI.Properties;

namespace Stock.UI.ViewModels.Base
{
    public abstract class AsyncCommandBase : ICommand, INotifyPropertyChanged
    {
        public void Execute(object parameter)
        {
            try
            {
                OnRunWorkerStarting();

                var worker = new BackgroundWorker();
                worker.DoWork += ((sender, e) => OnExecute(e.Argument));
                worker.RunWorkerCompleted += ((sender, e) => OnRunWorkerCompleted(e));
                
                if (!worker.IsBusy)
                    worker.RunWorkerAsync(parameter);
            }
            catch (Exception ex)
            {
                OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, ex, true));
                throw;
            }
        }

        public virtual bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler RunWorkerStarting;
        public event RunWorkerCompletedEventHandler RunWorkerCompleted;

        public abstract string Text { get; }
        
        private bool _isExecuting;
        public bool IsExecuting
        {
            get { return _isExecuting; }
            private set 
            {
                _isExecuting = value;
                if (CanExecuteChanged != null) CanExecuteChanged(this, EventArgs.Empty);
                OnPropertyChanged("IsExecuting");
            }
        }

        protected abstract void OnExecute(object paramenter);

        private void OnRunWorkerStarting()
        {
            IsExecuting = true;
            if (RunWorkerStarting != null)
                RunWorkerStarting(this, EventArgs.Empty);

        }

        private void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            IsExecuting = false;
            if (RunWorkerCompleted != null)
                RunWorkerCompleted(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
