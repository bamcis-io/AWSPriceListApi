using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// The discounted savings plan rate
    /// </summary>
    public class SavingsPlanRate
    {
        #region Public Properties

        public string DiscountedSku { get; }

        public string DiscountedUsageType { get; }

        public string DiscountedOperation { get; }

        public string DiscountedServiceCode { get; }

        public string RateCode { get; }

        public string Unit { get; }

        public SavingsPlanDiscountedRate DiscountedRate { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SavingsPlanRate(
            string discountedSku,
            string discountedUsageType,
            string discountedOperation,
            string discountedServiceCode,
            string rateCode,
            string unit,
            SavingsPlanDiscountedRate discountedRate)
        {
            this.DiscountedSku = discountedSku;
            this.DiscountedUsageType = discountedUsageType;
            this.DiscountedOperation = discountedOperation;
            this.DiscountedServiceCode = discountedServiceCode;
            this.RateCode = rateCode;
            this.Unit = unit;
            this.DiscountedRate = discountedRate ?? throw new ArgumentNullException("discountedRate");
        }

        #endregion
    }
}
