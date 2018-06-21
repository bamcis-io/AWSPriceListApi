using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BAMCIS.AWSPriceListApi.Serde
{
    /// <summary>
    /// Represents a price dimension inside an AWS offer term, this is a specific component to pricing
    /// the term. It might be an On Demand dimension, in which case there is only 1 dimesion for the term, or it
    /// could be the upfront fee dimesions for a reserved term or the monthly recurring fee for a reserved term
    /// </summary>
    public class PriceDimension
    {
        #region Public Properties

        /// <summary>
        /// The rate code for this price dimesion
        /// </summary>
        public string RateCode { get; }

        /// <summary>
        /// The description of the price dimesion, typically what is seen for the ItemDescription
        /// in the billing file
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The beginning range of when this pricing applies, typically 0
        /// </summary>
        public string BeginRange { get; }

        /// <summary>
        /// The end range of when this pricing applies, typically Inf
        /// </summary>
        public string EndRange { get; }

        /// <summary>
        /// The unit of the pricing, typically Quantity
        /// </summary>
        public string Unit { get; }

        /// <summary>
        /// The price per unit, although this is a dictionary, it typically only 1 key value pair, 
        /// the pricing denomination, like USD, and the actual cost
        /// </summary>
        public IReadOnlyDictionary<string, string> PricePerUnit { get; }

        /// <summary>
        /// What this pricing dimension applies to, this is usually an empty array, but contains product SKUs
        /// for global pricing dimensions
        /// </summary>
        public IReadOnlyCollection<string> AppliesTo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds a new price dimension
        /// </summary>
        /// <param name="rateCode"></param>
        /// <param name="description"></param>
        /// <param name="beginRange"></param>
        /// <param name="endRange"></param>
        /// <param name="unit"></param>
        /// <param name="pricePerUnit"></param>
        /// <param name="appliesTo"></param>
        [JsonConstructor]
        public PriceDimension(
            string rateCode,
            string description,
            string beginRange,
            string endRange,
            string unit,
            IDictionary<string, string> pricePerUnit,
            IList<string> appliesTo
            )
        {
            this.RateCode = rateCode;
            this.Description = description;
            this.BeginRange = beginRange;
            this.EndRange = endRange;
            this.Unit = unit;
            this.PricePerUnit = (pricePerUnit == null) ? new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()) : new ReadOnlyDictionary<string, string>(pricePerUnit);
            this.AppliesTo = (appliesTo == null) ? new ReadOnlyCollection<string>(new string[0]) : new ReadOnlyCollection<string>(appliesTo);
        }

        #endregion
    }
}
