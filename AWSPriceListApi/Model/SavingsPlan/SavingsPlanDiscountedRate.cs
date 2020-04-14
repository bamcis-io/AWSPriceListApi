using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// The discounted rate for a savings plan
    /// </summary>
    public class SavingsPlanDiscountedRate
    {
        #region Public Properties

        /// <summary>
        /// The price
        /// </summary>
        public double Price { get; }

        /// <summary>
        /// The payment currency like USD
        /// </summary>
        public string Currency { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a savings plan discounted rate
        /// </summary>
        /// <param name="price"></param>
        /// <param name="currency"></param>
        [JsonConstructor]
        public SavingsPlanDiscountedRate(double price, string currency)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException("price", "Price cannot be less than zero.");
            }

            if (String.IsNullOrEmpty(currency))
            {
                throw new ArgumentNullException("currency");
            }

            this.Price = price;
            this.Currency = currency;
        }

        #endregion
    }
}
