using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Interface
{
    public interface ICurrencyConverter
    {
        Task<decimal> CurrencyConvertorAsync(decimal amount, string fromCurrency, string toCurrency);
    }
}
