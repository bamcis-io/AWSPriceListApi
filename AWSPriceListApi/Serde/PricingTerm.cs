using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BAMCIS.AWSPriceListApi.Serde
{
    /// <summary>
    /// A pricing term contained in an offer file with 1 or more price dimensions. The term
    /// is either an On Demand or Reserved term and the price dimensions compose the different
    /// parts of the pricing, like upfront fee or recurring cost
    /// </summary>
    public sealed class PricingTerm
    {
        #region Public Properties
        /// <summary>
        /// The unique code of this offer term for the product SKU
        /// </summary>
        public string OfferTermCode { get; }
        /// <summary>
        /// The SKU of the product
        /// </summary>
        public string Sku { get; }
        /// <summary>
        /// The date this pricing term became effective
        /// </summary>
        public DateTime EffectiveDate { get; }
        /// <summary>
        /// The price dimensions that make up this pricing term, these are keyed by their
        /// rate code which is SKU.OfferTerCode.Rate
        /// </summary>
        public IReadOnlyDictionary<string, PriceDimension> PriceDimensions { get; }
        /// <summary>
        /// The attributes for this term including lease contract length, purchase option, 
        /// and offering class. This is null for on demand terms.
        /// </summary>
        public TermAttributes TermAttributes { get; }

        #endregion
        #region Constructors
        /// <summary>
        /// Builds a new pricing term
        /// </summary>
        /// <param name="offerTermCode"></param>
        /// <param name="sku"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="priceDimensions"></param>
        /// <param name="termAttributes"></param>
        [JsonConstructor]
        public PricingTerm(
            string offerTermCode,
            string sku,
            DateTime effectiveDate,
            IDictionary<string, PriceDimension> priceDimensions,
            TermAttributes termAttributes
            )
        {
            this.OfferTermCode = offerTermCode;
            this.Sku = sku;
            this.EffectiveDate = effectiveDate;
            this.PriceDimensions = new ReadOnlyDictionary<string, PriceDimension>(priceDimensions);
            this.TermAttributes = termAttributes ?? throw new ArgumentNullException("termAttributes");
        }

        #endregion
    }
}
