using Cobalt.Core.Domain.Data;
using CsvHelper;
using System.Globalization;
using System.IO;

namespace Cobalt.Data.Csv
{

    public class CobaltCsvRecordSource : ICobaltDataSource
    {
        public CobaltCsvRecordSource(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }

        public CobaltRecordSet ReturnRecordsFromDataSource()
        {
            using (var reader = new StreamReader(FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //var records = csv.GetRecords<Foo>();
            }
            return new CobaltRecordSet();
        }
    }
}
