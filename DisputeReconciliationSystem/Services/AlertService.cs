using DisputeReconciliationSystem.Interface;
using DisputeReconciliationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Services
{
    public class AlertService : IAlertService
    {
        public Task SendAlertAsync(Discrepancy discrepancy)
        {
          if(discrepancy == null)
            {
                Console.ForegroundColor= ConsoleColor.White;
                Console.WriteLine($"****Aletr****{discrepancy} is null");
                Console.ResetColor();
                return Task.CompletedTask;
            }

            switch (discrepancy.Severity)
            {
                case DiscrepancySeverity.High:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case DiscrepancySeverity.Medium:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case DiscrepancySeverity.Low:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }


            
            Console.WriteLine($"***Alert*** {discrepancy.Severity} severity {discrepancy.Description}");

            return Task.CompletedTask;
        }
    }
}
