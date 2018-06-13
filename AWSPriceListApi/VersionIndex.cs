using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// An index of the available versions of the price data
    /// </summary>
    public sealed class VersionIndex
    {
        #region Public Properties

        public string FormatVersion { get; }

        public string Disclaimer { get; }

        public DateTime PublicationDate { get; }

        public string OfferCode { get; }

        public string CurrentVersion { get; }

        public IDictionary<string, VersionData> Versions { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new version index
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="disclaimer"></param>
        /// <param name="publicationDate"></param>
        /// <param name="offerCode"></param>
        /// <param name="currentVersion"></param>
        /// <param name="versions"></param>
        public VersionIndex(
            string formatVersion, 
            string disclaimer, 
            DateTime publicationDate, 
            string offerCode, 
            string currentVersion,
            IDictionary<string, VersionData> versions
        )
        {
            if (String.IsNullOrEmpty(formatVersion))
            {
                throw new ArgumentNullException("formatVersion");
            }

            if (String.IsNullOrEmpty(disclaimer))
            {
                throw new ArgumentNullException("disclaimer");
            }

            if (String.IsNullOrEmpty(offerCode))
            {
                throw new ArgumentNullException("offerCode");
            }

            if (String.IsNullOrEmpty(currentVersion))
            {
                throw new ArgumentNullException("currentVersion");
            }

            this.FormatVersion = formatVersion;
            this.Disclaimer = disclaimer;
            this.PublicationDate = publicationDate;
            this.OfferCode = offerCode;
            this.CurrentVersion = currentVersion;
            this.Versions = versions ?? throw new ArgumentNullException("versions");
        }

        #endregion
    }
}
