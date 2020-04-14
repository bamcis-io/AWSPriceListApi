using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Model
{
    /// <summary>
    /// Represents an individual version data
    /// </summary>
    public class VersionData
    {
        #region Public Properties

        public DateTime VersionEffectiveBeginDate { get; }

        public DateTime? VersionEffectiveEndDate { get; }

        public string OfferVersionUrl { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new version data object
        /// </summary>
        /// <param name="versionEffectiveBeginDate"></param>
        /// <param name="versionEffectiveEndDate"></param>
        /// <param name="offerVersionUrl"></param>
        [JsonConstructor]
        public VersionData(DateTime versionEffectiveBeginDate, DateTime? versionEffectiveEndDate, string offerVersionUrl)
        {
            if (String.IsNullOrEmpty(offerVersionUrl))
            {
                throw new ArgumentNullException(nameof(offerVersionUrl));
            }

            this.VersionEffectiveBeginDate = versionEffectiveBeginDate;
            this.VersionEffectiveEndDate = versionEffectiveEndDate;
            this.OfferVersionUrl = offerVersionUrl;
        }

        #endregion
    }
}
