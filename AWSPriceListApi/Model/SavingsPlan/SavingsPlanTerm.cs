using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// Represents a term for an individual savings plan
    /// </summary>
    public class SavingsPlanTerm
    {
        #region Public Properties

        public string Sku { get; }

        public string Description { get; }

        public DateTime EffectiveDate { get; }

        public SavingsPlanLeaseContractLength LeaseContractLength { get; }

        public IEnumerable<SavingsPlanRate> Rates { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SavingsPlanTerm(
            string sku,
            string description,
            DateTime effectiveDate,
            SavingsPlanLeaseContractLength leaseContractLength,
            IEnumerable<SavingsPlanRate> rates)
        {
            this.Sku = sku;
            this.Description = description;
            this.EffectiveDate = effectiveDate;
            this.LeaseContractLength = leaseContractLength;
            this.Rates = rates;        
        }

        #endregion
    }
}
