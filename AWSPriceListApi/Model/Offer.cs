using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Model
{
    /// <summary>
    /// Represents an individual service inside the offer index file
    /// </summary>
    public class Offer
    {
        #region Public Properties

        /// <summary>
        /// A unique code for the product of an AWS service. For example, AmazonEC2 or AmazonS3. The OfferCode is used as the lookup key for the index. 
        /// </summary>
        public string OfferCode { get; }

        /// <summary>
        /// The list of available versions of the index file
        /// </summary>
        public string VersionIndexUrl { get; }

        /// <summary>
        /// The URL where you can download the most up-to-date offer file. 
        /// </summary>
        public string CurrentVersionUrl { get; }

        /// <summary>
        /// A list of available regional offer files.
        /// </summary>
        public string CurrentRegionIndexUrl { get; }

        /// <summary>
        /// The list of applicable Savings Plan offers.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SavingsPlanVersionIndexUrl { get; }

        /// <summary>
        /// The list of regional offer files for Savings Plans
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentSavingsPlanIndexUrl { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor to create an individual offer
        /// </summary>
        /// <param name="offerCode">The offer code</param>
        /// <param name="versionIndexUrl">The version index url</param>
        /// <param name="currentVersionUrl">The current verion url</param>
        /// <param name="currentRegionIndexUrl">The current region index url</param>
        [JsonConstructor]
        public Offer(
            string offerCode, 
            string versionIndexUrl, 
            string currentVersionUrl, 
            string currentRegionIndexUrl,
            string savingsPlanVersionIndexUrl,
            string currentSavingsPlanIndexUrl)
        {
            if (String.IsNullOrEmpty(offerCode))
            {
                throw new ArgumentNullException("offerCode");
            }

            if (String.IsNullOrEmpty(versionIndexUrl))
            {
                throw new ArgumentNullException("versionIndexUrl");
            }

            if (String.IsNullOrEmpty(currentVersionUrl))
            {
                throw new ArgumentNullException("currentVersionUrl");
            }

            this.OfferCode = offerCode;
            this.VersionIndexUrl = versionIndexUrl;
            this.CurrentVersionUrl = currentVersionUrl;
            this.CurrentRegionIndexUrl = currentRegionIndexUrl ?? String.Empty; // This can be null
            this.SavingsPlanVersionIndexUrl = savingsPlanVersionIndexUrl ?? String.Empty; // This can be null or missing for most services
            this.CurrentSavingsPlanIndexUrl = currentSavingsPlanIndexUrl ?? String.Empty; // This can be null or missing for most services
        }

        #endregion
    }
}
