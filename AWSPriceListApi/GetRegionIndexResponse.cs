using BAMCIS.AWSPriceListApi.Model;
using System.Net.Http;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// The response for a region index request
    /// </summary>
    public class GetRegionIndexResponse : AWSPriceListApiResponse<RegionIndex>
    {
        #region Public Properties

        /// <summary>
        /// The region index
        /// </summary>
        public RegionIndex RegionIndex { 
            get
            {
                return this.Data;
            }
        }

        #endregion

        #region Constructors

        internal GetRegionIndexResponse(HttpResponseMessage response) : base(response)
        {
        }

        #endregion
    }
}
