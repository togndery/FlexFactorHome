using DisputeReconciliationSystem.Interface;
using DisputeReconciliationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Rports
{
    public class ReportJsonTypeGenerator : IReport
    {
        public async Task GenerateReportAsync(ReconciliationResult result, string outputPath)
        {
            try
            {
                var ops = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                var jsonObj = JsonSerializer.Serialize(result, ops);
                await File.WriteAllTextAsync(outputPath, jsonObj);
            }
            catch (Exception ex)
            {

                throw new Exception($"Error generating JSON report: {ex.Message}", ex);
            }
        }
    }
}
