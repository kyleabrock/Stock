using System.Xml;
using Stock.Core.Domain;
using Stock.Core.Repository;

namespace Stock.Report
{
    public class StockUnitXmlConverter
    {
        public StockUnitXmlConverter()
        {
            _repository = new StockUnitRepository();
            _repairRepository = new RepairRepository();
            _noteRepository = new StockUnitNoteRepository();
        }

        public XmlDocument Convert(StockUnit arg)
        {
            _stockUnit = _repository.GetById(arg.Id, true);

            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", "yes"));

            var root = (XmlElement) doc.AppendChild(doc.CreateElement("StockUnit"));
            AppendStockUnitProperties(doc, root);
            AppendUnits(doc, root);
            AppendRepair(doc, root);
            AppendNotes(doc, root);

            return doc;
        }

        private readonly StockUnitRepository _repository;
        private readonly RepairRepository _repairRepository;
        private readonly StockUnitNoteRepository _noteRepository;
        private StockUnit _stockUnit;

        private void AppendStockUnitProperties(XmlDocument doc, XmlElement root)
        {
            var stockNumber = doc.CreateElement("StockNumber");
            var stockName = doc.CreateElement("StockName");
            var creationDate = doc.CreateElement("CreationDate");
            var comments = doc.CreateElement("Comments");

            stockNumber.InnerText = _stockUnit.StockNumber;
            stockName.InnerText = _stockUnit.StockName;
            creationDate.InnerText = _stockUnit.CreationDate.ToShortDateString();
            comments.InnerText = _stockUnit.Comments;

            root.AppendChild(stockNumber);
            root.AppendChild(stockName);
            root.AppendChild(creationDate);
            root.AppendChild(comments);
        }

        private void AppendUnits(XmlDocument doc, XmlElement root)
        {
            var units = doc.CreateElement("Units");
            var unitList = _stockUnit.UnitList;
            foreach (var unit in unitList)
            {
                var unitElement = doc.CreateElement("Unit");
                var unitType = doc.CreateElement("UnitType");
                var manufacture = doc.CreateElement("Manufacture");
                var model = doc.CreateElement("Model");
                var serial = doc.CreateElement("Serial");
                var comments = doc.CreateElement("Comments");

                unitType.InnerText = unit.UnitType.TypeName;
                manufacture.InnerText = unit.Manufacture;
                model.InnerText = unit.ModelName;
                serial.InnerText = unit.Serial;
                comments.InnerText = unit.Comments;

                unitElement.AppendChild(unitType);
                unitElement.AppendChild(manufacture);
                unitElement.AppendChild(model);
                unitElement.AppendChild(serial);
                unitElement.AppendChild(comments);

                units.AppendChild(unitElement);
            }

            root.AppendChild(units);
        }

        private void AppendRepair(XmlDocument doc, XmlElement root)
        {
            var repairList = _repairRepository.GetAllByStockUnit(_stockUnit);
            
            var repairElement = doc.CreateElement("RepairList");
            foreach (var repair in repairList)
            {
                var unitElement = doc.CreateElement("Repair");
                var defect = doc.CreateElement("Defect");
                var result = doc.CreateElement("Result");
                var startedDate = doc.CreateElement("StartedDate");
                var completedDate = doc.CreateElement("CompletedDate");
                var user = doc.CreateElement("User");
                var comments = doc.CreateElement("Comments");

                defect.InnerText = repair.Defect;
                result.InnerText = repair.Result;
                startedDate.InnerText = repair.StartedDate.ToShortDateString();
                completedDate.InnerText = repair.CompletedDate.ToShortDateString();
                user.InnerText = repair.User.Name.DisplayName;
                comments.InnerText = repair.Comments;

                unitElement.AppendChild(defect);
                unitElement.AppendChild(result);
                unitElement.AppendChild(startedDate);
                unitElement.AppendChild(completedDate);
                unitElement.AppendChild(user);
                unitElement.AppendChild(comments);

                repairElement.AppendChild(unitElement);
            }

            root.AppendChild(repairElement);
        }

        private void AppendNotes(XmlDocument doc, XmlElement root)
        {
            var notesList = _noteRepository.GetByStockUnitId(_stockUnit);

            var notesElement = doc.CreateElement("NotesList");
            foreach (var note in notesList)
            {
                var noteElement = doc.CreateElement("Note");
                var title = doc.CreateElement("Title");
                var text = doc.CreateElement("Text");
                var comments = doc.CreateElement("Comments");

                title.InnerText = note.Title;
                text.InnerText = note.Text;
                comments.InnerText = note.Comments;

                noteElement.AppendChild(title);
                noteElement.AppendChild(text);
                noteElement.AppendChild(comments);

                notesElement.AppendChild(noteElement);
            }

            root.AppendChild(notesElement);
        }
    }
}
