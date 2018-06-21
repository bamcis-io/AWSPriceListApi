using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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
            IReadOnlyDictionary<Term, IReadOnlyDictionary<string, IReadOnlyDictionary<string, PricingTerm>>> Dict = (IReadOnlyDictionary<Term, IReadOnlyDictionary<string, IReadOnlyDictionary<string, PricingTerm>>>)value;

            writer.WriteStartObject();

            foreach (KeyValuePair<Term, IReadOnlyDictionary<string, IReadOnlyDictionary<string, PricingTerm>>> Item in Dict)
            {
                writer.WritePropertyName(Item.Key.ToString());
                writer.WriteValue(Item.Value);
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            Type ValueType = objectType.GenericTypeArguments[1];
            Type IntermediateDictionaryType = typeof(Dictionary<,>).MakeGenericType(typeof(string), ValueType);
            IDictionary IntermediateDictionary = (IDictionary)Activator.CreateInstance(IntermediateDictionaryType);
            serializer.Populate(reader, IntermediateDictionary);

            var FinalDictionary = (IDictionary)Activator.CreateInstance(objectType);

            foreach (DictionaryEntry pair in IntermediateDictionary)
            {
                switch (pair.Key.ToString().ToLower())
                {
                    case "ondemand":
                        {
                            FinalDictionary.Add(Term.ON_DEMAND, pair.Value);
                            break;
                        }
                    case "reserved":
                        {
                            FinalDictionary.Add(Term.RESERVED, pair.Value);
                            break;
                        }
                    default:
                    case "unknown":
                        {
                            FinalDictionary.Add(Term.UNKNOWN, pair.Value);
                            break;
                        }
                }
            }

            return FinalDictionary;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        #endregion
    }
}
