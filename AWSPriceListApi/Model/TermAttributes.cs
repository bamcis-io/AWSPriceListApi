using BAMCIS.AWSPriceListApi.Serde;
using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Model
{
    /// <summary>
    /// A common set of pricing term attributes for a pricing term
    /// </summary>
    [JsonConverter(typeof(TermAttributesConverter))]
    public sealed class TermAttributes
    {
        #region Public Properties

        /// <summary>
        /// The lease contract length, i.e. the length of a reserved instance purchase
        /// </summary>
        [JsonConverter(typeof(LeaseContractLengthConverter))]
        public int LeaseContractLength { get; }

        /// <summary>
        /// The purchase option for the resource, OnDemand, Reserved, or Unknown
        /// </summary>
        public PurchaseOption PurchaseOption { get; }

        /// <summary>
        /// The offering class for the reserved term, either standard or convertible
        /// </summary>
        public OfferingClass OfferingClass { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new term attribute that defaults to a standard, on-demand term
        /// </summary>
        public TermAttributes()
        {
            this.LeaseContractLength = 0;
            this.PurchaseOption = PurchaseOption.ON_DEMAND;
            this.OfferingClass = OfferingClass.STANDARD;
        }

        /// <summary>
        /// Creates a new term attribute
        /// </summary>
        /// <param name="leaseContractLength"></param>
        /// <param name="purchaseOption"></param>
        /// <param name="offeringClass"></param>
        [JsonConstructor]
        public TermAttributes(
            int leaseContractLength,
            PurchaseOption purchaseOption,
            OfferingClass offeringClass
            )
        {
            if (leaseContractLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(leaseContractLength), "The lease contract length cannot be less than zero.");
            }

            this.LeaseContractLength = leaseContractLength;
            this.PurchaseOption = purchaseOption;
            this.OfferingClass = offeringClass;
        }

        #endregion
    }
}
