using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace Stock.Core.Finder
{
    public interface IFinder
    {
        DetachedCriteria Criteria { get; }
        void CreateCriteria(string searchString);
    }
}
