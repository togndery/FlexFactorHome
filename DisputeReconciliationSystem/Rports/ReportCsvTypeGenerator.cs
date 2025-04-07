using DisputeReconciliationSystem.Interface;
using DisputeReconciliationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Rports
{
    public class ReportCsvTypeGenerator : IReport
    {
        public async Task GenerateReportAsync(ReconciliationResult result, string outputPath)
        {
			try
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("ReconciliationDate,TotalExternalDisputes,TotalInternalDisputes,MatchedDisputes,TotalDiscrepancies");
				sb.AppendLine($"{result.ReconciliationDate:yyyy-MM-dd HH:mm:ss},{result.TotalInternalDisputes},{result.TotalExternalDisputes},{result.MatchedDisputes},{result.Discrepancies.Count}");

                sb.AppendLine("\nDisputeId,DiscrepancyType,Severity,Description");
                foreach (var discrepancy in result.Discrepancies)
                {
                    sb.AppendLine($"{discrepancy.DisputeId},{discrepancy.Type},{discrepancy.Severity},\"{discrepancy.Description}\"");
                }

                await File.WriteAllTextAsync(outputPath, sb.ToString());
            }
			catch (Exception ex)
			{

                throw new Exception($"Error generating CSV report: {ex.Message}", ex);
            }
        }
    }
}
