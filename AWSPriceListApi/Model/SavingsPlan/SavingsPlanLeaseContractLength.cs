using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// Represents the savings plan term contract length
    /// </summary>
    public class SavingsPlanLeaseContractLength
    {
        #region Public Properties

        /// <summary>
        /// The duration of the lease
        /// </summary>
        public int Duration { get; }

        /// <summary>
        /// The unit of the lease term, like "year"
        /// </summary>
        public string Unit { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SavingsPlanLeaseContractLength(int duration, string unit)
        {
            if (duration <= 0)
            {
                throw new ArgumentOutOfRangeException("duration", "Duration cannot be less than or equal to zero.");
            }

            if (String.IsNullOrEmpty(unit))
            {
                throw new ArgumentNullException("unit");
            }

            this.Duration = duration;
            this.Unit = unit;
        }

        #endregion
    }
}
