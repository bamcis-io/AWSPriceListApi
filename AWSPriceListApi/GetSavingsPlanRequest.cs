using Amazon.Runtime;
using System;

namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// A request to get the savings plan offer data
    /// </summary>
    public class GetSavingsPlanRequest : AmazonWebServiceRequest
    {
        #region Public Properties

        /// <summary>
        /// The relative path to the index file, for example "/savingsPlan/v1.0/aws/AWSComputeSavingsPlan/20200320225600/eu-west-3/index.json"
        /// </summary>
        public string VersionUrl { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new savings plan request
        /// </summary>
        /// <param name="versionUrl">The relative path to the savings plan index file,
        /// for example, "/savingsPlan/v1.0/aws/AWSComputeSavingsPlan/20200320225600/eu-west-3/index.json"</param>
        public GetSavingsPlanRequest(string versionUrl)
        {
            if (String.IsNullOrEmpty(versionUrl))
            {
                throw new ArgumentNullException("versionUrl");
            }

            this.VersionUrl = versionUrl;
        }

        #endregion
    }
}
