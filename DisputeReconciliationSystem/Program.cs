using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DisputeReconciliationSystem.Helper;
using DisputeReconciliationSystem.Interface;
using DisputeReconciliationSystem.Services;
using DisputeReconciliationSystem.Model;

namespace DisputeReconciliationSystem
{
    public class Program
    {

        static  async Task Main(string[] args)
        {
            var services = ServicesConfiguration.ConfigServices();


            var disputImporter = services.GetRequiredService<IDisputeImporter>();
            var intDataServices = services.GetRequiredService<IDispute>();
            var reconcliationServices = services.GetRequiredService<IReconciliationService>();
            var report = services.GetRequiredService<IReport>();
            var alert = services.GetRequiredService<IAlertService>();

            //command line args 
            string inputFilePath = args.Length > 0 ? args[0] : "external_disputes.csv";
            string outputFilePath = args.Length > 1 ? args[1] : "reconciliation_report.json";

            Console.WriteLine($"Starting reconciliation process...");
            Console.WriteLine($"Importing data from: {inputFilePath}");

            var externalDisputes = await disputImporter.DisputesImportAsync(inputFilePath);
            Console.WriteLine($"Imported {externalDisputes.Count} external disputes");

            var internalDisputes = await intDataServices.GetAllDisputesAsync();
            Console.WriteLine($"Retrieved {internalDisputes.Count} internal disputes");

            var reconciliationResult = await reconcliationServices.ReconcileDisputesAsync(externalDisputes, internalDisputes);
            Console.WriteLine("Reconciliation completed");

            foreach (var discrepancy in reconciliationResult.Discrepancies.Where(d => d.Severity == DiscrepancySeverity.High))
            {
                await alert.SendAlertAsync(discrepancy);
            }


            await report.GenerateReportAsync(reconciliationResult, outputFilePath);
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Report generated and saved to: {outputFilePath}");
          
            Console.WriteLine("Reconciliation process completed successfully");


        }
    }
}
