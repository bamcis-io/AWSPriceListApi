using BAMCIS.AWSPriceListApi.Serde;
using Newtonsoft.Json;

namespace BAMCIS.AWSPriceListApi.Model
{
    /// <summary>
    /// The different purchase options for AWS pricing terms
    /// </summary>
    [JsonConverter(typeof(PurchaseOptionConverter))]
    public enum PurchaseOption
    {
        /// <summary>
        /// The purchase option is unknown
        /// </summary>
        UNKNOWN = 99,

        /// <summary>
        /// The purchase option is On Demand
        /// </summary>
        ON_DEMAND = 98,

        /// <summary>
        /// No Upfront reserved instance
        /// </summary>
        NO_UPFRONT = 0,

        /// <summary>
        /// Partial Upfront reserved instance
        /// </summary>
        PARTIAL_UPFRONT = 1,

        /// <summary>
        /// All Upfront reserved instance
        /// </summary>
        ALL_UPFRONT = 2,

        /// <summary>
        /// Light Utilization ElastiCache reserved instance
        /// </summary>
        LIGHT_UTILIZATION = 10,

        /// <summary>
        /// Medium Utilization ElastiCache reserved instance
        /// </summary>
        MEDIUM_UTILIZATION = 11,

        /// <summary>
        /// Heavy Utilization ElastiCache reserved instance
        /// </summary>
        HEAVY_UTILIZATION = 12
    }
}
