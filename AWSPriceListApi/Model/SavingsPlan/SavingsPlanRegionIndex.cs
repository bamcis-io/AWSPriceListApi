using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// Represents the Savings Plan region index data
    /// </summary>
    public class SavingsPlanRegionIndex
    {
        #region Public Properties

        public string FormatVersion { get; }

        public string Disclaimer { get; }

        public DateTime PublicationDate { get; }

        public IEnumerable<SavingsPlanRegionData> Regions { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds a new savings plan region index
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="disclaimer"></param>
        /// <param name="publicationDate"></param>
        /// <param name="regions"></param>
        [JsonConstructor]
        public SavingsPlanRegionIndex(string formatVersion, string disclaimer, DateTime publicationDate, IEnumerable<SavingsPlanRegionData> regions)
        {
            if (String.IsNullOrEmpty(formatVersion))
            {
                throw new ArgumentNullException("formatVersion");
            }

            if (String.IsNullOrEmpty(disclaimer))
            {
                throw new ArgumentNullException("disclaimer");
            }

            this.FormatVersion = formatVersion;
            this.Disclaimer = disclaimer;
            this.PublicationDate = publicationDate;
            this.Regions = regions ?? throw new ArgumentNullException("regions");
        }

        #endregion
    }
}
