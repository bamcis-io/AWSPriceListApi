using Amazon.Runtime;
using BAMCIS.AWSPriceListApi.Model;
using System;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// A request for the savings plan version index data
    /// </summary>
    public class GetSavingsPlanVersionIndexRequest : AmazonWebServiceRequest
    {
        #region Public Fields

        public static readonly string DEFAULT_SAVINGS_PLAN_VERSION_URL = "/savingsPlan/v1.0/aws/AWSComputeSavingsPlan/current/region_index.json";

        #endregion

        #region Public Properties

        /// <summary>
        /// The relative path to the regional savings plans version index file, i.e. "/savingsPlan/v1.0/aws/AWSComputeSavingsPlan/current/region_index.json"
        /// </summary>
        public string SavingsPlanVersionIndexUrl { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor that uses the default relative path
        /// </summary>
        public GetSavingsPlanVersionIndexRequest()
        {
            this.SavingsPlanVersionIndexUrl = DEFAULT_SAVINGS_PLAN_VERSION_URL;
        }

        public GetSavingsPlanVersionIndexRequest(string savingsPlanVersionIndexUrl)
        {
            if (String.IsNullOrEmpty(savingsPlanVersionIndexUrl))
            {
                throw new ArgumentNullException("savingsPlanVersionIndexUrl");
            }

            this.SavingsPlanVersionIndexUrl = savingsPlanVersionIndexUrl;
        }

        public GetSavingsPlanVersionIndexRequest(Offer offer)
        {
            if (offer == null)
            {
                throw new ArgumentNullException("offer");
            }

            if (String.IsNullOrEmpty(offer.SavingsPlanVersionIndexUrl))
            {
                throw new ArgumentException("The SavingsPlanVersionIndexUrl of the offer was null or empty.");
            }

            this.SavingsPlanVersionIndexUrl = offer.SavingsPlanVersionIndexUrl;
        }

        #endregion
    }
}
