using DisputeReconciliationSystem.Interface;
using DisputeReconciliationSystem.Model;
using DisputeReconciliationSystem.Services;
using Moq;

namespace FlexFactorTest
{
    [TestClass]
    public sealed class ReconliationServicesTest
    {
        private ReconciliationService _reconciliationService;
        private Mock<ICurrencyConverter> _currencyConverter;

        [TestInitialize]
        public void Setup()
        {
            _currencyConverter = new Mock<ICurrencyConverter>();
            _currencyConverter.Setup(x => x.CurrencyConvertorAsync(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((decimal amount, string fromCurrency, string toCurrency) => amount);

            _reconciliationService = new ReconciliationService(_currencyConverter.Object);
        }
        [TestMethod]
        public async Task ReconcileDisputeNot_Create_Discrepancies()
        {
            var extDisputes = new List<Dispute>
            {
                new Dispute { DisputeId = "case_001", TransactionId = "txn_001", Amount = 100.00m, Currency = "USD", Status = "Open", Reason = "Fraud" }
            };
            var intlDisputes = new List<Dispute>
            {
                new Dispute { DisputeId = "case_001", TransactionId = "txn_001", Amount = 100.00m, Currency = "USD", Status = "Open", Reason = "Fraud" }
            };

            var result = await _reconciliationService.ReconcileDisputesAsync(extDisputes, intlDisputes);

            Assert.AreEqual(1, result.TotalExternalDisputes);
            Assert.AreEqual(1, result.TotalInternalDisputes);
            Assert.AreEqual(1, result.MatchedDisputes);
            Assert.AreEqual(0, result.Discrepancies.Count);

        }
        [TestMethod]
        public async Task ReconcileDisputes_MissingInternal()
        {
            // Arrange
            var extDisputes = new List<Dispute>
            {
                new Dispute { DisputeId = "case_001", TransactionId = "txn_001", Amount = 100.00m, Currency = "USD", Status = "Open", Reason = "Fraud" },
                new Dispute { DisputeId = "case_002", TransactionId = "txn_002", Amount = 50.00m, Currency = "USD", Status = "Open", Reason = "Fraud" }
            };

            var intlDisputes = new List<Dispute>
            {
                new Dispute { DisputeId = "case_001", TransactionId = "txn_001", Amount = 100.00m, Currency = "USD", Status = "Open", Reason = "Fraud" }
            };

            // Act
            var serviceData = await _reconciliationService.ReconcileDisputesAsync(extDisputes, intlDisputes);

            // Assert
            Assert.AreEqual(2, serviceData.TotalExternalDisputes);
            Assert.AreEqual(1, serviceData.TotalInternalDisputes);
            Assert.AreEqual(1, serviceData.MatchedDisputes);
            Assert.AreEqual(1, serviceData.Discrepancies.Count);
            Assert.AreEqual(DiscrepancyType.MissingInInternal, serviceData.Discrepancies[0].Type);
            Assert.AreEqual("case_002", serviceData.Discrepancies[0].DisputeId);
            Assert.AreEqual(DiscrepancySeverity.High, serviceData.Discrepancies[0].Severity);
        }
    }
}
