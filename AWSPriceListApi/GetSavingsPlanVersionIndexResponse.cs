using BAMCIS.AWSPriceListApi.Model.SavingsPlan;
using System.Net.Http;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// The response for a savings plan version index request
    /// </summary>
    public class GetSavingsPlanVersionIndexResponse : AWSPriceListApiResponse<SavingsPlanVersionIndex>
    {
        #region Public Properties

        /// <summary>
        /// The savings plan version index. It contains data on the publication date
        /// and relative url for all savings plans versions.
        /// </summary>
        public SavingsPlanVersionIndex VersionIndex { 
            get
            {
                return this.Data;
            }
        }

        #endregion

        #region Constructors

        internal GetSavingsPlanVersionIndexResponse(HttpResponseMessage response) : base(response)
        {          
        }

        #endregion
    }
}
