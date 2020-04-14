using Amazon.Runtime;
using Amazon.Runtime.Internal.Auth;
using System;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// Represents a request to retrieve the offer index file
    /// </summary>
    public sealed class GetOfferIndexFileRequest : AmazonWebServiceRequest
    {
        #region Public Fields

        /// <summary>
        /// The default relative path to the offer index file
        /// </summary>
        public static readonly string DEFAULT_OFFER_INDEX_FILE_URL = "/offers/v1.0/aws/index.json";

        #endregion

        #region Public Properties

        /// <summary>
        /// The relative path of the offer index file url
        /// </summary>
        public string RelativePath { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new offer index file request using the default index file url
        /// </summary>
        public GetOfferIndexFileRequest()
        {
            this.RelativePath = DEFAULT_OFFER_INDEX_FILE_URL;
        }

        /// <summary>
        /// Creates a new offer index file request
        /// </summary>
        /// <param name="relativePath">The relative url path for the index file</param>
        public GetOfferIndexFileRequest(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentException("The relativePath cannot be null or empty.");
            }

            this.RelativePath = relativePath;
        }

        #endregion
    }
}
