using Newtonsoft.Json;
using System;

namespace BAMCIS.AWSPriceListApi.Serde
{
    /// <summary>
    /// Custom converter for the PurchaseOption enum
    /// </summary>
    internal sealed class PurchaseOptionConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => true;

        public override bool CanWrite => true;

        #endregion

        #region Public Methods

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PurchaseOption Option = (PurchaseOption)value;

            switch (Option)
            {
                case PurchaseOption.ON_DEMAND:
                    {
                        writer.WriteValue("OnDemand");
                        break;
                    }
                case PurchaseOption.ALL_UPFRONT:
                    {
                        writer.WriteValue("All Upfront");
                        break;
                    }
                case PurchaseOption.PARTIAL_UPFRONT:
                    {
                        writer.WriteValue("Partial Upfront");
                        break;
                    }
                case PurchaseOption.NO_UPFRONT:
                    {
                        writer.WriteValue("No Upfront");
                        break;
                    }
                case PurchaseOption.HEAVY_UTILIZATION:
                    {
                        writer.WriteValue("Heavy Utilization");
                        break;
                    }
                case PurchaseOption.MEDIUM_UTILIZATION:
                    {
                        writer.WriteValue("Medium Utilization");
                        break;
                    }
                case PurchaseOption.LIGHT_UTILIZATION:
                    {
                        writer.WriteValue("Light Utilization");
                        break;
                    }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return EnumConverters.ConvertToPurchaseOption(reader.Value as string);      
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType) == typeof(PurchaseOption);
        }

        #endregion
    }
}
