using DisputeReconciliationSystem.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Services
{
    internal class CurrencyConverterServices : ICurrencyConverter
    {
        public Task<decimal> CurrencyConvertorAsync(decimal amount, string fromCurrency, string toCurrency)
        {
           if(fromCurrency == toCurrency)
            {
                return Task.FromResult(amount);
            }

           if(!_rates.TryGetValue(fromCurrency , out var resultforRate))
            {
                throw new ArgumentException($"rate for {fromCurrency} not available"); 
            }


            if (!_rates.TryGetValue(fromCurrency, out var resulttoRate))
            {
                throw new ArgumentException($"rate for {toCurrency} not available");
            }

            var usdAmmount = amount / resultforRate;

            var targetCurrencyAmmount = usdAmmount *resulttoRate;

            return Task.FromResult(targetCurrencyAmmount);

        }

        private readonly Dictionary<string, decimal> _rates = new Dictionary<string, decimal>
        {
            {"USD" ,1.0m },
            {"EUR" ,0.85m },
            {"NIS" ,3.20m }

        };
    }
}
