using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Stock.Core.Domain;
using Stock.Core.Repository;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;

namespace Stock.Report
{
    public class SimpleReport : IReport
    {
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
            var outFileName = exportPath + "\\" + stockUnit.StockNumber 
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

        private void FillDocument(StockUnit arg, string filePath)
        {
            using (var wordDocument = WordprocessingDocument.Open(filePath, true))
            {
                var doc = wordDocument.MainDocumentPart.Document;

                var xmlConverter = new StockUnitXmlConverter();
                var stockUnitXml = xmlConverter.Convert(arg);
                ExportStockUnitData(doc, stockUnitXml);
                doc.Save();
            }
        }

        private void ExportStockUnitData(Document doc, XmlDocument stockUnitXml)
        {
            //Get document body
            var body = doc.Body;
            var tables = body.Elements<Table>().ToList();
            foreach (var table in tables)
            {
                var codes = table.Descendants<FieldCode>().ToList();
                foreach (var code in codes)
                {
                    var list = code.InnerText.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    var text = list[1];

                    var xmlDoc = stockUnitXml.DocumentElement;
                    if (xmlDoc == null) return;
                    
                    XmlNode node = xmlDoc.SelectSingleNode(text);
                    if (node != null)
                    {
                        var newText = new Run(new Text(node.InnerText));
                        var run = code.Parent;
                        var para = run.Parent;
                        para.RemoveChild(run);
                        para.AppendChild(newText);
                    }
                }
            }
        }
    }
}
