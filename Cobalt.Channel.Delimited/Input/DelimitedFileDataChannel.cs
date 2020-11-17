using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Cobalt.Unit;
using CsvHelper;

namespace Cobalt.Channel.Delimited.Input
{
    public class DelimitedFileDataChannel : DelimitedDataChannel<DelimitedFileDataChannelOptions>
    {
        public DelimitedFileDataChannel(DelimitedFileDataChannelOptions options) : base(options)
        {
        }

        public override async Task<IEnumerable<CobaltUnit>> GetDataAsync()
        {
            var records = new List<CobaltUnit>();

            try
            {
                using (var reader = new StreamReader(Options.CsvFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.HasHeaderRecord = true;
                    var dynamicRecords = csv.GetRecords<dynamic>();

                    foreach (var record in dynamicRecords) records.Add(new CobaltUnit(false, record));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return records;
        }
    }
}