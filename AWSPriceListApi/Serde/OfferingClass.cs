using Newtonsoft.Json;

namespace BAMCIS.AWSPriceListApi.Serde
{
    /// <summary>
    /// The different offering classes for AWS reserved instances
    /// </summary>
    [JsonConverter(typeof(OfferingClassConverter))]
    public enum OfferingClass
    {
        /// <summary>
        /// The standard offering class
        /// </summary>
        STANDARD = 0,

        /// <summary>
        /// The convertible offering class, these reserved instances can be converted
        /// to cover instances of smaller or larger types in the same family
        /// </summary>
        CONVERTIBLE = 1,

        /// <summary>
        /// The offering class is unknown
        /// </summary>
        UNKNOWN = 2
    }
}
