using System;

namespace BAMCIS.AWSPriceListApi
{
    public sealed class PriceListClientConfig
    {
        #region Private Fields

        private static readonly Uri _PriceListBaseUrlDefault = new Uri("https://pricing.us-east-1.amazonaws.com");

        #endregion

        #region Public Properties

        public Extension Extension { get; set; }

        public Uri PriceListBaseUrl { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a default client config
        /// </summary>
        public PriceListClientConfig()
        {
            this.Extension = Extension.JSON;
            this.PriceListBaseUrl = _PriceListBaseUrlDefault;
        }

        #endregion
    }
}
