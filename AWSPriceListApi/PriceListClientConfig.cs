using Amazon.Runtime;
using Amazon.Util.Internal;
using System;
using System.Text;

namespace BAMCIS.AWSPriceListApi
{
    public sealed class PriceListClientConfig : ClientConfig
    {
        #region Private Fields

        private static readonly string UserAgentString = InternalSDKUtils.BuildUserAgentString("2.0.1");
        internal static readonly Uri PRICE_LIST_DEFAULT_URL = new Uri("https://pricing.us-east-1.amazonaws.com");

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

        public override string ServiceVersion
        {
            get
            {
                return "2020-04-13";
            }
        }

        public override string UserAgent
        {
            get
            {
                return PriceListClientConfig.UserAgentString;
            }
        }

        public override string RegionEndpointServiceName
        {
            get
            {
                return "api.pricing";
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a default client config
        /// </summary>
        public PriceListClientConfig()
        {
            this.PriceListBaseUrl = PRICE_LIST_DEFAULT_URL;
        }

        #endregion

        #region Internal Methods

        internal string GetBaseUrlString()
        {
            return GetBaseUrlString(this.PriceListBaseUrl);
        }

        internal static string GetBaseUrlString(Uri baseUrl)
        {
            StringBuilder buffer = new StringBuilder($"{baseUrl.Scheme}://{baseUrl.DnsSafeHost}");

            if (baseUrl.Scheme == "http" && baseUrl.Port != 80 ||
                baseUrl.Scheme == "https" && baseUrl.Port != 443)
            {
                buffer.Append($":{baseUrl.Port}");
            }

            return buffer.ToString();
        }

        #endregion
    }
}
