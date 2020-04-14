using System;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// The version data for a Savings Plan offer
    /// </summary>
    public class SavingsPlanVersionData
    {
        #region Public Properties

        /// <summary>
        /// The publication date of the version
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// The URL to the version's index
        /// </summary>
        public string OfferVersionUrl { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SavingsPlanVersionData() { }

        /// <summary>
        /// Creates a savings plan version data object
        /// </summary>
        /// <param name="publicationDate"></param>
        /// <param name="offerVersionUrl"></param>
        public SavingsPlanVersionData(DateTime publicationDate, string offerVersionUrl)
        {
            if (String.IsNullOrEmpty(offerVersionUrl))
            {
                throw new ArgumentNullException("offerVersionUrl");
            }

            this.PublicationDate = publicationDate;
            this.OfferVersionUrl = offerVersionUrl;
        }

        #endregion
    }
}
