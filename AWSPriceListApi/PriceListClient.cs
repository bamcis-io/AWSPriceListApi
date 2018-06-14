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

        private static HttpClientHandler _Handler = new HttpClientHandler();

        private static HttpClient _Client = new HttpClient(_Handler);

        private OfferIndexFile _IndexFile = null;

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
            if (this._IndexFile == null || this.Config.NoCache)
            {
                string Path = $"{this.Config.GetBaseUrlString()}{request.RelativePath}";

                try
                {
                    this._IndexFile = await OfferIndexFile.GetAsync(Path);
                }
                catch (PriceListException ex)
                {
                    return new GetOfferIndexFileResponse(ex.Message, ex.StatusCode);
                }
            }

            return new GetOfferIndexFileResponse(this._IndexFile);
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
            if (this._IndexFile == null || this.Config.NoCache)
            {
                GetOfferIndexFileResponse Response = await this.GetOfferIndexFileAsync();

                if (!Response.IsError)
                {
                    this._IndexFile = Response.OfferIndexFile;
                }
                else
                {
                    throw new PriceListException(Response.Reason, Response.StatusCode);
                }
            }

            return new ListServicesResponse(this._IndexFile.Offers.Keys);
        }

        /// <summary>
        /// Gets price data for a specified product
        /// </summary>
        /// <param name="product">The product to get price data for</param>
        /// <returns></returns>
        public async Task<GetProductResponse> GetProductAsync(GetProductRequest request)
        {
            if (this._IndexFile == null || this.Config.NoCache)
            {
                GetOfferIndexFileResponse Response = await GetOfferIndexFileAsync();

                if (!Response.IsError)
                {
                    this._IndexFile = Response.OfferIndexFile;
                }
                else
                {
                    throw new PriceListException(Response.Reason, Response.StatusCode)
                    {
                        Reason = Response.Reason
                    };
                }
            }

            IEnumerable<KeyValuePair<string, Offer>> Offers = this._IndexFile.Offers.Where(
                x => x.Key.Equals(request.ServiceCode, StringComparison.OrdinalIgnoreCase)
            );

            if (Offers.Any())
            {
                string Path = $"{this.Config.GetBaseUrlString()}{Offers.First().Value.CurrentVersionUrl}";

                switch (request.Format)
                {
                    case Format.CSV:
                        {
                            Path = System.IO.Path.ChangeExtension(Path, "csv");
                            break;
                        }
                    case Format.JSON:
                        {
                            Path = System.IO.Path.ChangeExtension(Path, "json");
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException($"Unknown format: {request.Format.ToString()}.");
                        }
                }

                HttpResponseMessage Response = await _Client.GetAsync(Path);

                if (Response.IsSuccessStatusCode)
                {
                    return new GetProductResponse(Response, request);                        
                }
                else
                {
                    throw new PriceListException(await Response.Content.ReadAsStringAsync(), Response.StatusCode)
                    {
                        Reason = Response.ReasonPhrase,
                        Request = Response.RequestMessage
                    };
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
