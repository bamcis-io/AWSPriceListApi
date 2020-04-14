using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.AWSPriceListApi.Model.SavingsPlan
{
    /// <summary>
    /// Represents the terms in the savings plan offer
    /// </summary>
    public class SavingsPlanOfferTerms
    {
        #region Public Properties

        public IEnumerable<SavingsPlanTerm> SavingsPlan { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SavingsPlanOfferTerms(IEnumerable<SavingsPlanTerm> savingsPlan)
        {
            this.SavingsPlan = savingsPlan ?? throw new ArgumentNullException(nameof(savingsPlan));
        }

        #endregion
    }
}
