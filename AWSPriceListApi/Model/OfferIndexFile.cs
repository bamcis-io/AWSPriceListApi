using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.AWSPriceListApi.Model
{
    /// <summary>
    /// Represents the contents of an offer index file
    /// </summary>
    public class OfferIndexFile
    {
        #region Public Fields

        public static readonly Uri OfferIndexFileUrl = new Uri("https://pricing.us-east-1.amazonaws.com/offers/v1.0/aws/index.json");

        #endregion

        #region Public Properties

        public string FormatVersion { get; }

        public string Disclaimer { get; }

        public DateTime PublicationDate { get; }

        public IDictionary<string, Offer> Offers { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds a new offer index file
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="disclaimer"></param>
        /// <param name="publicationDate"></param>
        /// <param name="offers"></param>
        [JsonConstructor]
        public OfferIndexFile(
            string formatVersion, 
            string disclaimer, 
            DateTime publicationDate, 
            IDictionary<string, Offer> offers)
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
            this.Offers = offers;
        }

        #endregion
    }
}
