using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Model
{
    public class Discrepancy
    {
        public string DisputeId { get; set; }
        public DiscrepancyType Type { get; set; }
        public DiscrepancySeverity Severity { get; set; }
        public string Description { get; set; }
        public Dispute ExternalDispute { get; set; }
        public Dispute InternalDispute { get; set; }
    }
}
