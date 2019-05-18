using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// A client to interact with the AWS Price List API
    /// </summary>
    public sealed class PriceListClient
    {
        #region Private Fields

        private static HttpClientHandler handler;

        private static HttpClient httpClient;

        private OfferIndexFile indexFile = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// The client configuration
        /// </summary>
        public PriceListClientConfig Config { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Static constructor
        /// </summary>
        static PriceListClient()
        {
            handler = new HttpClientHandler();
            httpClient = new HttpClient(handler);
        }

        /// <summary>
        /// Creates a new price list client with the default configuration
        /// </summary>
        public PriceListClient()
        {
            this.Config = new PriceListClientConfig();
        }

        /// <summary>
        /// Creates a new price list client with the provided configuration
        /// </summary>
        /// <param name="config"></param>
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
        public async Task<GetOfferIndexFileResponse> GetOfferIndexFileAsync()
        {
            return await GetOfferIndexFileAsync(new GetOfferIndexFileRequest());
        }

        /// <summary>
        /// Gets the offer index file
        /// </summary>
        /// <param name="relativePath">
        /// The relative path of the offer index file, like "/offers/v1.0/aws/index.json"
        /// </param>
        /// <returns></returns>
        public async Task<GetOfferIndexFileResponse> GetOfferIndexFileAsync(GetOfferIndexFileRequest request)
        {
            if (this.indexFile == null || this.Config.NoCache)
            {
                string path = $"{this.Config.GetBaseUrlString()}{request.RelativePath}";

                try
                {
                    this.indexFile = await OfferIndexFile.GetAsync(path);
                }
                catch (PriceListException ex)
                {
                    return new GetOfferIndexFileResponse(ex.Message, ex.StatusCode);
                }
            }

            return new GetOfferIndexFileResponse(this.indexFile);
        }

        /// <summary>
        /// Lists the services that have price data available
        /// </summary>
        /// <returns></returns>
        public async Task<ListServicesResponse> ListServicesAsync()
        {
            return await ListServicesAsync(new ListServicesRequest());
        }

        /// <summary>
        /// Lists the services that have price data available
        /// </summary>
        /// <returns></returns>
        public async Task<ListServicesResponse> ListServicesAsync(ListServicesRequest request)
        {
            if (this.indexFile == null || this.Config.NoCache)
            {
                GetOfferIndexFileResponse response = await this.GetOfferIndexFileAsync();

                if (!response.IsError)
                {
                    this.indexFile = response.OfferIndexFile;
                }
                else
                {
                    throw new PriceListException(response.Reason, response.StatusCode);
                }
            }

            return new ListServicesResponse(this.indexFile.Offers.Keys);
        }

        /// <summary>
        /// Gets price data for a specified product
        /// </summary>
        /// <param name="product">The product to get price data for</param>
        /// <returns></returns>
        public async Task<GetProductResponse> GetProductAsync(GetProductRequest request)
        {
            if (this.indexFile == null || this.Config.NoCache)
            {
                GetOfferIndexFileResponse response = await GetOfferIndexFileAsync();

                if (!response.IsError)
                {
                    this.indexFile = response.OfferIndexFile;
                }
                else
                {
                    throw new PriceListException(response.Reason, response.StatusCode)
                    {
                        Reason = response.Reason
                    };
                }
            }

            IEnumerable<KeyValuePair<string, Offer>> offers = this.indexFile.Offers.Where(
                x => x.Key.Equals(request.ServiceCode, StringComparison.OrdinalIgnoreCase)
            );

            if (offers.Any())
            {
                string path = $"{this.Config.GetBaseUrlString()}{offers.First().Value.CurrentVersionUrl}";

                switch (request.Format)
                {
                    case Format.CSV:
                        {
                            path = System.IO.Path.ChangeExtension(path, "csv");
                            break;
                        }
                    case Format.JSON:
                        {
                            path = System.IO.Path.ChangeExtension(path, "json");
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException($"Unknown format: {request.Format.ToString()}.");
                        }
                }

                HttpResponseMessage response = await httpClient.GetAsync(path);

                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return new GetProductResponse(response, request);
                    }
                    else
                    {
                        throw new PriceListException(await response.Content.ReadAsStringAsync(), response.StatusCode)
                        {
                            Reason = response.ReasonPhrase,
                            Request = response.RequestMessage
                        };
                    }
                }
                finally
                {
                    response.Content.Dispose();
                    response.Dispose();
                }
            }
            else
            {
                throw new ArgumentException($"No product found matching {request.ServiceCode}.");
            }
        }

        #endregion
    }
}
