using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Serde
{
    /// <summary>
    /// Custom converter for the Term enum
    /// </summary>
    public class TermConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => true;

        public override bool CanWrite => true;

        #endregion

        #region Public Methods

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Term Term = (Term)value;

            switch (Term)
            {
                case Term.ON_DEMAND:
                    {
                        writer.WriteValue("OnDemand");
                        break;
                    }
                case Term.RESERVED:
                    {
                        writer.WriteValue("Reserved");
                        break;
                    }
                case Term.UNKNOWN:
                    {
                        writer.WriteValue("Unknown");
                        break;
                    }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return EnumConverters.ConvertToTerm(reader.Value as string);
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType) == typeof(Term);
        }

        #endregion
    }
}