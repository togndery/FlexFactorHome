﻿using DisputeReconciliationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Interface
{
    public interface IAlertService
    {
        Task SendAlertAsync(Discrepancy discrepancy);
    }
}
