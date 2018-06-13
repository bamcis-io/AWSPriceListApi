using System;

namespace BAMCIS.AWSPriceListApi
{
    public sealed class PriceListClientConfig
    {
        #region Private Fields

        private static readonly Uri _PriceListBaseUrlDefault = new Uri("https://pricing.us-east-1.amazonaws.com");

        #endregion

        #region Public Properties

        /// <summary>
        /// The base url to the price list api, do not include
        /// any path information
        /// </summary>
        public Uri PriceListBaseUrl { get; set; }

        /// <summary>
        /// If set to true, the client will not cache results of the offer index
        /// file
        /// </summary>
        public bool NoCache { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a default client config
        /// </summary>
        public PriceListClientConfig()
        {
            this.PriceListBaseUrl = _PriceListBaseUrlDefault;
        }

        #endregion
    }
}
