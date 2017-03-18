using System.Collections.Generic;
using NHibernate.Criterion;
using Stock.Core.Domain;
using Stock.Core.Filter.FilterParams;

namespace Stock.Core.Filter
{
    public abstract class FilterBase : IFilter
    {
        public FilterBase() {}

        private DetachedCriteria _criteria = DetachedCriteria.For<EntityBase>();
        public DetachedCriteria Criteria
        {
            get { return _criteria; }
            protected set { _criteria = value; }
        }

        public string SearchString { get; set; }
        public IFilterParams FilterParams { get; set; }

        public abstract void CreateFilter();

        public void ClearFilter()
        {
            FilterParams = null;
            SearchString = string.Empty;
            
            Criteria = DetachedCriteria.For<EntityBase>();
        }

        public void ClearFilterParams()
        {
            FilterParams = null;
        }

        public string LastError { get; set; }

        
        protected ICriterion SearchStringCriterion;

        protected ICriterion CreateCriterion(IEnumerable<string> entities, string aliasName, string fieldName)
        {
            ICriterion result = null;
            foreach (var entity in entities)
            {
                result = result == null ? CreateCriterion(entity, aliasName, fieldName) :
                    Restrictions.Or(result, CreateCriterion(entity, aliasName, fieldName));
            }

            return result;
        }

        protected ICriterion CreateCriterion(IEnumerable<EntityBase> entities, string aliasName)
        {
            ICriterion result = null;
            foreach (var entity in entities)
            {
                result = result == null ? CreateIdCriterion(entity, aliasName) :
                    Restrictions.Or(result, CreateIdCriterion(entity, aliasName));
            }

            return result;
        }

        private ICriterion CreateCriterion(string entity, string aliasName, string field)
        {
            var eqField = aliasName + "." + field;
            return Restrictions.Eq(eqField, entity);
        }

        private ICriterion CreateIdCriterion(EntityBase entity, string field)
        {
            var idField = field + ".Id";
            return Restrictions.Eq(idField, entity.Id);
        }
    }
}
