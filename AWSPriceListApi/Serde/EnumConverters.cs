using BAMCIS.AWSPriceListApi.Model;
using System;
using System.Text.RegularExpressions;

namespace BAMCIS.AWSPriceListApi.Serde
{
    /// <summary>
    /// Converts string values from the price list API to specific Enums or Int values
    /// </summary>
    public static class EnumConverters
    {
        #region Public Methods

        /// <summary>
        /// Converts a string value from the price list api to the appropriate enum value
        /// </summary>
        /// <param name="option"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PurchaseOption ConvertToPurchaseOption(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return PurchaseOption.ON_DEMAND;
            }

            switch (value.ToLower().Replace(" ", ""))
            {
                default:
                case "on demand":
                case "ondemand":
                    {
                        return PurchaseOption.ON_DEMAND;
                    }
                case "allupfront":
                case "all upfront":
                    {
                        return PurchaseOption.ALL_UPFRONT;
                    }
                case "partialupfront":
                case "partial upfront":
                    {
                        return PurchaseOption.PARTIAL_UPFRONT;
                    }
                case "noupfront":
                case "no upfront":
                    {
                        return PurchaseOption.NO_UPFRONT;
                    }
                case "heavyutilization":
                case "heavy utilization":
                    {
                        return PurchaseOption.HEAVY_UTILIZATION;
                    }
                case "mediumutilization":
                case "medium utilization":
                    {
                        return PurchaseOption.MEDIUM_UTILIZATION;
                    }
                case "lightutilization":
                case "light utilization":
                    {
                        return PurchaseOption.LIGHT_UTILIZATION;
                    }
                case "unknown":
                    {
                        return PurchaseOption.UNKNOWN;
                    }
            }
        }

        /// <summary>
        /// Converts a string value from the price list api to the appropriate enum value
        /// </summary>
        /// <param name="offering"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static OfferingClass ConvertToOfferingClass(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return OfferingClass.STANDARD;
            }

            switch (value.ToLower())
            {
                // Ensure anything that doesn't have an offering class is labeled as standard
                default:
                case "standard":
                    {
                        return OfferingClass.STANDARD;
                    }
                case "convertible":
                    {
                        return OfferingClass.CONVERTIBLE;
                    }
                case "unknown":
                    {
                        return OfferingClass.UNKNOWN;
                    }
            }
        }

        /// <summary>
        /// Converts a string value from the price list api to the appropriate enum value
        /// </summary>
        /// <param name="term"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Term ConvertToTerm(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return Term.UNKNOWN;
            }

            switch (value.ToLower())
            {
                case "ondemand":
                    {
                        return Term.ON_DEMAND;
                    }
                case "reserved":
                    {
                        return Term.RESERVED;
                    }
                default:
                case "unknown":
                    {
                        return Term.UNKNOWN;
                    }
            }
        }

        /// <summary>
        /// Converts a string lease contract length value to an int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ConvertToLeaseContractLength(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }

            Match match = Regex.Match(value, "^([0-9]+).*$");

            if (match.Success && Int32.TryParse(match.Groups[1].Value, out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}
