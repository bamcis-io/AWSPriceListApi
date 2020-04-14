using BAMCIS.AWSPriceListApi.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// The response from an offer index file request
    /// </summary>
    public sealed class GetOfferIndexFileResponse : AWSPriceListApiResponse<OfferIndexFile>
    {
        #region Public Properties

        /// <summary>
        /// The offer index file data
        /// </summary>
        public OfferIndexFile OfferIndexFile { 
            get
            {
                return this.Data;
            }
        }

        /// <summary>
        /// Indicates if the response was provided from the local cache
        /// </summary>
        public bool FromCache { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new offer index file response
        /// </summary>
        /// <param name="file"></param>
        internal GetOfferIndexFileResponse(HttpResponseMessage response) : base(response)
        {
            this.FromCache = false;
        }

        #endregion
    }
}
