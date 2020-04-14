using BAMCIS.AWSPriceListApi.Model;
using BAMCIS.ExponentialBackoffAndRetry;
using BAMCIS.ExponentialBackoffAndRetry.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        private readonly HttpClientHandler handler;

        private readonly ExponentialBackoffAndRetryHandler backoffHandler;

        private readonly HttpClient httpClient;

        private GetOfferIndexFileResponse cachedOfferIndexFile = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// The client configuration
        /// </summary>
        public PriceListClientConfig Config { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new price list client with the default configuration
        /// </summary>
        public PriceListClient() : this(new PriceListClientConfig())
        { }

        /// <summary>
        /// Creates a new price list client with the provided configuration
        /// </summary>
        /// <param name="config"></param>
        public PriceListClient(PriceListClientConfig config)
        {
            this.Config = config ?? throw new ArgumentNullException("config");


            handler = new HttpClientHandler();
            backoffHandler = new ExponentialBackoffAndRetryHandler((IExponentialBackoffAndRetry)new ExponentialBackoffAndRetryClient(new ExponentialBackoffAndRetryConfig()
            {
                MaximumRetries = this.Config.MaxErrorRetry,
                DelayInMilliseconds = 100,
                MaxBackoffInMilliseconds = this.Config.Timeout.HasValue ? (int)this.Config.Timeout.Value.TotalMilliseconds : 10000,
                ExceptionHandlingLogic = (ex => ex is HttpResponseException && ((HttpResponseException)ex).StatusCode >= HttpStatusCode.InternalServerError || (ex is TimeoutException || ex is OperationCanceledException) || ex is HttpRequestException)
            }), this.handler);
            this.httpClient = new HttpClient(this.backoffHandler);
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
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (this.cachedOfferIndexFile != null && !this.Config.NoCache)
            {
                return this.cachedOfferIndexFile;
            }

            return await ExecuteRequestAsync(request.RelativePath, nameof(request.RelativePath), (x) => new GetOfferIndexFileResponse(x));
        }

        /// <summary>
        /// Gets the region index based on the relative path provided in the request
        /// </summary>
        /// <param name="request">The request for a regional index. The request contains a relative path that will specify the service, 
        /// for example, "/offers/v1.0/aws/AmazonEC2/current/region_index.json".</param>
        /// <returns></returns>
        public async Task<GetRegionIndexResponse> GetRegionIndexAsync(GetRegionIndexRequest request)
        {
            return await ExecuteRequestAsync(request.CurrentRegionIndexUrl, nameof(request.CurrentRegionIndexUrl), (x) => new GetRegionIndexResponse(x));
        }

        /// <summary>
        /// Gets the version index based on the relative path provided in the request. You can use these versions to check historical prices instead of the current.
        /// </summary>
        /// <param name="request">The request for a version index. The request contains a relative path that will specify the service,
        /// for example, "/offers/v1.0/aws/AmazonEC2/index.json" </param>
        /// <returns>A list of versions of the offer files that are available for the service.</returns>
        public async Task<GetVersionIndexResponse> GetVersionIndexAsync(GetVersionIndexRequest request)
        {
            return await ExecuteRequestAsync(request.VersionIndexUrl, nameof(request.VersionIndexUrl), (x) => new GetVersionIndexResponse(x));
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
            if (this.cachedOfferIndexFile == null || this.Config.NoCache)
            {
                GetOfferIndexFileResponse response = await this.GetOfferIndexFileAsync();

                if (response.IsError())
                {
                    throw new PriceListException(response.ResponseMetadata.Metadata["ErrorReason"], response.HttpStatusCode);
                }

                this.cachedOfferIndexFile = response;
            }

            return new ListServicesResponse(this.cachedOfferIndexFile);
        }

        /// <summary>
        /// Gets price data for a specified product
        /// </summary>
        /// <param name="product">The product to get price data for</param>
        /// <returns></returns>
        public async Task<GetProductResponse> GetProductAsync(GetProductRequest request)
        {
            if (this.cachedOfferIndexFile == null || this.Config.NoCache)
            {
                GetOfferIndexFileResponse offerIndexFile = await GetOfferIndexFileAsync();

                if (offerIndexFile.IsError())
                {
                    throw new PriceListException(offerIndexFile.ResponseMetadata.Metadata.ContainsKey("ErrorReason") ? offerIndexFile.ResponseMetadata.Metadata["ErrorReason"] : "", offerIndexFile.HttpStatusCode);
                }

                this.cachedOfferIndexFile = offerIndexFile;
            }

            IEnumerable<KeyValuePair<string, Offer>> offers = this.cachedOfferIndexFile.OfferIndexFile.Offers.Where(
                x => x.Key.Equals(request.ServiceCode, StringComparison.OrdinalIgnoreCase)
            );

            if (!offers.Any())
            {
                throw new ArgumentException($"No product found matching {request.ServiceCode}.");
            }

            string path = offers.First().Value.CurrentVersionUrl;

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

            return await ExecuteRequestAsync(path, nameof(path), (x) => new GetProductResponse(x, request.Format, request.ServiceCode));
        }

        #region Savings Plans

        /// <summary>
        /// Gets the version index for savings plans
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetSavingsPlanVersionIndexResponse> GetSavingsPlanVersionIndexAsync(GetSavingsPlanVersionIndexRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await ExecuteRequestAsync(request.SavingsPlanVersionIndexUrl, nameof(request.SavingsPlanVersionIndexUrl), (x) => new GetSavingsPlanVersionIndexResponse(x));
        }

        /// <summary>
        /// Gets the region index for savings plans
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetSavingsPlanIndexFileResponse> GetSavingsPlanRegionIndexAsync(GetSavingsPlanIndexFileRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await ExecuteRequestAsync(request.CurrentSavingsPlanIndexUrl, nameof(request.CurrentSavingsPlanIndexUrl), (x) => new GetSavingsPlanIndexFileResponse(x));
        }

        /// <summary>
        /// Gets the savings plan data for a specific service and region (determined by the relative path in the request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetSavingsPlanResponse> GetSavingsPlanAsync(GetSavingsPlanRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await ExecuteRequestAsync(request.VersionUrl, nameof(request.VersionUrl), (x) => new GetSavingsPlanResponse(x));
        }

        #endregion

        #endregion

        private async Task<T> ExecuteRequestAsync<T>(string relativePath, string parameterName, Func<HttpResponseMessage, T> func)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException(parameterName);
            }

            HttpResponseMessage response = await this.httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, PriceListClientConfig.GetBaseUrlString(PriceListClientConfig.PRICE_LIST_DEFAULT_URL) + relativePath), HttpCompletionOption.ResponseHeadersRead);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return func(response);
                }

                throw new PriceListException(response, await response.Content.ReadAsStringAsync());
            }
            finally
            {
                response.Dispose();
            }
        }
    }   
}
