using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// Represents a savings plan product
    /// </summary>
    public class SavingsPlanProduct
    {
        #region Public Properties

        public string Sku { get; }

        public string ProductFamily { get; }

        public string ServiceCode { get; }

        public string UsageType { get; }

        public string Operation { get; }

        //public IReadOnlyDictionary<string, string> Attributes { get; }

        public SavingsPlanProductAttributes Attributes { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SavingsPlanProduct(
            string sku,
            string productFamily,
            string serviceCode,
            string usageType,
            string operation,
            //IDictionary<string, string> attributes)
            SavingsPlanProductAttributes attributes)
        {
            if (String.IsNullOrEmpty(sku))
            {
                throw new ArgumentNullException(nameof(sku));
            }

            if (String.IsNullOrEmpty(productFamily))
            {
                throw new ArgumentNullException(nameof(productFamily));
            }

            if (String.IsNullOrEmpty(serviceCode))
            {
                throw new ArgumentNullException(nameof(serviceCode));
            }

            if (String.IsNullOrEmpty(usageType))
            {
                throw new ArgumentNullException(nameof(usageType));
            }

            this.Sku = sku;
            this.ProductFamily = productFamily;
            this.ServiceCode = serviceCode;
            this.UsageType = usageType;
            this.Operation = operation ?? throw new ArgumentNullException(nameof(operation)); // Allowed to be empty
            //this.Attributes = attributes == null ? new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()) : new ReadOnlyDictionary<string, string>(attributes.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase));
            this.Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        #endregion
    }
}
