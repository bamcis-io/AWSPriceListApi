using System;
using System.Net;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// The response from an offer index file request
    /// </summary>
    public sealed class GetOfferIndexFileResponse
    {
        #region Public Properties

        /// <summary>
        /// The http status code returned by AWS
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// The offer index file data
        /// </summary>
        public OfferIndexFile OfferIndexFile { get; }

        /// <summary>
        /// The reason of any error
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Indicates if an error occured
        /// </summary>
        public bool IsError { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new offer index file response
        /// </summary>
        /// <param name="file"></param>
        internal GetOfferIndexFileResponse(OfferIndexFile file, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            this.OfferIndexFile = file ?? throw new ArgumentNullException("file");
            this.StatusCode = statusCode;
            this.Reason = String.Empty;
            this.IsError = false;
        }

        /// <summary>
        /// Creates a response that was due to an error
        /// </summary>
        /// <param name="statusCode"></param>
        internal GetOfferIndexFileResponse(string reason, HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
            this.Reason = reason;
            this.IsError = true;
            this.OfferIndexFile = null;
        }

        #endregion
    }
}
