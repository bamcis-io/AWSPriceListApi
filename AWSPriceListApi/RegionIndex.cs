using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// The region offer index data
    /// </summary>
    public sealed class RegionIndex
    {
        #region Public Properties

        public string FormatVersion { get; }

        public string Disclaimer { get; }

        public DateTime PublicationDate { get; }

        public IDictionary<string, RegionData> Regions { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds a new region index data
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="disclaimer"></param>
        /// <param name="publicationDate"></param>
        /// <param name="regions"></param>
        [JsonConstructor]
        public RegionIndex(string formatVersion, string disclaimer, DateTime publicationDate, IDictionary<string, RegionData> regions)
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
