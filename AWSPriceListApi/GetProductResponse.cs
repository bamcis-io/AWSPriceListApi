using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// A response for a get product request
    /// </summary>
    public sealed class GetProductResponse
    {
        #region Private Fields

        private static volatile object sync = new object();

        #endregion

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
        public Stream ProductInfo { get; }

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
        internal GetProductResponse(HttpResponseMessage response, GetProductRequest request, bool disposeResponse = false)
        {
            try
            {
                this.StatusCode = response.StatusCode;
                this.Format = request.Format;
                this.ServiceCode = request.ServiceCode;
                this.IsError = !response.IsSuccessStatusCode;
                this.ProductInfo = new MemoryStream();

                if (this.IsError)
                {
                    this.Reason = response.Content.ReadAsStringAsync().Result; 
                }
                else
                {
                    response.Content.LoadIntoBufferAsync().Wait();
                    response.Content.CopyToAsync(this.ProductInfo).Wait();
                    this.ProductInfo.Position = 0;
                    this.Reason = String.Empty;
                }
            }
            finally
            {
                if (disposeResponse)
                {
                    try
                    {
                        response.Content.Dispose();
                    }
                    catch { }

                    try
                    {
                        response.Dispose();
                    }
                    catch { }
                }
            }
        }

        #endregion

        #region Deconstructor

        ~GetProductResponse()
        {
            try
            {
                this.ProductInfo.Dispose();
            }
            catch
            { }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the product info stream as a string
        /// </summary>
        /// <returns></returns>
        public string GetProductInfoAsString()
        {
            lock (sync)
            {
                this.ProductInfo.Position = 0;
                using (StreamReader reader = new StreamReader(this.ProductInfo))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}
