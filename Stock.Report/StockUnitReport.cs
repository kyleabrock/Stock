using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Stock.Core.Domain;
using Stock.Core.Repository;

namespace Stock.Report
{
    public class StockUnitReport : IReport
    {
        public StockUnitReport()
        {
            _repairRepository = new RepairRepository();
            _noteRepository = new StockUnitNoteRepository();
        }

        public string LastError { get; private set; }
        public string LastExportedFileName { get; private set; }

        public bool Export(EntityBase entityBase, string templatePath, string exportPath)
        {
            var stockUnit = entityBase as StockUnit;
            if (stockUnit == null)
            {
                LastError = "Несоответствие типа объекта. Экспорт невозможен";
                return false;
            }

            if (!File.Exists(templatePath))
            {
                LastError = "Не найден файл шаблона: " + templatePath;
                return false;
            }

            exportPath = exportPath.TrimEnd(new[] {'\\'});
            var outFileName = exportPath + "\\StockUnit\\" + stockUnit.StockNumber 
                + "_" + DateTime.Now.ToString("yyyyMMdd_HH-mm-ss") + ".docx";
            
            var outDirectory = Path.GetDirectoryName(outFileName);
            if (string.IsNullOrEmpty(outDirectory))
            {
                LastError = "Не задан файл выгрузки или адрес некоректен: " + outFileName;
                return false;
            }
            if (!Directory.Exists(outDirectory))
            {
                LastError = "Директория не существует: " + outDirectory;
                return false;
            }
            
            if (File.Exists(outFileName))
                File.Delete(outFileName);
            File.Copy(templatePath, outFileName);

            try
            {
                FillDocument(stockUnit, outFileName);
                LastExportedFileName = Path.GetFullPath(outFileName);
                return true;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
        }

        public IEnumerable<string> GetTemplates(string templatesFolderPath)
        {
            const string searchPattern = "*.docx";

            char[] charsToTrim = {'\\'};
            var templatesFolder = templatesFolderPath.TrimEnd(charsToTrim);
            var result = Directory.GetFiles(templatesFolder + "\\StockUnit\\", searchPattern, SearchOption.TopDirectoryOnly);
            
            return result.ToList();
        }

        private readonly RepairRepository _repairRepository;
        private readonly StockUnitNoteRepository _noteRepository;

        private void FillDocument(StockUnit arg, string filePath)
        {
            using (var wordDocument = WordprocessingDocument.Open(filePath, true))
            {
                var doc = wordDocument.MainDocumentPart.Document;

                ExportStockUnitData(doc, arg);
                ExportUnitData(doc, arg.UnitList);

                var repair = _repairRepository.GetAllByStockUnit(arg);
                ExportRepairData(doc, repair);

                var notes = _noteRepository.GetByStockUnit(arg);
                ExportNoteData(doc, notes);

                RemoveSdtElements(doc);
                RemoveRubbish(doc);
                
                doc.Save();
            }
        }

        private void ExportStockUnitData(DocumentFormat.OpenXml.Wordprocessing.Document doc, StockUnit stockUnit)
        {
            string tag;
            tag = "StockUnit:StockNumber";
            FillRepeatingData(doc, tag, stockUnit.StockNumber, false);
            tag = "StockUnit:StockName";
            FillRepeatingData(doc, tag, stockUnit.StockName, false);
            tag = "StockUnit:CreationDate";
            FillRepeatingData(doc, tag, stockUnit.CreationDate.ToShortDateString(), false);
        }

        private void ExportUnitData(DocumentFormat.OpenXml.Wordprocessing.Document doc, IList<Unit> unitList)
        {
            string tag;
            for (int i = 0; i < unitList.Count; i++)
            {
                var addRow = i != unitList.Count - 1;
                tag = "tbl:Unit:AutoNumber";
                FillRepeatingData(doc, tag, (i + 1).ToString(), addRow);
                tag = "tbl:Unit:UnitType";
                FillRepeatingData(doc, tag, unitList[i].UnitType.TypeName, false);
                tag = "tbl:Unit:Manufacture";
                FillRepeatingData(doc, tag, unitList[i].Manufacture, false);
                tag = "tbl:Unit:ModelName";
                FillRepeatingData(doc, tag, unitList[i].ModelName, false);
                tag = "tbl:Unit:Serial";
                FillRepeatingData(doc, tag, unitList[i].Serial, false);
                tag = "tbl:Unit:Comments";
                FillRepeatingData(doc, tag, unitList[i].Comments, false);
            }
        }

        private void ExportRepairData(DocumentFormat.OpenXml.Wordprocessing.Document doc, IList<Repair> repairList)
        {
            string tag;
            for (int i = 0; i < repairList.Count; i++)
            {
                var addRow = i != repairList.Count - 1;
                tag = "tbl:Repair:AutoNumber";
                FillRepeatingData(doc, tag, (i + 1).ToString(), addRow);
                tag = "tbl:Repair:Defect";
                FillRepeatingData(doc, tag, repairList[i].Defect, false);
                tag = "tbl:Repair:Result";
                FillRepeatingData(doc, tag, repairList[i].Result, false);
                tag = "tbl:Repair:UserAcc";
                FillRepeatingData(doc, tag, repairList[i].User.Name.DisplayName, false);
                tag = "tbl:Repair:CompletedDate";
                FillRepeatingData(doc, tag, repairList[i].CompletedDate.ToShortDateString(), false);
                tag = "tbl:Repair:Comments";
                FillRepeatingData(doc, tag, repairList[i].Comments, false);
            }
        }

        private void ExportNoteData(DocumentFormat.OpenXml.Wordprocessing.Document doc, IList<StockUnitNote> notes)
        {
            string tag;
            for (int i = 0; i < notes.Count; i++)
            {
                var addRow = i != notes.Count - 1;
                tag = "tbl:Notes:AutoNumber";
                FillRepeatingData(doc, tag, (i + 1).ToString(), addRow);
                tag = "tbl:Notes:Title";
                FillRepeatingData(doc, tag, notes[i].Title, false);
                tag = "tbl:Notes:Text";
                FillRepeatingData(doc, tag, notes[i].Text, false);
                tag = "tbl:Notes:Comments";
                FillRepeatingData(doc, tag, notes[i].Comments, false);
            }
        }

        private void FillRepeatingData(DocumentFormat.OpenXml.Wordprocessing.Document doc, string tag, string text, bool addRow)
        {
            var element = doc.Body.Descendants<SdtElement>()
                   .FirstOrDefault(sdt => sdt.SdtProperties.GetFirstChild<Tag>().Val == tag);
            if (element == null) return;
            
            var tableRow = element.Parent as TableRow;
            if (tableRow == null)
            {
                tableRow = element.Parent.Parent as TableRow;
                if (tableRow == null)
                {
                    tableRow = element.Parent.Parent.Parent as TableRow;
                    if (tableRow == null) return;
                }
            }
            
            var table = tableRow.Parent as Table;
            if (table == null) return;

            var newTableRow = tableRow.Clone() as OpenXmlElement;
            if (addRow)
                table.InsertAfter(newTableRow, tableRow);

            ReplaceSdtWithText(element, text);
        }

        private void RemoveSdtElements(DocumentFormat.OpenXml.Wordprocessing.Document doc)
        {
            var sdtList = doc.Body.Descendants<SdtElement>().ToList();
            foreach (var element in sdtList)
                element.Remove();
        }

        private void RemoveRubbish(DocumentFormat.OpenXml.Wordprocessing.Document doc)
        {
            var body = doc.Body;
            var textList = body.Descendants<Text>();
            foreach (var text in textList)
            {
                text.Text = Regex.Replace(text.Text, @"((^\s*\(\))|(^\s*\()|(^\s*\)))", "", RegexOptions.None);
            }
        }

        private void ReplaceSdtWithText(SdtElement element, string arg)
        {
            if (element == null) return;
            var openXmlElement = element.Parent;

            OpenXmlElement text = new Run(new Text(arg));
            if (openXmlElement is TableCell)
                text = new Paragraph(new Run(new Text(arg)));
            if (openXmlElement is TableRow)
                text = new TableCell(new Paragraph(new Run(new Text(arg))));
            
            openXmlElement.ReplaceChild(text, element);
        }
    }
}
