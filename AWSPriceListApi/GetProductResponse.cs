using BAMCIS.AWSPriceListApi.Model;
using System.Net.Http;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// A response for a get product request
    /// </summary>
    public sealed class GetProductResponse : AWSPriceListApiResponse<ProductOffer>
    {
        #region Public Properties

        /// <summary>
        /// The product that was requested
        /// </summary>
        public string ServiceCode { get; }

        /// <summary>
        /// The product offer containing all price list data about the service
        /// </summary>
        public ProductOffer ProductOffer
        {
            get
            {
                return this.Data;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a get product response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="format"></param>
        internal GetProductResponse(HttpResponseMessage response, Format format, string service) : base(response, format)
        {
            this.ServiceCode = service;
        }

        #endregion
    }
}
