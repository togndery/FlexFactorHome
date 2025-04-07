using DisputeReconciliationSystem.Interface;
using DisputeReconciliationSystem.Rports;
using DisputeReconciliationSystem.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Helper
{
    public static class ServicesConfiguration
    {
        public static ServiceProvider ConfigServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IDisputeImporter , CsvImporterService>();
            services.AddSingleton<IDispute , MockDataServices>();
            services.AddSingleton<IReconciliationService , ReconciliationService>();
            services.AddSingleton<IReport, ReportCsvTypeGenerator>();
            services.AddSingleton<IAlertService ,AlertService>();
            services.AddSingleton<ICurrencyConverter ,CurrencyConverterServices>();

            return services.BuildServiceProvider();


        }

     
       
    }
}
