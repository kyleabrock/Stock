using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using Stock.Core.Domain;
using Stock.Core.Filter.FilterParams;

namespace Stock.Core.Filter
{
    public class DocumentFilter : FilterBase
    {
        public DocumentFilter() : base()
        {
            
        }

        public DocumentFilter(DocumentFilterParams filterParams)
        {
            if (filterParams != null)
                FilterParams = filterParams;
        }

        public override void CreateFilter()
        {
            var filterParams = FilterParams as DocumentFilterParams;
            if (filterParams == null) return;

            Criteria = DetachedCriteria.For<Document>();
            Criteria.CreateAlias("Owner", "owner")
                    .CreateAlias("DocumentType", "documentType");

            var owners = filterParams.Owner.Cast<EntityBase>().ToList();
            var docTypes = filterParams.DocumentType.Cast<EntityBase>().ToList();

            var ownersCriterion = CreateCriterion(owners, "owner");
            var docTypesCriterion = CreateCriterion(docTypes, "documentType");

            if (!string.IsNullOrEmpty(SearchString))
                SetSearchString(SearchString);

            ICriterion result = ownersCriterion;
            if (result == null && docTypesCriterion != null)
                result = docTypesCriterion;
            if (result != null && docTypesCriterion != null)
                result = Restrictions.And(docTypesCriterion, result);

            if (result == null && SearchStringCriterion != null)
                result = SearchStringCriterion;
            if (result != null && SearchStringCriterion != null)
                result = Restrictions.And(SearchStringCriterion, result);

            if (result != null) Criteria.Add(result);
        }

        private void SetSearchString(string searchString)
        {
            ICriterion documentType = Restrictions.Like("documentType.TypeName", searchString, MatchMode.Anywhere);
            ICriterion owner = Restrictions.Like("owner.Name.DisplayName", searchString, MatchMode.Anywhere);
            ICriterion comments = Restrictions.Like("Comments", searchString, MatchMode.Anywhere);

            ICriterion result = documentType;
            result = Restrictions.Or(result, owner);
            result = Restrictions.Or(result, comments);

            SearchStringCriterion = result;
        }
    }
}
