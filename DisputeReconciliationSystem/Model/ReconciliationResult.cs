using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Model
{
    public class ReconciliationResult
    {
        public DateTime ReconciliationDate { get; set; }
        public int TotalExternalDisputes { get; set; }
        public int TotalInternalDisputes { get; set; }
        public int MatchedDisputes { get; set; }
        public List<Discrepancy> Discrepancies { get; set; } = new List<Discrepancy>();
    }
}
