using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Serde
{
    /// <summary>
    /// Custom converter for the OfferingClass enum
    /// </summary>
    internal sealed class OfferingClassConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => true;

        public override bool CanWrite => true;

        #endregion

        #region Public Methods

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            OfferingClass Class = (OfferingClass)value;

            switch (Class)
            {
                case OfferingClass.STANDARD:
                    {
                        writer.WriteValue("Standard");
                        break;
                    }
                case OfferingClass.CONVERTIBLE:
                    {
                        writer.WriteValue("Convertible");
                        break;
                    }
                default:
                case OfferingClass.UNKNOWN:
                    {
                        writer.WriteValue("Unknown");
                        break;
                    }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return EnumConverters.ConvertToOfferingClass(reader.Value as string);
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType) == typeof(OfferingClass);
        }

        #endregion
    }
}
