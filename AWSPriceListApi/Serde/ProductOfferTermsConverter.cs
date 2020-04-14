using BAMCIS.AWSPriceListApi.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BAMCIS.AWSPriceListApi.Serde
{
    /// <summary>
    /// Custom converter for the AWSOffer terms property that uses a Term enum as the top level dictionary key
    /// </summary>
    public class ProductOfferTermsConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => true;

        public override bool CanWrite => true;

        #endregion

        #region Public Methods

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IReadOnlyDictionary<Term, IReadOnlyDictionary<string, IReadOnlyDictionary<string, PricingTerm>>> dictionary = (IReadOnlyDictionary<Term, IReadOnlyDictionary<string, IReadOnlyDictionary<string, PricingTerm>>>)value;

            writer.WriteStartObject();

            foreach (KeyValuePair<Term, IReadOnlyDictionary<string, IReadOnlyDictionary<string, PricingTerm>>> item in dictionary)
            {
                writer.WritePropertyName(item.Key.ToString());
                writer.WriteValue(item.Value);
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            // Get the second type of the dictionary, for ProductOffer Terms, the dictionary looks like
            // Dictionary<Term, IDictionary<string, IDictionary<string, PricingTerm>>>, so the type is
            // IDictionary<string, IDictionary<string, PricingTerm>>
            Type valueType = objectType.GenericTypeArguments[1];

            // Create a dictionary where the key is a string instead of the Term Enum
            IDictionary intermediateDictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(typeof(string), valueType));

            // Populate the dictionary from the Json, this is what would normally happen,
            // but we will need to take another step to swap the string key for the enum
            serializer.Populate(reader, intermediateDictionary);

            // This is the final dictionary that will be populated
            var finalDictionary = (IDictionary)Activator.CreateInstance(objectType);

            // Populate the dictionary with the values from the intermediate dictionary
            // and populate the appropriate keys, this should only loop twice, once for
            // OnDemand and then for Reserved
            foreach (DictionaryEntry pair in intermediateDictionary)
            {
                switch (pair.Key.ToString().ToLower())
                {
                    case "ondemand":
                        {
                            finalDictionary.Add(Term.ON_DEMAND, pair.Value);
                            break;
                        }
                    case "reserved":
                        {
                            finalDictionary.Add(Term.RESERVED, pair.Value);
                            break;
                        }
                    default:
                    case "unknown":
                        {
                            finalDictionary.Add(Term.UNKNOWN, pair.Value);
                            break;
                        }
                }
            }

            return finalDictionary;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        #endregion
    }
}
