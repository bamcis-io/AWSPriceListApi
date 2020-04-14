using BAMCIS.AWSPriceListApi.Serde;
using Newtonsoft.Json;

namespace BAMCIS.AWSPriceListApi.Model
{
    /// <summary>
    /// The terms for which something can be purchased from AWS, either RESERVED capacity or ON DEMAND
    /// </summary>
    [JsonConverter(typeof(TermConverter))]
    public enum Term
    {
        /// <summary>
        /// On Demand purchase term
        /// </summary>
        ON_DEMAND,

        /// <summary>
        /// Reserved purchased term
        /// </summary>
        RESERVED,

        /// <summary>
        /// The purchase term is unknown
        /// </summary>
        UNKNOWN
    }
}
