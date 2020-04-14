using BAMCIS.AWSPriceListApi.Model;
using System.Net.Http;

namespace BAMCIS.AWSPriceListApi
{
    public class GetVersionIndexResponse : AWSPriceListApiResponse<VersionIndex>
    {
        #region Public Properties

        /// <summary>
        /// The vesion index
        /// </summary>
        public VersionIndex VersionIndex { 
            get
            {
                return this.Data;
            }
        }

        #endregion

        #region Constructors

        internal GetVersionIndexResponse(HttpResponseMessage response) : base(response)
        {
        }

        #endregion
    }
}
