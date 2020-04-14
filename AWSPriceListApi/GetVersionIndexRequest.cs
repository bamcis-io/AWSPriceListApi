using Amazon.Runtime;
using BAMCIS.AWSPriceListApi.Model;
using System;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// A request for the version index
    /// </summary>
    public class GetVersionIndexRequest : AmazonWebServiceRequest
    {
        #region Public Properties

        /// <summary>
        /// The url to the the version index
        /// </summary>
        public string VersionIndexUrl { get; set; }

        #endregion

        #region Constructors

        public GetVersionIndexRequest(string versionIndexUrl)
        {
            if (String.IsNullOrEmpty(versionIndexUrl))
            {
                throw new ArgumentNullException("versionIndexUrl");
            }

            this.VersionIndexUrl = versionIndexUrl;
        }

        public GetVersionIndexRequest(Offer offer)
        {
            if (offer == null)
            {
                throw new ArgumentNullException("offer");
            }

            if (String.IsNullOrEmpty(offer.VersionIndexUrl))
            {
                throw new ArgumentException("The VersionIndexUrl of the offer was null or empty.");
            }

            this.VersionIndexUrl = offer.VersionIndexUrl;
        }

        #endregion
    }
}
