namespace BAMCIS.AWSPriceListApi
{
    /// <summary>
    /// Represents a request to retrieve the offer index file
    /// </summary>
    public sealed class GetOfferIndexFileRequest
    {
        #region Public Fields

        /// <summary>
        /// The default relative path to the offer index file
        /// </summary>
        public static readonly string DefaultOfferIndexFileUrl = "/offers/v1.0/aws/index.json";

        #endregion

        #region Public Properties

        /// <summary>
        /// The relative path of the offer index file url
        /// </summary>
        public string RelativePath { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new offer index file request using the default index file url
        /// </summary>
        public GetOfferIndexFileRequest()
        {
            this.RelativePath = DefaultOfferIndexFileUrl;
        }

        /// <summary>
        /// Creates a new offer index file request
        /// </summary>
        /// <param name="relativePath">The relative url path for the index file</param>
        public GetOfferIndexFileRequest(string relativePath)
        {
            this.RelativePath = relativePath;
        }

        #endregion
    }
}
