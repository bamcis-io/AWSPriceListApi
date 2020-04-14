using BAMCIS.AWSPriceListApi.Model.SavingsPlan;
using System.Net.Http;

namespace BAMCIS.AWSPriceListApi
{
    public class GetSavingsPlanIndexFileResponse : AWSPriceListApiResponse<SavingsPlanRegionIndex>
    {
        #region Public Properties

        /// <summary>
        /// The savings plan region index
        /// </summary>
        public SavingsPlanRegionIndex RegionIndex { 
            get
            {
                return this.Data;
            }
        }

        #endregion

        #region Constructors

        internal GetSavingsPlanIndexFileResponse(HttpResponseMessage response) : base(response)
        {
        }

        #endregion
    }
}
