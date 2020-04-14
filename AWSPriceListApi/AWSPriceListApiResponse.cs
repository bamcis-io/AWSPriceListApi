using Amazon.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace BAMCIS.AWSPriceListApi
{
    public abstract class AWSPriceListApiResponse<T> : AmazonWebServiceResponse, IDisposable
    {
        #region Private Fields

        private static volatile object sync = new object();

        #endregion

        #region Public Properties

        /// <summary>
        /// The content of the response. If the content was serialized in JSON, this
        /// class will automatically handle deserializing it. However, if it was returned
        /// in CSV, then you can use this stream to read the CSV data.
        /// </summary>
        public Stream Content { get; protected set; }

        /// <summary>
        /// The data returned by the request. If the request used CSV as a format,
        /// this property will be null.
        /// </summary>
        public T Data { get; protected set; }

        /// <summary>
        /// The format of the data, i.e. json or csv
        /// </summary>
        public Format Format { get; }

        #endregion

        #region Constructors

        public AWSPriceListApiResponse(HttpResponseMessage response) : this(response, Format.JSON, false)
        { }

        public AWSPriceListApiResponse(HttpResponseMessage response, Format format) : this(response, format, false)
        { }

        public AWSPriceListApiResponse(HttpResponseMessage response, bool disposeResponse) : this(response, Format.JSON, disposeResponse)
        { }

        public AWSPriceListApiResponse(HttpResponseMessage response, Format format, bool disposeResponse)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            try
            {
                this.HttpStatusCode = response.StatusCode;
                this.ResponseMetadata = new ResponseMetadata();
                this.Content = new MemoryStream();
                this.Format = format;

                if (response.Content != null)
                {
                    if (response.Content.Headers.ContentLength != null)
                    {
                        this.Content.SetLength((long)response.Content.Headers.ContentLength);
                    }

                    response.Content.ReadAsStreamAsync().Result.CopyTo(this.Content);
                    this.Content.Position = 0;

                    if (!response.IsSuccessStatusCode)
                    {
                        using (StreamReader streamReader = new StreamReader(this.Content))
                        {
                            this.ResponseMetadata.Metadata.Add(new KeyValuePair<string, string>("ErrorReason", streamReader.ReadToEnd()));
                        }
                    }
                    else
                    {
                        this.GenerateData();
                    }
                }

                IEnumerable<string> values;

                if (response.Headers.TryGetValues("x-amz-request-id", out values))
                {
                    this.ResponseMetadata.RequestId = values.First();
                }
                else
                {
                    this.ResponseMetadata.RequestId = String.Empty;
                }
            }
            finally
            {
                if (disposeResponse)
                {
                    try
                    {
                        response.Content?.Dispose();
                    }
                    catch { }

                    try
                    {
                        response?.Dispose();
                    }
                    catch { }
                }
            }
        }

        public AWSPriceListApiResponse(AWSPriceListApiResponse<T> response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            this.HttpStatusCode = response.HttpStatusCode;
            this.ContentLength = response.ContentLength;
            this.ResponseMetadata = new ResponseMetadata();
            this.Content = new MemoryStream();
            response.Content.Position = 0;
            response.Content.CopyTo(this.Content);
            this.GenerateData();

            if (response.ResponseMetadata != null)
            {
                this.ResponseMetadata.RequestId = response.ResponseMetadata.RequestId;

                foreach (KeyValuePair<string, string> item in response.ResponseMetadata.Metadata)
                {
                    this.ResponseMetadata.Metadata.Add(new KeyValuePair<string, string>(item.Key, item.Value));
                }
            }
        }

        ~AWSPriceListApiResponse()
        {
            this.Dispose();
        }

        #endregion

        #region Public Methods

        public virtual bool IsError()
        {
            return this.ResponseMetadata.Metadata.ContainsKey("ErrorReason");
        }

        public void Dispose()
        {
            try
            {
                this.Content?.Dispose();
            }
            catch { }
        }

        /// <summary>
        /// Attempts to get the response data as a string. Some responses may
        /// be too large for a single string, like the AmazonEC2 offer file
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        public bool TryGetResponseContentAsString(out string productInfo)
        {
            try
            {
                lock (sync)
                {
                    this.Content.Position = 0;
                    using (StreamReader reader = new StreamReader(this.Content))
                    {
                        productInfo = reader.ReadToEnd();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                productInfo = String.Empty;
                return false;
            }
        }

        #endregion

        #region Private Methods

        private void GenerateData()
        {
            if (this.Format == Format.JSON && this.Content != null && this.Content.Length > 0)
            {
                // Defaults derived from https://referencesource.microsoft.com/#mscorlib/system/io/streamreader.cs
                Encoding encoding = Encoding.UTF8;
                bool detectEncodingFromByteOrderMarks = true;
                int defaultBufferSize = 1024;
                bool leaveUnderlyingStreamOpen = true; // We don't want to dispose of the content stream so it can be read again later

                this.Content.Position = 0;

                using (StreamReader streamReader = new StreamReader(this.Content, encoding, detectEncodingFromByteOrderMarks, defaultBufferSize, leaveUnderlyingStreamOpen))
                {
                    using (JsonReader reader = new JsonTextReader(streamReader))
                    {
                        JsonSerializer serializer = new JsonSerializer();

                        // read the json from a stream
                        // json size doesn't matter because only a small piece is read at a time from the HTTP request
                        this.Data = serializer.Deserialize<T>(reader);
                    }
                }

                // Reset the position
                this.Content.Position = 0;
            }
        }

        #endregion
    }
}
