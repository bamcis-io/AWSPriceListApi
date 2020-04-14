using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// Represents the contents of a savings plan offer file
    /// </summary>
    public class SavingsPlanOffer
    {
        #region Public Properties

        public string FormatVersion { get; }

        public string Disclaimer { get; }

        public string OfferCode { get; }

        public string RegionCode { get; }

        public string Version { get; }

        public DateTime PublicationDate { get; }

        public IEnumerable<SavingsPlanProduct> Products { get; }

        public SavingsPlanOfferTerms Terms { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SavingsPlanOffer(
            string formatVersion,
            string disclaimer,
            string offerCode,
            string regionCode,
            string version,
            DateTime publicationDate,
            IEnumerable<SavingsPlanProduct> products,
            SavingsPlanOfferTerms terms)
        {
            if (String.IsNullOrEmpty(formatVersion))
            {
                throw new ArgumentNullException(nameof(formatVersion));
            }

            if (String.IsNullOrEmpty(disclaimer))
            {
                throw new ArgumentNullException(nameof(disclaimer));
            }

            if (String.IsNullOrEmpty(offerCode))
            {
                throw new ArgumentNullException(nameof(offerCode));
            }

            if (String.IsNullOrEmpty(regionCode))
            {
                throw new ArgumentNullException(nameof(regionCode));
            }

            if (String.IsNullOrEmpty(version))
            {
                throw new ArgumentNullException(nameof(version));
            }

            this.FormatVersion = formatVersion;
            this.Disclaimer = disclaimer;
            this.OfferCode = offerCode;
            this.RegionCode = regionCode;
            this.Version = version;
            this.PublicationDate = publicationDate;
            this.Products = products ?? throw new ArgumentNullException(nameof(products));
            this.Terms = terms ?? throw new ArgumentNullException(nameof(terms));
        }

        #endregion
    }
}
