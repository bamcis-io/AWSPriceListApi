using BAMCIS.AWSPriceListApi;
using BAMCIS.AWSPriceListApi.Model;
using BAMCIS.AWSPriceListApi.Serde;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AWSPriceListApi.Tests
{
    public class PriceListApiTests
    {
        [Fact]
        public async Task TestGetProduct_ElastiCache_SingleSkuFromOffer()
        {
            // ARRANGE
            string sku = "HBRQZSXXSY2DXJ77";

            PriceListClient client = new PriceListClient();

            GetProductRequest request = new GetProductRequest("AmazonElastiCache")
            {
                Format = Format.JSON
            };

            GetProductResponse response = await client.GetProductAsync(request);
            ProductOffer ecOffer = response.ProductOffer;

            // ACT

            IEnumerable<IGrouping<string, PricingTerm>> groupedTerms = ecOffer.Terms
                .SelectMany(x => x.Value) // Get all of the product item dictionaries from on demand and reserved
                                          //.Where(x => ApplicableProductSkus.Contains(x.Key)) // Only get the pricing terms for products we care about
                .SelectMany(x => x.Value) // Get all of the pricing term key value pairs
                .Select(x => x.Value) // Get just the pricing terms
                .GroupBy(x => x.Sku); // Put all of the same skus together

            IGrouping<string, PricingTerm> skuTerms = groupedTerms.First(x => x.Key.Equals(sku));

            // ASSERT
            Assert.True(skuTerms.Where(x => x.TermAttributes.PurchaseOption == PurchaseOption.ON_DEMAND).Count() == 1);
        }

        [Fact]
        public async Task TestGetProduct_EC2()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();

            GetProductRequest request = new GetProductRequest("AmazonEC2")
            {
                Format = Format.JSON
            };

            // ACT
            GetProductResponse response = await client.GetProductAsync(request);

            ProductOffer ec2Offer = response.ProductOffer;

            // ASSERT
            Assert.NotNull(ec2Offer);
        }

        [Fact]
        public async Task TestGetProduct_ECS_FromJsonContentStream()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();

            GetProductRequest request = new GetProductRequest("AmazonECS")
            {
                Format = Format.JSON
            };

            // ACT
            GetProductResponse response = await client.GetProductAsync(request);

            ProductOffer offer = ProductOffer.FromJsonStream(response.Content);

            // ASSERT
            Assert.NotNull(offer);
        }

        [Fact]
        public async Task TestGetProduct_ECS_FromJsonString()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();

            GetProductRequest request = new GetProductRequest("AmazonECS")
            {
                Format = Format.JSON
            };

            // ACT
            GetProductResponse response = await client.GetProductAsync(request);

            bool success = response.TryGetResponseContentAsString(out string productInfo);

            // ASSERT
            Assert.True(success);

            ProductOffer offer = ProductOffer.FromJson(productInfo);

            Assert.NotNull(offer);
        }

        [Fact]
        public void TestGetProduct_EC2ReservedHost_FromJsonFile()
        {
            // ARRANGE
            string sku = "R788QK3FA3RPDDXZ";

            string json = File.ReadAllText("ReservedHostEC2.json");

            ProductOffer ec2Offer = ProductOffer.FromJson(json);

            // ACT

            IEnumerable<IGrouping<string, PricingTerm>> groupedTerms = ec2Offer.Terms
                .SelectMany(x => x.Value) // Get all of the product item dictionaries from on demand and reserved
                                          //.Where(x => ApplicableProductSkus.Contains(x.Key)) // Only get the pricing terms for products we care about
                .SelectMany(x => x.Value) // Get all of the pricing term key value pairs
                .Select(x => x.Value) // Get just the pricing terms
                .GroupBy(x => x.Sku); // Put all of the same skus together

            IGrouping<string, PricingTerm> skuTerms = groupedTerms.First(x => x.Key.Equals(sku));

            // ASSERT
            Assert.True(skuTerms.Where(x => x.TermAttributes.PurchaseOption == PurchaseOption.ON_DEMAND).Count() == 0);
        }

        [Fact]
        public async Task TestGetProduct_AmazonDynamoDB_FromJsonContentStream()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();

            GetProductRequest request = new GetProductRequest("AmazonDynamoDB")
            {
                Format = Format.JSON
            };

            GetProductResponse response = await client.GetProductAsync(request);

            // ACT
            ProductOffer ddbOffer = ProductOffer.FromJsonStream(response.Content);

            // ASSERT
            Assert.NotNull(ddbOffer);
            Assert.True(!String.IsNullOrEmpty(ddbOffer.Version));
        }

        [Fact]
        public void TestGetProduct_AmazonRedshift_FromJsonFile()
        {
            // ARRANGE
            using (FileStream stream = File.OpenRead("AmazonRedshift.json"))
            {
                byte[] bytes = new byte[stream.Length];

                stream.Read(bytes, 0, bytes.Length);

                string json = Encoding.UTF8.GetString(bytes);

                // ACT
                ProductOffer offer = ProductOffer.FromJson(json);

                // ASSERT
                Assert.NotNull(offer);
            }
        }

        [Fact]
        public async Task TestListServices()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();

            // ACT
            ListServicesResponse services = await client.ListServicesAsync();

            // ASSERT
            Assert.True(!services.IsError());
            Assert.True(services.Services.Any());
        }

        [Fact]
        public async Task TestGetProduct_AmazonRDS_CSV()
        {
            // ARRANGE
            PriceListClientConfig config = new PriceListClientConfig();

            PriceListClient client = new PriceListClient(config);

            GetProductRequest request = new GetProductRequest("AmazonRDS")
            {
                Format = Format.CSV
            };

            // ACT
            GetProductResponse response = await client.GetProductAsync(request);

            // ASSERT
            Assert.True(!String.IsNullOrEmpty(response.ServiceCode));
        }

        [Fact]
        public async Task TestGetProduct_AmazonRDS()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetProductRequest request = new GetProductRequest("AmazonRDS");

            // ACT
            GetProductResponse response = await client.GetProductAsync(request);

            // ASSERT
            Assert.True(!response.IsError());
            Assert.True(!String.IsNullOrEmpty(response.ServiceCode));
        }

        [Fact]
        public async Task TestGetOfferIndexFile_WithoutRequest()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();

            // ACT
            GetOfferIndexFileResponse response = await client.GetOfferIndexFileAsync();

            // ASSERT
            Assert.True(!response.IsError());
            Assert.NotNull(response.OfferIndexFile);
        }

        [Fact]
        public async Task TestGetOfferIndexFile_WithRequest()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetOfferIndexFileRequest request = new GetOfferIndexFileRequest("/offers/v1.0/aws/index.json");

            // ACT
            GetOfferIndexFileResponse response = await client.GetOfferIndexFileAsync(request);

            // ASSERT
            Assert.True(!response.IsError());
            Assert.NotNull(response.OfferIndexFile);
        }

        [Fact]
        public async Task TestGetRegionIndexFile_AllServices()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetOfferIndexFileResponse response = await client.GetOfferIndexFileAsync();
            ConcurrentBag<RegionIndex> indices = new ConcurrentBag<RegionIndex>();

            // ACT
            IEnumerable<Task> tasks = response.OfferIndexFile.Offers.Select(async x =>
            {
                GetRegionIndexResponse regionIndexResponse = await client.GetRegionIndexAsync(new GetRegionIndexRequest(x.Value));
                if (regionIndexResponse != null && regionIndexResponse.RegionIndex != null)
                {
                    indices.Add(regionIndexResponse.RegionIndex);
                }
            });

            await Task.WhenAll(tasks);

            // ASSERT
            Assert.Equal(response.OfferIndexFile.Offers.Where(x => !String.IsNullOrEmpty(x.Value.CurrentRegionIndexUrl)).Count(), indices.Count);
            Assert.All(indices, x => Assert.NotNull(x));
        }

        [Fact]
        public async Task TestGetVersionIndexFile_AllServices()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetOfferIndexFileResponse response = await client.GetOfferIndexFileAsync();
            ConcurrentBag<VersionIndex> indices = new ConcurrentBag<VersionIndex>();

            // ACT
            IEnumerable<Task> tasks = response.OfferIndexFile.Offers.Select(async x =>
            {
                GetVersionIndexResponse versionIndexResponse = await client.GetVersionIndexAsync(new GetVersionIndexRequest(x.Value));

                if (versionIndexResponse != null && versionIndexResponse.VersionIndex != null)
                {
                    indices.Add(versionIndexResponse.VersionIndex);
                }
            });

            await Task.WhenAll(tasks);

            // ASSERT
            Assert.Equal(response.OfferIndexFile.Offers.Where(x => !String.IsNullOrEmpty(x.Value.VersionIndexUrl)).Count(), indices.Count);
            Assert.All(indices, x => Assert.NotNull(x));
        }

        [Fact]
        public async Task TestGetVerionIndexFile_AmazonEC2_WithRelativePath()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetOfferIndexFileResponse response = await client.GetOfferIndexFileAsync();

            string versionIndexUrl = response.OfferIndexFile.Offers["AmazonEC2"].VersionIndexUrl;

            // ACT
            GetVersionIndexResponse versionResponse = await client.GetVersionIndexAsync(new GetVersionIndexRequest(versionIndexUrl));

            // ASSERT
            Assert.Equal(200, (int)versionResponse.HttpStatusCode);
        }

        [Fact]
        public async Task TestGetVerionIndexFile_AmazonEC2_WithOffer()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetOfferIndexFileResponse response = await client.GetOfferIndexFileAsync();

            Offer offer = response.OfferIndexFile.Offers["AmazonEC2"];

            // ACT
            GetVersionIndexResponse versionResponse = await client.GetVersionIndexAsync(new GetVersionIndexRequest(offer));

            // ASSERT
            Assert.Equal(200, (int)versionResponse.HttpStatusCode);
        }
    }
}
