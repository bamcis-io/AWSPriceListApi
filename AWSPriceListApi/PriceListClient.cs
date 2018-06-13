using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BAMCIS.AWSPriceListApi
{
    public sealed class PriceListClient
    {
        #region Private Fields

        private static HttpClientHandler _Handler = new HttpClientHandler();

        private static HttpClient _Client = new HttpClient(_Handler);

        private OfferIndexFile _IndexFile = null;

        #endregion

        #region Public Fields

        public static readonly string DefaultOfferIndexFileUrl = "/offers/v1.0/aws/index.json";

        #endregion

        #region Public Properties

        public PriceListClientConfig Config { get; }

        #endregion

        #region Constructors

        public PriceListClient()
        {
            this.Config = new PriceListClientConfig();
        }

        public PriceListClient(PriceListClientConfig config)
        {
            this.Config = config ?? throw new ArgumentNullException("config");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the offer index file
        /// </summary>
        /// <returns></returns>
        public async Task<OfferIndexFile> GetOfferIndexFileAsync()
        {
            return await GetOfferIndexFileAsync(DefaultOfferIndexFileUrl);
        }

        /// <summary>
        /// Gets the offer index file
        /// </summary>
        /// <param name="relativePath">
        /// The relative path of the offer index file, like "/offers/v1.0/aws/index.json"
        /// </param>
        /// <returns></returns>
        public async Task<OfferIndexFile> GetOfferIndexFileAsync(string relativePath)
        {
            string Path = $"{this.Config.PriceListBaseUrl.Scheme}://{this.Config.PriceListBaseUrl.DnsSafeHost}{relativePath}";

            return await OfferIndexFile.GetAsync(Path);
        }

        /// <summary>
        /// Lists the services that have price data available
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListServicesAsync()
        {
            OfferIndexFile Index = await this.GetOfferIndexFileAsync();

            return Index.Offers.Keys;
        }

        /// <summary>
        /// Gets price data for a specified product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<string> GetProductAsync(string product)
        {
            if (this._IndexFile == null)
            {
                this._IndexFile = await GetOfferIndexFileAsync();
            }

            IEnumerable<KeyValuePair<string, Offer>> Offers = this._IndexFile.Offers.Where(
                x => x.Key.Equals(product, StringComparison.OrdinalIgnoreCase)
            );

            if (Offers.Any())
            {
                string Path = $"{this.Config.PriceListBaseUrl.Scheme}://{this.Config.PriceListBaseUrl.DnsSafeHost}{Offers.First().Value.CurrentVersionUrl}";

                switch (this.Config.Extension)
                {
                    case Extension.CSV:
                        {
                            Path = System.IO.Path.ChangeExtension(Path, "csv");
                            break;
                        }
                    case Extension.JSON:
                        {
                            Path = System.IO.Path.ChangeExtension(Path, "json");
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException($"Unknown extension: {this.Config.Extension.ToString()}.");
                        }
                }

                HttpResponseMessage Response = await _Client.GetAsync(Path);

                if (Response.IsSuccessStatusCode)
                {
                    return await Response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException($"{(int)Response.StatusCode} {Response.ReasonPhrase} : {await Response.Content.ReadAsStringAsync()}");
                }
            }
            else
            {
                throw new ArgumentException($"No product found matching {product}.");
            }
        }

        #endregion
    }
}
