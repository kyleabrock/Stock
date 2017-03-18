using System;
using System.Collections.Generic;
using Stock.Core.Domain;

namespace Stock.Core.Filter.FilterParams
{
    public class DocumentFilterParams : IFilterParams
    {
        public DocumentFilterParams()
        {
            ClearFilter();
        }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public IEnumerable<DocumentType> DocumentType { get; set; }
        public IEnumerable<Owner> Owner { get; set; }
        
        public void ClearFilter()
        {
            StartDateTime = DateTime.Now.AddYears(-20);
            EndDateTime = DateTime.Now;
            DocumentType = new List<DocumentType>();
            Owner = new List<Owner>();
        }
    }
}
