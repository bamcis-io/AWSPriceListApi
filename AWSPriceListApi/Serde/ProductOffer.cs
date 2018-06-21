using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BAMCIS.AWSPriceListApi.Serde
{
    /// <summary>
    /// The offer data for an individal AWS service
    /// </summary>
    public sealed class ProductOffer
    {
        #region Public Properties

        /// <summary>
        /// The format version of the file, something like v1.0
        /// </summary>
        public string FormatVersion { get; }

        /// <summary>
        /// The disclaimer provided with the offer file
        /// </summary>
        public string Disclaimer { get; }

        /// <summary>
        /// The offer code of the file, represents the service the offer terms covers, like AmazonEC2
        /// </summary>
        public string OfferCode { get; }

        /// <summary>
        /// The timestamp representing the version of the offer file
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// An ISO compliant date time of when the file was published
        /// </summary>
        public DateTime PublicationDate { get; }

        /// <summary>
        /// The total set of products and their attributes in the service family, they are organized by their SKU as 
        /// the dictionary key
        /// </summary>
        public IReadOnlyDictionary<string, Product> Products { get; }

        //First dictionary is categories like on demand and reserved, the second dictionary is the product sku, and the third dictionary has an object in the form of sku.offerTermCode
        /*

         "Reserved" : {      
            "DQ578CGN99KG6ECF" : {
                "DQ578CGN99KG6ECF.HU7G6KETJZ" : {
                    "offerTermCode" : "HU7G6KETJZ",
                    "sku" : "DQ578CGN99KG6ECF",
                    "effectiveDate" : "2015-04-30T23:59:59Z",
                    "priceDimensions" : {
                        "DQ578CGN99KG6ECF.HU7G6KETJZ.2TG2D8R56U" : {
                            "rateCode" : "DQ578CGN99KG6ECF.HU7G6KETJZ.2TG2D8R56U",
                            "description" : "Upfront Fee",
                            "unit" : "Quantity",
                            "pricePerUnit" : {
                                "USD" : "11213"
                            },
                            "appliesTo" : [ ]
                        },
                        "DQ578CGN99KG6ECF.HU7G6KETJZ.6YS6EN2CT7" : {
                        ...   
                        }
                    },
                    "termAttributes" : {
                        "LeaseContractLength" : "1yr",
                        "PurchaseOption" : "Partial Upfront"
                    }
                },
                "DQ578CGN99KG6ECF.38NPMPTW36" : {
                ...
                }
            }
        }
        */

        /// <summary>
        /// The available OnDemand and Reserved terms for all of the products. These are organized by Term type (Reserved or OnDemand),
        /// then by SKU, then by SKU.OfferTermCode, which represents an actual term that is available for purchase (like 1 yr all upfront or
        /// on demand)
        /// </summary>
        [JsonConverter(typeof(ProductOfferTermsConverter))]
        // This needs to be a dictionary so the JSON converter can create a concrete
        // implementation of it during serialization
        public Dictionary<Term, IDictionary<string, IDictionary<string, PricingTerm>>> Terms { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds a new product offer
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="disclaimer"></param>
        /// <param name="offerCode"></param>
        /// <param name="version"></param>
        /// <param name="publicationDate"></param>
        /// <param name="products"></param>
        /// <param name="terms"></param>
        [JsonConstructor]
        public ProductOffer(
            string formatVersion,
            string disclaimer,
            string offerCode,
            string version,
            DateTime publicationDate,
            IDictionary<string, Product> products,
            Dictionary<Term, IDictionary<string, IDictionary<string, PricingTerm>>> terms 
        )
        {
            this.FormatVersion = formatVersion;
            this.Disclaimer = disclaimer;
            this.OfferCode = offerCode;
            this.Version = version;
            this.PublicationDate = publicationDate;
            this.Products = products == null ? 
                new ReadOnlyDictionary<string, Product>(new Dictionary<string, Product>()) : 
                new ReadOnlyDictionary<string, Product>(products);
            this.Terms = terms == null ?
                new Dictionary<Term, IDictionary<string, IDictionary<string, PricingTerm>>>(new Dictionary<Term, IDictionary<string, IDictionary<string, PricingTerm>>>()) :
                terms;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the JSON into a ProductOffer
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ProductOffer FromJson(string json)
        {
            if (String.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return JsonConvert.DeserializeObject<ProductOffer>(json);
        }

        /// <summary>
        /// Serializes the product offer into JSON
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        public static string ToJson(ProductOffer offer)
        {
            if (offer == null)
            {
                throw new ArgumentNullException("offer");
            }

            return JsonConvert.SerializeObject(offer);
        }

        #endregion
    }
}
