using System;
using System.Data.SqlTypes;

namespace Stock.Core.Domain
{
    public class DocumentNumber
    {
        private string _number = "";
        public virtual string Number
        {
            get { return _number; }
            set { _number = value; }
        }
        
        private DateTime _date = SqlDateTime.MinValue.Value;
        public virtual DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public virtual string FullNumber
        {
        	get
        	{
        	    if (string.IsNullOrEmpty(Number) && Date == DateTime.MinValue)
        	        return "б/н";
                if (string.IsNullOrEmpty(Number) && Date != DateTime.MinValue)
                    return Number;
                if (!string.IsNullOrEmpty(Number) && Date == DateTime.MinValue)
                    return "б/н от " + Date.ToShortDateString();
        	    
        		return Number + " от " + Date.ToShortDateString();
        	}
        }        
    }
}
