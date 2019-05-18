using BAMCIS.AWSPriceListApi;
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
        public async Task TestElastiCache()
        {
            // ARRANGE
            string Sku = "HBRQZSXXSY2DXJ77";

            PriceListClient Client = new PriceListClient();

            GetProductRequest Request = new GetProductRequest("AmazonElastiCache")
            {
                Format = Format.JSON
            };

            GetProductResponse Response = await Client.GetProductAsync(Request);

            ProductOffer ECOffer = ProductOffer.FromJson(Response.GetProductInfoAsString());

            // ACT

            IEnumerable<IGrouping<string, PricingTerm>> GroupedTerms = ECOffer.Terms
                .SelectMany(x => x.Value) // Get all of the product item dictionaries from on demand and reserved
                                          //.Where(x => ApplicableProductSkus.Contains(x.Key)) // Only get the pricing terms for products we care about
                .SelectMany(x => x.Value) // Get all of the pricing term key value pairs
                .Select(x => x.Value) // Get just the pricing terms
                .GroupBy(x => x.Sku); // Put all of the same skus together

            IGrouping<string, PricingTerm> SkuTerms = GroupedTerms.First(x => x.Key.Equals(Sku));

            // ASSERT
            Assert.True(SkuTerms.Where(x => x.TermAttributes.PurchaseOption == PurchaseOption.ON_DEMAND).Count() == 1);
        }

        [Fact]
        public void TestEC2ReservedHost()
        {
            // ARRANGE
            string Sku = "R788QK3FA3RPDDXZ";

            string Json = File.ReadAllText("ReservedHostEC2.json");

            ProductOffer ECOffer = ProductOffer.FromJson(Json);

            // ACT

            IEnumerable<IGrouping<string, PricingTerm>> GroupedTerms = ECOffer.Terms
                .SelectMany(x => x.Value) // Get all of the product item dictionaries from on demand and reserved
                                          //.Where(x => ApplicableProductSkus.Contains(x.Key)) // Only get the pricing terms for products we care about
                .SelectMany(x => x.Value) // Get all of the pricing term key value pairs
                .Select(x => x.Value) // Get just the pricing terms
                .GroupBy(x => x.Sku); // Put all of the same skus together

            IGrouping<string, PricingTerm> SkuTerms = GroupedTerms.First(x => x.Key.Equals(Sku));

            // ASSERT
            Assert.True(SkuTerms.Where(x => x.TermAttributes.PurchaseOption == PurchaseOption.ON_DEMAND).Count() == 0);
        }

        [Fact]
        public async Task ParseJsonTest()
        {
            // ARRANGE
            PriceListClient Client = new PriceListClient();

            GetProductRequest Request = new GetProductRequest("AmazonDynamoDB")
            {
                Format = Format.JSON
            };

            GetProductResponse Response = await Client.GetProductAsync(Request);

            // ACT
            ProductOffer DDBOffer = ProductOffer.FromJson(Response.GetProductInfoAsString());

            // ASSERT
            Assert.True(!String.IsNullOrEmpty(DDBOffer.Version));
        }

        [Fact]
        public void FromJsonTest()
        {
            // ARRANGE
            using (FileStream Stream = File.OpenRead("index.json"))
            {
                byte[] Bytes = new byte[Stream.Length];

                Stream.Read(Bytes, 0, Bytes.Length);

                string Json = Encoding.UTF8.GetString(Bytes);

                // ACT
                ProductOffer Offer = ProductOffer.FromJson(Json);

                // ASSERT
                Assert.NotNull(Offer);
            }
        }

        [Fact]
        public async Task ListServicesTest()
        {
            // ARRANGE
            PriceListClient Client = new PriceListClient();

            // ACT
            ListServicesResponse Services = await Client.ListServicesAsync();

            // ASSERT
            Assert.True(!Services.IsError);
            Assert.True(Services.Services.Any());
        }

        [Fact]
        public async Task PrictListProductTestCsv()
        {
            // ARRANGE
            PriceListClientConfig Config = new PriceListClientConfig();

            PriceListClient Client = new PriceListClient(Config);

            GetProductRequest Request = new GetProductRequest("AmazonRDS")
            {
                Format = Format.CSV
            };

            // ACT
            GetProductResponse Response = await Client.GetProductAsync(Request);

            // ASSERT
            Assert.True(!String.IsNullOrEmpty(Response.ServiceCode));
        }

        [Fact]
        public async Task PrictListProductTest()
        {
            // ARRANGE
            PriceListClient Client = new PriceListClient();
            GetProductRequest Request = new GetProductRequest("AmazonRDS");

            // ACT
            GetProductResponse Response = await Client.GetProductAsync(Request);

            // ASSERT
            Assert.True(!Response.IsError);
            Assert.True(!String.IsNullOrEmpty(Response.ServiceCode));
        }

        [Fact]
        public async Task PriceListClientTest()
        {
            // ARRANGE
            PriceListClient Client = new PriceListClient();

            // ACT
            GetOfferIndexFileResponse Response = await Client.GetOfferIndexFileAsync();

            // ASSERT
            Assert.True(!Response.IsError);
            Assert.NotNull(Response.OfferIndexFile);
        }

        [Fact]
        public async Task PriceListClientTest2()
        {
            // ARRANGE
            PriceListClient Client = new PriceListClient();
            GetOfferIndexFileRequest Request = new GetOfferIndexFileRequest("/offers/v1.0/aws/index.json");

            // ACT
            GetOfferIndexFileResponse Response = await Client.GetOfferIndexFileAsync(Request);

            // ASSERT
            Assert.True(!Response.IsError);
            Assert.NotNull(Response.OfferIndexFile);
        }

        [Fact]
        public async Task OfferIndexFileTest()
        {
            // ARRANGE

            // ACT
            OfferIndexFile File = await OfferIndexFile.GetAsync();

            // ASSERT
            Assert.NotNull(File);
        }

        [Fact]
        public async Task RegionIndexFileTest()
        {
            // ARRANGE
            OfferIndexFile File = await OfferIndexFile.GetAsync();
            ConcurrentBag<RegionIndex> Indexes = new ConcurrentBag<RegionIndex>();

            // ACT
            IEnumerable<Task> Tasks = File.Offers.Select(async x =>
            {
                int n = 0;

                while (true)
                {
                    try
                    {
                        RegionIndex Index = await x.Value.GetRegionIndexAsync();
                        if (Index != null)
                        {
                            Indexes.Add(Index);
                        }
                        
                        break;
                    }
                    catch (HttpRequestException e)
                    {
                        if (n < 5)
                        {
                            Thread.Sleep((n++ * 1000) + 500);
                        }
                        else
                        {
                            throw e;
                        }

                    }
                }
            });

            await Task.WhenAll(Tasks);

            // ASSERT
            Assert.Equal(File.Offers.Where(x => !String.IsNullOrEmpty(x.Value.CurrentRegionIndexUrl)).Count(), Indexes.Count);

            foreach (RegionIndex Index in Indexes)
            {
                Assert.NotNull(Index);
            }
        }

        [Fact]
        public async Task VersionIndexFileTest()
        {
            // ARRANGE
            OfferIndexFile File = await OfferIndexFile.GetAsync();
            ConcurrentBag<VersionIndex> Indexes = new ConcurrentBag<VersionIndex>();

            // ACT
            IEnumerable<Task> Tasks = File.Offers.Select(async x =>
            {
                int n = 0;
                while (true)
                {
                    try
                    {
                        VersionIndex Index = await x.Value.GetVersionIndexAsync();
                        Indexes.Add(Index);
                        break;
                    }
                    catch (HttpRequestException e)
                    {
                        if (n < 5)
                        {
                            Thread.Sleep((1000 * n++) + 500);
                        }
                        else
                        {
                            throw e;
                        }
                    }
                }
            });

            await Task.WhenAll(Tasks);

            // ASSERT
            Assert.Equal(File.Offers.Where(x => !String.IsNullOrEmpty(x.Value.VersionIndexUrl)).Count(), Indexes.Count);

            foreach (VersionIndex Index in Indexes)
            {
                Assert.NotNull(Index);
            }
        }
    }
}
