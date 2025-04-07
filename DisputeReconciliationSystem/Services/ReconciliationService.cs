using DisputeReconciliationSystem.Interface;
using DisputeReconciliationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeReconciliationSystem.Services
{
    public class ReconciliationService : IReconciliationService
    {
        private readonly ICurrencyConverter _currencyConverter;
        

        public ReconciliationService(ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="externalDisputes"></param>
        /// <param name="internalDisputes"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ReconciliationResult> ReconcileDisputesAsync(List<Dispute> externalDisputes, List<Dispute> internalDisputes)
        {
            
                var reconcliation = new ReconciliationResult
                {
                    ReconciliationDate = DateTime.Now,
                    TotalExternalDisputes = externalDisputes.Count,
                    TotalInternalDisputes = internalDisputes.Count,
                };

                //lookup for faster access

                var exDisputMap = externalDisputes.ToDictionary(d => d.DisputeId);
                var inDisputeMap = internalDisputes.ToDictionary(d => d.DisputeId);


                //check missing disputes in interanl system 
                foreach (var item in externalDisputes)
                {
                    if (!inDisputeMap.ContainsKey(item.DisputeId))
                    {
                        reconcliation.Discrepancies.Add(new Discrepancy
                        {
                            ExternalDispute = item,
                            Description = $"Dispute {item.DisputeId} found in external report but missing in interanl",
                            Severity = DiscrepancySeverity.High,
                            Type = DiscrepancyType.MissingInInternal,
                            DisputeId = item.DisputeId,

                        });
                    }
                }

                //check for dusputes missing in the report
                foreach (var item in internalDisputes)
                {
                    if (!exDisputMap.ContainsKey(item.DisputeId))
                    {
                        reconcliation.Discrepancies.Add(new Discrepancy
                        {
                            InternalDispute = item,
                            Description = $"Dispute{item.DisputeId} fount in internal system but missing in external",
                            Severity = DiscrepancySeverity.Medium,
                            Type = DiscrepancyType.MissingInExternal,
                            DisputeId = item.DisputeId,

                        });
                    }
                }

                //verify that boot system are align 
                foreach (var item in externalDisputes)
                {
                    if (inDisputeMap.TryGetValue(item.DisputeId, out var intDispute))
                    {
                        reconcliation.MatchedDisputes++;


                        //verify  status 
                        if (item.Status != intDispute.Status)
                        {
                            reconcliation.Discrepancies.Add(new Discrepancy
                            {
                                ExternalDispute = item,
                                InternalDispute = intDispute,
                                DisputeId = item.DisputeId,
                                Type = DiscrepancyType.StatusMismatch,
                                Severity = DiscrepancySeverity.High,
                                Description = $"Status mismatch for dispute {item.DisputeId}: External={item.Status}, Internal={intDispute.Status}",

                            });
                        }

                        //verify amount 
                        if (item.Currency == intDispute.Currency)
                        {
                            if (item.Amount != intDispute.Amount)
                            {
                                reconcliation.Discrepancies.Add(new Discrepancy
                                {
                                    Description = $"Amount mismatch for dispute {item.DisputeId}: External={item.Amount} {item.Currency}, Internal={intDispute.Amount} {intDispute.Currency}",
                                    DisputeId = item.DisputeId,
                                    Type = DiscrepancyType.AmountMismatch,
                                    Severity = DiscrepancySeverity.High,
                                    ExternalDispute = item,
                                    InternalDispute = intDispute
                                });
                            }
                        }
                        else
                        {
                            //currencies for comparison
                            var ammount = await _currencyConverter.CurrencyConvertorAsync
                                (
                                   item.Amount,
                                   item.Currency,
                                   intDispute.Currency

                                );

                            //rounding due rate conversions
                            if (Math.Abs(ammount - intDispute.Amount) > 0.01m)
                            {
                                reconcliation.Discrepancies.Add(new Discrepancy
                                {
                                    Description = $"Amount mismatch for dispute {item.DisputeId}: External={item.Amount} {item.Currency} (converted: {ammount} {intDispute.Currency}), Internal={intDispute.Amount} {intDispute.Currency}",
                                    ExternalDispute = item,
                                    InternalDispute = intDispute,
                                    DisputeId = item.DisputeId,
                                    Type = DiscrepancyType.AmountMismatch,
                                    Severity = DiscrepancySeverity.High,
                                });
                            }

                        }
                        //verify reason mismatch
                        if (item.Reason != intDispute.Reason)
                        {
                            reconcliation.Discrepancies.Add(new Discrepancy
                            {
                                ExternalDispute = item,
                                InternalDispute = intDispute,
                                Type = DiscrepancyType.ReasonMismatch,
                                Severity = DiscrepancySeverity.Medium,
                                Description = $"Reason mismatch for dispute {item.DisputeId}: External={item.Reason}, Internal={intDispute.Reason}",
                                DisputeId = item.DisputeId,

                            });
                        }

                    }
               
            }
            return reconcliation;


        }
    }
}
