﻿using Newtonsoft.Json;
using System;

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
            int length = (int)value;
            writer.WriteValue($"{length}yr");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return EnumConverters.ConvertToLeaseContractLength(reader.Value as string);
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType) == typeof(int);
        }

        #endregion
    }
}
