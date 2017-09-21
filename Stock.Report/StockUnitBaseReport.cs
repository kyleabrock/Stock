using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Stock.Core.Domain;
using System.Linq;
using Stock.Core.Repository;

namespace Stock.Report
{
    public class StockUnitBaseReport
    {
        /// <summary>
        /// Экспорт элемента базы данных в файл формата MS Word
        /// </summary>
        /// <param name="stockUnit">Экспортируемый элемент базы данных</param>
        /// <param name="templatePath">Путь к шаблону для экспорта в формате .docx</param>
        /// <param name="outPath">Полный путь экспортируемого файла в формате .docx</param>
        /// <returns>Возвращает результат операции</returns>
        public bool Export(StockUnit stockUnit, string templatePath, string outPath)
        {
            if (!File.Exists(templatePath))
                throw new FileNotFoundException(string.Format("Файл шаблона {0} не найден", templatePath));
            if (!Directory.Exists(Directory.GetDirectoryRoot(outPath)))
                throw new DirectoryNotFoundException(string.Format("Директория {0} не существует", outPath));

            if (File.Exists(outPath))
                File.Delete(outPath);
            File.Copy(templatePath, outPath);

            var stockUnitRepository = new StockUnitRepository();
            _stockUnit = stockUnitRepository.GetById(stockUnit.Id, true);

            var repairRepository = new RepairRepository();
            _repairList = repairRepository.GetAllByStockUnit(_stockUnit);

            var stockUnitNoteRepository = new StockUnitNoteRepository();
            _stockUnitNotes = stockUnitNoteRepository.GetByStockUnit(_stockUnit);

            using (var wordDocument = WordprocessingDocument.Open(outPath, true))
            {
                var doc = wordDocument.MainDocumentPart.Document;
                foreach (var table in doc.Body.Descendants<Table>())
                    ProcessTable(table);
                
                doc.Save();
            }

            return true;
        }

        private StockUnit _stockUnit;
        private IList<Repair> _repairList;
        private IList<StockUnitNote> _stockUnitNotes;

        /// <summary>
        /// Заполнение таблицы в документе 
        /// </summary>
        /// <param name="table">Таблица для заполнения</param>
        private void ProcessTable(Table table)
        {
            //Заполняем таблицу с элементами комплектов
            var tag = table.Descendants<Tag>().FirstOrDefault(t => t.Val.Value.StartsWith("Table:Units:"));
            if (tag != null)
            {
                IList<EntityBase> values = _stockUnit.UnitList.Cast<EntityBase>().ToList();
                FillRepeatableTableWithValues(table, tag, typeof(Unit), values);
            }

            //Заполняем таблицу с ремонтом
            tag = table.Descendants<Tag>().FirstOrDefault(t => t.Val.Value.StartsWith("Table:RepairList:"));
            if (tag != null)
            {
                IList<EntityBase> values = _repairList.Cast<EntityBase>().ToList();
                FillRepeatableTableWithValues(table, tag, typeof(Repair), values);
            }

            //Заполняем таблицу с ремонтом
            tag = table.Descendants<Tag>().FirstOrDefault(t => t.Val.Value.StartsWith("Table:StockUnitNotes:"));
            if (tag != null)
            {
                IList<EntityBase> values = _stockUnitNotes.Cast<EntityBase>().ToList();
                FillRepeatableTableWithValues(table, tag, typeof(StockUnitNote), values);
            }

            //Заполняем остальные таблицы с не повторяющимися ячейками
            var sdtRunList = table.Descendants<SdtRun>();
            var sdtCellList = table.Descendants<SdtCell>();
            
            foreach (var sdtRun in sdtRunList)
                AppendAfterElementInTable(sdtRun, typeof(StockUnit), _stockUnit, true);
            foreach (var sdtCell in sdtCellList)
                AppendAfterElementInTable(sdtCell, typeof(StockUnit), _stockUnit, true);

            //RemoveTags(table);
        }

        private void RemoveTags(Table table)
        {
            var rows = table.Descendants<TableRow>();
            foreach (var row in rows)
            {
                var sdtRunList = row.Descendants<SdtRun>().ToList();
                foreach (var sdtRun in sdtRunList)
                {
                    AppentAfterSdtRunToText(sdtRun, "");
                    sdtRun.Remove();
                }

                var sdtCellList = row.Descendants<SdtCell>().ToList();
                foreach (var sdtCell in sdtCellList)
                {
                    AppentAfterSdtCellToText(sdtCell, "", true);
                    sdtCell.Remove();
                }
            }
        }


        /// <summary>
        /// Заполнение таблицы с повторяющимися ячейками значениями
        /// </summary>
        /// <param name="table">Заполняемая таблица</param>
        /// <param name="tag">Тэг, по которому осуществляется поиск в таблице</param>
        /// <param name="domainType">Тип объекта базы данных</param>
        /// <param name="domainObjects">Экземпляр объекта базы данных</param>
        private void FillRepeatableTableWithValues(Table table, Tag tag, Type domainType, IList<EntityBase> domainObjects)
        {
            if (domainObjects.Count == 0) return;

            var tableRows = AddTableRows(table, tag, domainObjects.Count);
            if (tableRows.Count > domainObjects.Count)
                throw new Exception(string.Format("Number of rows in table more than {0}", domainType));

            for (int i = 0; i < tableRows.Count; i++)
            {
                var sdtRunList = tableRows[i].Descendants<SdtRun>().ToList();
                foreach (var sdtRun in sdtRunList)
                {
                    AppendAfterElementInTable(sdtRun, domainType, domainObjects[i]);
                    sdtRun.Remove();
                }

                var sdtCellList = tableRows[i].Descendants<SdtCell>().ToList();
                foreach (var sdtCell in sdtCellList)
                {
                    AppendAfterElementInTable(sdtCell, domainType, domainObjects[i], true);
                    sdtCell.Remove();
                }
            }
        }

        /// <summary>
        /// Заполнение таблицы с повторяющимися ячейками дополнительными строками
        /// </summary>
        /// <param name="table">Таблица для заполнения</param>
        /// <param name="tag">Тэг, расположенный в ячейке таблицы</param>
        /// <param name="count">Чисто дополнительных ячеек</param>
        /// <returns>Возвращает добавленные строки таблицы</returns>
        private IList<TableRow> AddTableRows(Table table, Tag tag, int count)
        {
            var result = new List<TableRow>();

            var parent = tag.Parent;
            while (parent != table)
            {
                var item = parent as TableRow;
                if (item != null)
                {
                    result.Add(item);
                    for (int i = 0; i < count - 1; i++)
                    {
                        var row = parent.CloneNode(true);
                        table.AppendChild(row);

                        result.Add((TableRow) row);
                    }
                    return result;
                }
                parent = parent.Parent;
            }

            return result;
        }
        
        private void AppendAfterElementInTable(OpenXmlElement element, Type domainType, EntityBase domainObject, bool replace = false)
        {
            var tags = element.Descendants<Tag>();
            foreach (var tag in tags)
            {
                //Удаление служебного значения "Table:DomainObject:" тэга
                var tagValue = tag.Val.Value;
                if (tagValue.Contains(":"))
                {
                    var split = tagValue.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    tagValue = split[split.Length - 1];
                }

                var propertyInfo = domainType.GetProperty(tagValue);
                if (propertyInfo != null)
                {
                    var field = propertyInfo.GetValue(domainObject, null);

                    var sdtRun = element as SdtRun;
                    if (sdtRun != null) AppentAfterSdtRunToText(sdtRun, field.ToString(), replace);
                    var sdtCell = element as SdtCell;
                    if (sdtCell != null) AppentAfterSdtCellToText(sdtCell, field.ToString(), replace);
                }
            }
        }

        private void AppentAfterSdtRunToText(SdtRun sdtRun, string text, bool replace = false)
        {
            var run = new Run();
            run.AppendChild(new Text(text));

            var parent = sdtRun.Parent;
            if (replace) 
                parent.ReplaceChild(run, sdtRun);
            else
            {
                parent.InsertAfter(run, sdtRun);
            }
        }

        private void AppentAfterSdtCellToText(SdtCell sdtCell, string text, bool replace = false)
        {
            var tableCell = new TableCell();
            var p = new Paragraph();
            var run = new Run(new Text(text));

            p.AppendChild(run);
            tableCell.AppendChild(p);

            var parent = sdtCell.Parent;
            if (replace)
                parent.ReplaceChild(tableCell, sdtCell);
            else
            {
                parent.InsertAfter(tableCell, sdtCell);
            }
        }
    }
}
