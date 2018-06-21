﻿using System;
using System.Net;
using System.Net.Http;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// A response for a get product request
    /// </summary>
    public sealed class GetProductResponse
    {
        #region Public Properties

        /// <summary>
        /// The status code of the AWS response
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// The product that was requested
        /// </summary>
        public string ServiceCode { get; }

        /// <summary>
        /// The content of the product pricing information
        /// </summary>
        public string ProductInfo { get; }

        /// <summary>
        /// The format of the data
        /// </summary>
        public Format Format { get; }

        /// <summary>
        /// The reason for any error that occured
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Indicates if an error occured during processing
        /// </summary>
        public bool IsError { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a get product response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="format"></param>
        internal GetProductResponse(HttpResponseMessage response, GetProductRequest request)
        {
            this.StatusCode = response.StatusCode;           
            this.Format = request.Format;
            this.ServiceCode = request.ServiceCode;
            this.IsError = !response.IsSuccessStatusCode;

            if (this.IsError)
            {
                this.Reason = response.Content.ReadAsStringAsync().Result;
                this.ProductInfo = String.Empty;
            }
            else
            {
                this.ProductInfo = response.Content.ReadAsStringAsync().Result;
                this.Reason = String.Empty;
            }
        }

        #endregion
    }
}
