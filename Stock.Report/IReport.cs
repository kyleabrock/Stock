using System.Collections.Generic;
using Stock.Core.Domain;

namespace Stock.Report
{
    public interface IReport
    {
        string LastError { get; }
        string LastExportedFileName { get; }
        bool Export(EntityBase entityBase, string templatePath, string exportPath);
        IEnumerable<string> GetTemplates(string templatesFolderPath);
    }
}
