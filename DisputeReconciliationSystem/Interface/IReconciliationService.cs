using DisputeReconciliationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Interface
{
    public interface IReconciliationService
    {
        Task<ReconciliationResult> ReconcileDisputesAsync(List<Dispute> externalDisputes, List<Dispute> internalDisputes);
    }
}
