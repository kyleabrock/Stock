using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.Core.Domain
{
    public interface ILoggedEntity
    {
        string LoggedMessage { get; }
    }
}
