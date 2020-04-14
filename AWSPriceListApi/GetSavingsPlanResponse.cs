using BAMCIS.AWSPriceListApi.Model.SavingsPlan;
using System.Net.Http;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// Represents the contents of a savings plan offer, i.e. all of the content about products and discounted terms
    /// </summary>
    public class GetSavingsPlanResponse : AWSPriceListApiResponse<SavingsPlanOffer>
    {
        #region Public Properties

        /// <summary>
        /// The savings plans data
        /// </summary>
        public SavingsPlanOffer SavingsPlan
        {
            get
            {
                return this.Data;
            }
        }

        #endregion

        #region Constructors

        public GetSavingsPlanResponse(HttpResponseMessage response) : base(response)
        {
        }

        #endregion
    }
}
