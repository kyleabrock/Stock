using System;

namespace Stock.Core.Domain
{
    public class StockUnitNote : EntityBase
    {
        public virtual StockUnit StockUnit { get; set; }

        public virtual string Title
        {
            get { return _title; } 
            set { _title = value; }
        }

        public virtual string Text
        {
            get { return _text; } 
            set { _text = value; }
        }

        public virtual string Comments
        {
            get { return _comments; } 
            set { _comments = value; }
        }
        
        private string _title = String.Empty;
        private string _text = String.Empty;
        private string _comments = String.Empty;

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            var other = obj as StockUnitNote;
            if (other == null)
                return false;

            return (Title == other.Title)
                && (Text == other.Text)
                && (Comments == other.Comments);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (_title != null ? _title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_text != null ? _text.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_comments != null ? _comments.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StockUnit != null ? StockUnit.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(StockUnitNote other)
        {
            return base.Equals(other) 
                && string.Equals(_title, other._title) 
                && string.Equals(_text, other._text) 
                && string.Equals(_comments, other._comments);
        }
    }
}
