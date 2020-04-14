using Amazon.Runtime;
using BAMCIS.AWSPriceListApi.Model;
using System;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// A request for the region index for a service, which will provide a list of URLs for each region, which can be
    /// used to retrieve Offer information for the service in that region
    /// </summary>
    public class GetRegionIndexRequest : AmazonWebServiceRequest
    {
        #region Public Properties

        /// <summary>
        /// The region index url for a specific service, i.e. "/offers/v1.0/aws/AmazonEC2/current/region_index.json"
        /// </summary>
        public string CurrentRegionIndexUrl { get; set; }

        #endregion

        #region Constructors

        public GetRegionIndexRequest(string currentRegionIndexUrl)
        {
            if (String.IsNullOrEmpty(currentRegionIndexUrl))
            {
                throw new ArgumentNullException("currentRegionIndexUrl");
            }

            this.CurrentRegionIndexUrl = currentRegionIndexUrl;
        }

        public GetRegionIndexRequest(Offer offer)
        {
            if (offer == null)
            {
                throw new ArgumentNullException("offer");
            }

            if (String.IsNullOrEmpty(offer.CurrentRegionIndexUrl))
            {
                throw new ArgumentException("The CurrentRegionIndexUrl of the offer was null or empty.");
            }

            this.CurrentRegionIndexUrl = offer.CurrentRegionIndexUrl;
        }

        #endregion
    }
}
