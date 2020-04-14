using System;
using System.Net;
using System.Net.Http;

namespace BAMCIS.AWSPriceListApi.Model
{
    /// <summary>
    /// Contains information about exceptions that occur when
    /// accessing the price list API
    /// </summary>
    public class PriceListException : Exception
    {
        #region Public Properties

        /// <summary>
        /// The status code returned by AWS
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// The reason associated with the failed request
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// If available, the HTTP request that caused the bad http response
        /// </summary>
        public HttpRequestMessage Request { get; set; }

        #endregion

        #region Constructors

        public PriceListException(string message) : base(message)
        {
            this.Reason = message;
        }

        public PriceListException(string message, HttpStatusCode statusCode) : this(message)
        {
            this.StatusCode = statusCode;
        }

        public PriceListException(string message, Exception innerException) : base(message, innerException)
        {
            this.Reason = message;
        }

        public PriceListException(string message, Exception innerException, HttpStatusCode statusCode) : this(message, innerException)
        {
            this.StatusCode = statusCode;
        }

        public PriceListException(HttpResponseMessage response, string message) : this(message)
        {
            this.Reason = response.ReasonPhrase;
            this.Request = response.RequestMessage;
        }

        #endregion
    }
}
