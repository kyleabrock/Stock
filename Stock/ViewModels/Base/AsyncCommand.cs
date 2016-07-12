using System;

namespace Stock.UI.ViewModels.Base
{
    public class AsyncCommand : AsyncCommandBase
    {
        public AsyncCommand(Action<object> action)
        {
            ExecuteDelegate = action;
        }

        public Action<object> ExecuteDelegate { get; set; }

        public override string Text
        {
            get { throw new NotImplementedException(); }
        }

        protected override void OnExecute(object paramenter)
        {
            if (ExecuteDelegate != null)
                ExecuteDelegate(paramenter);
        }
    }
}
