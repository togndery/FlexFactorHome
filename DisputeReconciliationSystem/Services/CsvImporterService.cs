using DisputeReconciliationSystem.Interface;
using DisputeReconciliationSystem.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Services
{
    public class CsvImporterService : IDisputeImporter
    {
        public async Task<List<Dispute>> DisputesImportAsync(string filelocation)
        {
           var data = new List<Dispute>();
            try
            {
                using(var reader = new StreamReader(filelocation))
                {
                    //skip the first line 
                    await reader.ReadLineAsync();

                    var line = string.Empty; 

                    while((line = await reader.ReadLineAsync())!= null)
                    {
                        var values = line.Split(',');

                        if(values.Length >= 6)
                        {
                            data.Add(new Dispute
                            {
                                DisputeId = values[0],
                                TransactionId = values[1],
                                Amount = decimal.Parse(values[2], CultureInfo.InvariantCulture),
                                Currency = values[3],
                                Status = values[4],
                                Reason = values[5]

                            });
                        }
                    }
                }
            }
            catch(Exception ex) 
            {
                throw new Exception($"Error import CSV {ex.Message}");
            }
            return data;
        }
    }
}
