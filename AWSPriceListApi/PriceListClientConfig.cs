using System;
using System.Text;

namespace BAMCIS.AWSPriceListApi
{
    public sealed class PriceListClientConfig
    {
        #region Private Fields

        internal static readonly Uri _PriceListBaseUrlDefault = new Uri("https://pricing.us-east-1.amazonaws.com");

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

        #region Public Methods

        internal string GetBaseUrlString()
        {
            return GetBaseUrlString(this.PriceListBaseUrl);
        }

        internal static string GetBaseUrlString(Uri baseUrl)
        {
            StringBuilder Base = new StringBuilder($"{baseUrl.Scheme}://{baseUrl.DnsSafeHost}");

            if (baseUrl.Scheme == "http" && baseUrl.Port != 80 ||
                baseUrl.Scheme == "https" && baseUrl.Port != 443)
            {
                Base.Append($":{baseUrl.Port}");
            }

            return Base.ToString();
        }

        #endregion
    }
}
