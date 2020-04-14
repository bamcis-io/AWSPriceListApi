using Amazon.Runtime;
using BAMCIS.AWSPriceListApi.Model;
using System;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// Request for a savings plan index file
    /// </summary>
    public class GetSavingsPlanIndexFileRequest : AmazonWebServiceRequest
    {
        #region Public Fields

        public static readonly string DEFAULT_CURRENT_SAVINGS_PLAN_INDEX_URL = "/savingsPlan/v1.0/aws/AWSComputeSavingsPlan/current/region_index.json";

        #endregion

        #region Public Properties

        /// <summary>
        /// The relative path to the regional savings plans index file, i.e. "/savingsPlan/v1.0/aws/AWSComputeSavingsPlan/current/region_index.json"
        /// </summary>
        public string CurrentSavingsPlanIndexUrl { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a request with the default url
        /// </summary>
        public GetSavingsPlanIndexFileRequest()
        {
            this.CurrentSavingsPlanIndexUrl = DEFAULT_CURRENT_SAVINGS_PLAN_INDEX_URL;
        }

        public GetSavingsPlanIndexFileRequest(string currentSavingsPlanIndexUrl)
        {
            if (String.IsNullOrEmpty(currentSavingsPlanIndexUrl))
            {
                throw new ArgumentNullException("currentSavingsPlanIndexUrl");
            }

            this.CurrentSavingsPlanIndexUrl = currentSavingsPlanIndexUrl;
        }

        public GetSavingsPlanIndexFileRequest(Offer offer)
        {
            if (offer == null)
            {
                throw new ArgumentNullException("offer");
            }

            if (String.IsNullOrEmpty(offer.CurrentSavingsPlanIndexUrl))
            {
                throw new ArgumentException("The CurrentSavingsPlanIndexUrl of the offer was null or empty.");
            }

            this.CurrentSavingsPlanIndexUrl = offer.CurrentSavingsPlanIndexUrl;
        }

        #endregion
    }
}
