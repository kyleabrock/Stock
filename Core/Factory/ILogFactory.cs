using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stock.Core.Domain;

namespace Stock.Core.Factory
{
    public interface ILogFactory
    {
        Log CreateLogMessage(UserAcc user, ILoggedEntity entity);
    }
}
