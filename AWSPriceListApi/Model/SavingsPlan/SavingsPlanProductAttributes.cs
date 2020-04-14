using BAMCIS.AWSPriceListApi.Serde;
using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// Represents the standard attributes for a savings plan product
    /// </summary>
    public class SavingsPlanProductAttributes
    {
        #region Public Properties

        /// <summary>
        /// How the savings plan is purchased, all upfront, partial upfront, etc
        /// </summary>
        public PurchaseOption PurchaseOption { get; }

        /// <summary>
        /// The granularity the savings are applied
        /// </summary>
        public string Granularity { get; }

        /// <summary>
        /// The instance family, like r5 or c4, this property might not
        /// exist for some products, i.e. plans that apply to any region
        /// and any instance type
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InstanceType { get; }

        [JsonConverter(typeof(LeaseContractLengthConverter))]
        public int PurchaseTerm { get; }

        /// <summary>
        /// The location type, i.e. AWS Region
        /// </summary>
        public string LocationType { get; }

        /// <summary>
        /// The location of the product
        /// </summary>
        public string Location { get; }

        #endregion

        #region Constructors

        public SavingsPlanProductAttributes(
            PurchaseOption purchaseOption,
            string granularity,
            string instanceType,
            int purchaseTerm,
            string locationType,
            string location
        )
        {
            if (purchaseTerm < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(purchaseTerm));
            }

            if (String.IsNullOrEmpty(granularity))
            {
                throw new ArgumentNullException(nameof(granularity));
            }

            if (String.IsNullOrEmpty(locationType))
            {
                throw new ArgumentNullException(nameof(locationType));
            }

            if (String.IsNullOrEmpty(location))
            {
                throw new ArgumentNullException(nameof(location));
            }

            this.PurchaseOption = purchaseOption;
            this.Granularity = granularity;
            this.InstanceType = instanceType;
            this.PurchaseTerm = purchaseTerm;
            this.LocationType = locationType;
            this.Location = location;
        }

        #endregion
    }
}
