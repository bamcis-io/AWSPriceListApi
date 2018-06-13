using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// Represents an individual service inside the offer index file
    /// </summary>
    public sealed class Offer
    {
        #region Private Fields

        private static HttpClientHandler _Handler = new HttpClientHandler();

        private static HttpClient _Client = new HttpClient(_Handler);

        private static readonly Uri _PriceListBaseUrl = new Uri("https://pricing.us-east-1.amazonaws.com");

        #endregion

        #region Public Properties

        public string OfferCode { get; }

        public string VersionIndexUrl { get; }

        public string CurrentVersionUrl { get; }

        public string CurrentRegionIndexUrl { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor to create an individual offer
        /// </summary>
        /// <param name="offerCode">The offer code</param>
        /// <param name="versionIndexUrl">The version index url</param>
        /// <param name="currentVersionUrl">The current verion url</param>
        /// <param name="currentRegionIndexUrl">The current region index url</param>
        [JsonConstructor]
        public Offer(string offerCode, string versionIndexUrl, string currentVersionUrl, string currentRegionIndexUrl)
        {
            if (String.IsNullOrEmpty(offerCode))
            {
                throw new ArgumentNullException("offerCode");
            }

            if (String.IsNullOrEmpty(versionIndexUrl))
            {
                throw new ArgumentNullException("versionIndexUrl");
            }

            if (String.IsNullOrEmpty(currentVersionUrl))
            {
                throw new ArgumentNullException("currentVersionUrl");
            }

            this.OfferCode = offerCode;
            this.VersionIndexUrl = versionIndexUrl;
            this.CurrentVersionUrl = currentVersionUrl;
            this.CurrentRegionIndexUrl = currentRegionIndexUrl ?? String.Empty; // This can be null
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the region index file
        /// </summary>
        /// <returns>The region index file, or null if this offer does not
        /// have a region index url defined
        /// </returns>
        public async Task<RegionIndex> GetRegionIndexAsync()
        {
            if (!String.IsNullOrEmpty(this.CurrentRegionIndexUrl))
            {
                string Path = $"{_PriceListBaseUrl.Scheme}://{_PriceListBaseUrl.DnsSafeHost}{this.CurrentRegionIndexUrl}";

                HttpResponseMessage Response = await _Client.GetAsync(Path);

                if (Response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<RegionIndex>(await Response.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new HttpRequestException($"{Path} : {(int)Response.StatusCode} {Response.StatusCode} {Response.ReasonPhrase} : {await Response.Content.ReadAsStringAsync()}");
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the version index file
        /// </summary>
        /// <returns>
        /// The version index file or null if this offer does not
        /// have a version index url defined
        /// </returns>
        public async Task<VersionIndex> GetVersionIndexAsync()
        {
            if (!String.IsNullOrEmpty(this.VersionIndexUrl))
            {
                string Path = $"{_PriceListBaseUrl.Scheme}://{_PriceListBaseUrl.DnsSafeHost}{this.VersionIndexUrl}";

                HttpResponseMessage Response = await _Client.GetAsync(Path);

                if (Response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<VersionIndex>(await Response.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new HttpRequestException($"{Path} : {(int)Response.StatusCode} {Response.StatusCode} {Response.ReasonPhrase} : {await Response.Content.ReadAsStringAsync()}");
                }
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
