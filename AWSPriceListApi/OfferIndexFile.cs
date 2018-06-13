using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BAMCIS.AWSPriceListApi
{
    public class OfferIndexFile
    {
        #region Private Fields

        private static HttpClientHandler _Handler = new HttpClientHandler();

        private static HttpClient _Client = new HttpClient(_Handler);

        #endregion

        #region Public Fields

        public static readonly Uri PriceListBaseUrl = new Uri("https://pricing.us-east-1.amazonaws.com");

        public static readonly Uri OfferIndexFileUrl = new Uri("https://pricing.us-east-1.amazonaws.com/offers/v1.0/aws/index.json");

        #endregion

        #region Public Properties

        public string FormatVersion { get; }

        public string Disclaimer { get; }

        public DateTime PublicationDate { get; }

        public IDictionary<string, Offer> Offers { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds a new offer index file
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="disclaimer"></param>
        /// <param name="publicationDate"></param>
        /// <param name="offers"></param>
        [JsonConstructor]
        public OfferIndexFile(string formatVersion, string disclaimer, DateTime publicationDate, IDictionary<string, Offer> offers)
        {
            if (String.IsNullOrEmpty(formatVersion))
            {
                throw new ArgumentNullException("formatVersion");
            }

            if (String.IsNullOrEmpty(disclaimer))
            {
                throw new ArgumentNullException("disclaimer");
            }

            this.FormatVersion = formatVersion;
            this.Disclaimer = disclaimer;
            this.PublicationDate = publicationDate;
            this.Offers = offers;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the offer index file from the default Url
        /// </summary>
        /// <returns></returns>
        public async static Task<OfferIndexFile> GetAsync()
        {
            return await GetAsync(OfferIndexFileUrl);
        }

        /// <summary>
        /// Gets the offer index file from the specified url
        /// </summary>
        /// <param name="url">The url of the offer index file</param>
        /// <returns></returns>
        public async static Task<OfferIndexFile> GetAsync(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            return await GetAsync(new Uri(url));
        }

        /// <summary>
        /// Gets the offer index file from the specified url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async static Task<OfferIndexFile> GetAsync(Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            string Content = await _Client.GetStringAsync(url);

            return JsonConvert.DeserializeObject<OfferIndexFile>(Content);
        }

        /// <summary>
        /// Gets the default offer index file url
        /// </summary>
        /// <returns></returns>
        public static Uri GetOfferIndexFileUrl()
        {
            return OfferIndexFileUrl;
        }
        #endregion
    }
}
