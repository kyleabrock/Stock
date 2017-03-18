using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Stock.Core.Domain;

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
                
                var body = doc.Body;
                var tableList = body.Descendants<Table>().ToList();
                foreach (var table in tableList)
                {
                    var nodesValue = TableSelectNodesText("TblStart", table);
                    var startRow = FindRowByMergeFieldText("TblStart", table, true);
                    var endRow = FindRowByMergeFieldText("TblEnd", table, true);
                    if (startRow != null && endRow != null)
                    {
                        var nodes = stockUnitXml.SelectNodes(nodesValue);
                        FillTable(table, startRow, nodes);
                    }
                }

                var fieldCodes = body.Descendants<FieldCode>();
                foreach (var fieldCode in fieldCodes)
                {
                    
                }


                doc.Save();
            }
        }

        private void ReplaceFieldCode(OpenXmlElement fieldCode, OpenXmlElement rElement)
        {
            var fieldCoreParent = fieldCode.Parent as Run;
            if (fieldCoreParent == null) return;

            var runParent = fieldCoreParent.Parent;
            var runs = runParent.ChildElements;
            for (int i = 0; i < runs.Count; i++)
            {
                if (runs[i].ChildElements.Contains(fieldCode))
                {
                    var beginFieldChar = runs[i - 1];
                    var endFieldChar = runs[i + 1];
                    var text = runs[i + 2];
                    
                    runParent.RemoveChild(beginFieldChar);
                    runParent.RemoveChild(endFieldChar);
                    runParent.RemoveChild(text);

                    fieldCoreParent.ReplaceChild(rElement, fieldCode);
                }
            }
        }

        private void FillTable(Table table, TableRow fieldsRow, XmlNodeList list)
        {
            var lastRow = fieldsRow;
            var newRow = fieldsRow.CloneNode(true) as TableRow;
            var row = newRow.CloneNode(true) as TableRow;
            
            foreach (XmlNode xmlNode in list)
            {
                var fields = row.Descendants<FieldCode>();
                foreach (var field in fields)
                {
                    var codeTextList = field.InnerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (codeTextList.Length <= 1) continue;
                    var codeText = codeTextList[1];

                    var node = xmlNode.SelectSingleNode(codeText);
                    if (node != null)
                    {
                        ReplaceFieldCode(field, new Text(node.InnerText));
                    }
                }

                var simpleFields = row.Descendants<SimpleField>();
                foreach (var simpleField in simpleFields)
                {
                    var instruction = simpleField.Instruction.Value;
                    var codeTextList = instruction.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (codeTextList.Length <= 1) continue;
                    
                    var codeText = codeTextList[1];
                    var node = xmlNode.SelectSingleNode(codeText);
                    if (node != null)
                    {
                        var parent = simpleField.Parent;
                        parent.ReplaceChild(new Run(new Text(node.InnerText)), simpleField);
                    }
                }

                table.InsertAfter(row, lastRow);
                lastRow = row;
                row = newRow.CloneNode(true) as TableRow;
            }
            
            fieldsRow.Remove();
        }

        private string TableSelectNodesText(string text, Table table)
        {
            var codes = table.Descendants<FieldCode>();
            foreach (var code in codes)
            {
                var codeTextList = code.InnerText.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                if (codeTextList.Length <= 1) continue;

                var codeText = codeTextList[1];
                if (codeText.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                {
                    var resultList = codeText.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
                    if (resultList.Length > 1)
                        return resultList[1];
                }
            }
            
            return "";
        }

        private TableRow FindRowByMergeFieldText(string text, Table table, bool deleteField)
        {
            var codes = table.Descendants<FieldCode>();
            foreach (var code in codes)
            {
                var codeTextList = code.InnerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (codeTextList.Length <= 1) continue;

                var codeText = codeTextList[1];
                if (codeText.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                {
                    TableRow row;
                    if (FindRow(code, out row))
                    {
                        if (deleteField) ReplaceFieldCode(code, new Text());
                        return row;
                    }

                    return null;
                }
            }

            return null;
        }

        private bool FindRow(OpenXmlElement child, out TableRow row)
        {
            var parent = child.Parent;
            while (!(parent is TableRow))
            {
                if (parent != null) parent = parent.Parent;
                else
                {
                    row = null;
                    return false;
                }
            }

            row = parent as TableRow;
            return true;
        }
    }
}
