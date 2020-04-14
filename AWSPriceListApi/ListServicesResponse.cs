using BAMCIS.AWSPriceListApi.Model;
using System;
using System.Collections.Generic;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// A response for a list services request
    /// </summary>
    public sealed class ListServicesResponse : AWSPriceListApiResponse<OfferIndexFile>
    {
        #region Public Properties

        /// <summary>
        /// The services that have available pricing data
        /// </summary>
        public IEnumerable<string> Services { 
            get
            {
                return this.Data.Offers.Keys;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new list services response
        /// </summary>
        /// <param name="services"></param>
        internal ListServicesResponse(GetOfferIndexFileResponse offer) : base(offer)
        {
            if (offer == null)
            {
                throw new ArgumentNullException("offer");
            }
        }

        #endregion
    }
}
