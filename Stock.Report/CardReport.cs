using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Stock.Core.Domain;
using Stock.Core.Repository;

namespace Stock.Report
{
    public class CardReport
    {
        public CardReport()
        {
            _templateFilePath = AppDomain.CurrentDomain.BaseDirectory;
            _templateFilePath += "\\Templates\\passportTemplate.docx";
            _outFilePath = "";
        }

        public void Export(Card card, bool overwrite)
        {
            Export(_templateFilePath, _outFilePath, card, overwrite);
        }

        public void Export(string templateFilePath, string outFilePath, Card card, bool overwrite)
        {
            if (!File.Exists(templateFilePath))
                throw new FileNotFoundException("File not found (template for export):", templateFilePath);

            var outFileName = outFilePath;
            if (string.IsNullOrEmpty(outFilePath))
            {
                var assemplyDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                var outputDir = assemplyDir + "\\Export\\";
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                outFileName = outputDir + DateTime.Now.ToString("yyyyMMdd_HH-mm-ss") + "_" + card.CardNumber + ".docx";
            }
            //TODO: if exists && !overwrite add to filename number
            if (File.Exists(outFileName) && overwrite)
                File.Delete(outFileName);
            File.Copy(templateFilePath, outFileName);

            try
            {
                ExportToWord(outFileName, card);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void ExportToWord(string filePath, Card card)
        {
            using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Open(filePath, true))
            {
                Table unitTable = wordDocument.MainDocumentPart.Document.Body.Elements<Table>().First();

                var stockUnitList = GetStockUnitList(card);
                for (int i = 0; i < stockUnitList.Count; i++)
                {
                    var stockUnit = stockUnitList[i];
                    var unitList = stockUnit.UnitList;

                    for (int j = 0; j < unitList.Count; j++)
                    {
                        var unit = unitList[j];

                        var tr = GetUnitTableRow(stockUnit, unit, i, j);
                        unitTable.Append(tr);
                    }
                }

                wordDocument.MainDocumentPart.Document.Save();
            }
        }

        private string _templateFilePath = "";
        private string _outFilePath = "";

        private TableRow GetUnitTableRow(StockUnit stockUnit, Unit unit, int pointA, int pointB)
        {
            TableRow tr = new TableRow();
            
            TableCell cell1 = new TableCell();
            Paragraph paragraph1 = cell1.AppendChild(new Paragraph());
            Run run1 = paragraph1.AppendChild(new Run());
            run1.AppendChild(new Text((pointA + 1) + "." + (pointB + 1)));

            TableCell cell2 = new TableCell();
            Paragraph paragraph2 = cell2.AppendChild(new Paragraph());
            Run run2 = paragraph2.AppendChild(new Run());
            run2.AppendChild(new Text(unit.UnitType.TypeName));
            run2.AppendChild(new Break());
            run2.AppendChild(new Text(unit.Manufacture + " " + unit.ModelName));

            TableCell cell3 = new TableCell();
            Paragraph paragraph3 = cell3.AppendChild(new Paragraph());
            Run run3 = paragraph3.AppendChild(new Run());
            run3.AppendChild(new Text(unit.Serial));
            run3.AppendChild(new Break());
            run3.AppendChild(new Text("(инв.№" + stockUnit.StockNumber + ")"));

            var comments = unit.Comments;
            TableCell cell4 = new TableCell();
            Paragraph paragraph4 = cell4.AppendChild(new Paragraph());
            Run run4 = paragraph4.AppendChild(new Run());
            run4.AppendChild(new Text(comments));

            tr.Append(cell1, cell2, cell3, cell4);
            return tr;
        }

        private IList<StockUnit> GetStockUnitList(Card card)
        {
            var repository = new StockUnitRepository();
            return repository.GetFromCard(card.Id);
        }
    }
}
