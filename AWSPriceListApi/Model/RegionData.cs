using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Model
{
    /// <summary>
    /// The data for a specific region
    /// </summary>
    public class RegionData
    {
        #region Public Properties

        /// <summary>
        /// The region code, like us-east-1
        /// </summary>
        public string RegionCode { get; }

        /// <summary>
        /// The url to the current version of the offer index file for this region
        /// </summary>
        public string CurrentVersionUrl { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds a new region data object
        /// </summary>
        /// <param name="regionCode">The region code, like us-east-1</param>
        /// <param name="currentVersionUrl">The current version url</param>
        [JsonConstructor]
        public RegionData(string regionCode, string currentVersionUrl)
        {
            if (String.IsNullOrEmpty(regionCode))
            {
                throw new ArgumentNullException(nameof(regionCode));
            }

            if (String.IsNullOrEmpty(currentVersionUrl))
            {
                throw new ArgumentNullException(nameof(currentVersionUrl));
            }

            this.RegionCode = regionCode;
            this.CurrentVersionUrl = currentVersionUrl;
        }

        #endregion
    }
}
