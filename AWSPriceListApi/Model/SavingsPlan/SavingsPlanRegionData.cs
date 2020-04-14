using System;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// The data in the Savings Plan region index
    /// </summary>
    public class SavingsPlanRegionData
    {
        #region Public Properties

        /// <summary>
        /// The region code, like us-east-1
        /// </summary>
        public string RegionCode { get; set; }

        /// <summary>
        /// The relative path of the url to the index file for this region
        /// </summary>
        public string VersionUrl { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SavingsPlanRegionData()
        { }

        public SavingsPlanRegionData(string regionCode, string versionUrl)
        {
            if (string.IsNullOrEmpty(regionCode))
            {
                throw new ArgumentNullException("regionCode");
            }

            if (string.IsNullOrEmpty(versionUrl))
            {
                throw new ArgumentNullException("versionUrl");
            }

            this.RegionCode = regionCode;
            this.VersionUrl = versionUrl;
        }

        #endregion
    }
}
