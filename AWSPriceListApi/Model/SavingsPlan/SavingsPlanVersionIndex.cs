using System;
using System.Collections.Generic;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// Represents the Savings Plan version index data
    /// </summary>
    public class SavingsPlanVersionIndex
    {
        #region Public Properties

        public string FormatVersion { get; }

        public string Disclaimer { get; }

        public DateTime PublicationDate { get; }

        public string CurrentOfferVersionUrl { get; }

        public IEnumerable<SavingsPlanVersionData> Versions { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new version index
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="disclaimer"></param>
        /// <param name="publicationDate"></param>
        /// <param name="currentOfferVersionUrl"></param>
        /// <param name="versions"></param>
        public SavingsPlanVersionIndex(
            string formatVersion,
            string disclaimer,
            DateTime publicationDate,
            string currentOfferVersionUrl,
            IEnumerable<SavingsPlanVersionData> versions
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

            if (String.IsNullOrEmpty(currentOfferVersionUrl))
            {
                throw new ArgumentNullException("currentOfferVersionUrl");
            }

            this.FormatVersion = formatVersion;
            this.Disclaimer = disclaimer;
            this.PublicationDate = publicationDate;
            this.CurrentOfferVersionUrl = currentOfferVersionUrl;
            this.Versions = versions ?? throw new ArgumentNullException("versions");
        }

        #endregion
    }
}
