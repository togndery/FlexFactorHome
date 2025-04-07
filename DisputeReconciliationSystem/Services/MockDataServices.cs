using DisputeReconciliationSystem.Interface;
using DisputeReconciliationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Services
{
    public class MockDataServices : IDispute
    {
        public Task<List<Dispute>> GetAllDisputesAsync()
        {
            var mockData = new List<Dispute>
            {
                new Dispute { DisputeId = "case_001", TransactionId = "txn_001", Amount = 100.00m, Currency = "USD", Status = "Open", Reason = "Fraud" },
                new Dispute { DisputeId = "case_002", TransactionId = "txn_005", Amount = 150.00m, Currency = "USD", Status = "Lost", Reason = "Product Not Received" },
                new Dispute { DisputeId = "case_004", TransactionId = "txn_007", Amount = 90.00m, Currency = "USD", Status = "Open", Reason = "Unauthorized" }
            };

            return Task.FromResult(mockData);
        }
    }
}
