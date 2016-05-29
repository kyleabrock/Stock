using System;

namespace Core.Domain
{
    public class DocumentNumber
    {
        public virtual string Number { get; set; }
        public virtual DateTime Date { get; set; }

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
