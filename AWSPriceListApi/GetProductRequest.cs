using System;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// A request for a specific product price data
    /// </summary>
    public sealed class GetProductRequest
    {
        #region Public Properties

        /// <summary>
        /// The product to retrieve pricing data for
        /// </summary>
        public string ServiceCode { get; }

        /// <summary>
        /// The relative path to the offer file for the product
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// The format you want the data returned in, either json or csv
        /// </summary>
        public Format Format { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new product request
        /// </summary>
        /// <param name="product"></param>
        public GetProductRequest(string product)
        {
            if (String.IsNullOrEmpty(product))
            {
                throw new ArgumentNullException("product");
            }

            this.ServiceCode = product;
            this.Format = Format.JSON;
            this.RelativePath = String.Empty;
        }

        #endregion
    }
}
