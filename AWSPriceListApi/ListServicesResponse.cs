using System;
using System.Collections.Generic;
using System.Net;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// A response for a list services request
    /// </summary>
    public sealed class ListServicesResponse
    {
        #region Public Properties

        /// <summary>
        /// The status code of the AWS response
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// The services that have available pricing data
        /// </summary>
        public IEnumerable<string> Services { get; }

        /// <summary>
        /// The reason of any error that occured
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Indicates if an error occured
        /// </summary>
        public bool IsError { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new list services response
        /// </summary>
        /// <param name="services"></param>
        internal ListServicesResponse(IEnumerable<string> services)
        {
            this.StatusCode = HttpStatusCode.OK;
            this.Services = services;
            this.Reason = String.Empty;
            this.IsError = false;
        }

        /// <summary>
        /// Creates a new list services response that was an error
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="statusCode"></param>
        internal ListServicesResponse(string reason, HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
            this.Services = null;
            this.Reason = reason;
            this.IsError = true;
        }

        #endregion
    }
}
