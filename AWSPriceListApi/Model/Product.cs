using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BAMCIS.AWSPriceListApi.Model
{
    /// <summary>
    /// A product in the AWS offer file.
    /// 
    /// This is what a single product looks like in the AWS offer file
    ///{
    ///  ...
    ///  "products": {
    ///    ...
    ///    "TCFKJNWNXBMMZMQ3" : {
    ///      "sku" : "TCFKJNWNXBMMZMQ3",
    ///      "productFamily" : "Compute Instance",
    ///      "attributes" : {
    ///        "servicecode" : "AmazonEC2",
    ///        "location" : "Asia Pacific (Mumbai)",
    ///        "locationType" : "AWS Region",
    ///        "instanceType" : "i3.2xlarge",
    ///        "currentGeneration" : "Yes",
    ///        "instanceFamily" : "Storage optimized",
    ///        "vcpu" : "8",
    ///        "physicalProcessor" : "Intel Xeon E5-2686 v4 (Broadwell)",
    ///        "clockSpeed" : "2.3 GHz",
    ///        "memory" : "61 GiB",
    ///        "storage" : "1 x 1.9 NVMe SSD",
    ///        "networkPerformance" : "Up to 10 Gigabit",
    ///        "processorArchitecture" : "64-bit",
    ///        "tenancy" : "Dedicated",
    ///        "operatingSystem" : "Windows",
    ///        "licenseModel" : "No License required",
    ///        "usagetype" : "APS3-DedicatedUsage:i3.2xlarge",
    ///        "operation" : "RunInstances:0102",
    ///        "dedicatedEbsThroughput" : "1750 Mbps",
    ///        "ecu" : "53",
    ///        "enhancedNetworkingSupported" : "Yes",
    ///        "normalizationSizeFactor" : "16",
    ///        "preInstalledSw" : "SQL Ent",
    ///        "processorFeatures" : "Intel AVX, Intel AVX2, Intel Turbo",
    ///        "servicename" : "Amazon Elastic Compute Cloud"
    ///      }
    ///    },
    ///  ...
    ///  "terms" : {
    ///    "OnDemand" : {
    ///    }   
    ///    "Reserved" : {
    ///    }
    ///  }
    ///}
    /// </summary>
    public sealed class Product
    { 
        #region Public Properties

        /// <summary>
        /// The product unique SKU
        /// </summary>
        public string Sku { get; }

        /// <summary>
        /// The product family, things like Compute Instance, Provisioned IOPS, Data Transfer
        /// </summary>
        public string ProductFamily { get; }

        /// <summary>
        /// A dictionary of attributes of the product
        /// </summary>
        public IReadOnlyDictionary<string, string> Attributes { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="productFamily"></param>
        /// <param name="attributes"></param>
        [JsonConstructor]
        public Product(
            string sku,
            string productFamily,
            IDictionary<string, string> attributes
            )
        {
            if (String.IsNullOrEmpty(sku))
            {
                throw new ArgumentNullException(nameof(sku));
            }

            /* In some versions, it appears this field is sometimes omitted for some products
            if (String.IsNullOrEmpty(productFamily))
            {
                throw new ArgumentNullException(nameof(productFamily));
            }
            */

            this.Attributes = attributes == null ? new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()) : new ReadOnlyDictionary<string, string>(attributes.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase));
            this.ProductFamily = productFamily;
            this.Sku = sku;
        }

        #endregion
    }
}
