using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace BAMCIS.AWSPriceListApi.Serde
{
    /// <summary>
    /// Custom converter for the LeastContractLength property in the price list API termAttributes property
    /// </summary>
    internal sealed class LeaseContractLengthConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => true;

        public override bool CanWrite => true;

        #endregion

        #region Public Methods

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Int32 Length = (Int32)value;

            writer.WriteValue($"{Length}yr");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string Value = (string)reader.Value;

            Match M = Regex.Match(Value, "^([0-9]+).*$");

            if (M.Success)
            {
                string YearsString = M.Groups[1].Value;

                if (Int32.TryParse(YearsString, out int Years))
                {
                    return Years;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType) == typeof(Int32);
        }

        #endregion
    }
}
