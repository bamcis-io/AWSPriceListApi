using BAMCIS.AWSPriceListApi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AWSPriceListApi.Tests
{
    public class PriceListApiTests
    {
        [Fact]
        public async Task ListServicesTest()
        {
            // ARRANGE
            PriceListClient Client = new PriceListClient();

            // ACT
            IEnumerable<string> Services = await Client.ListServicesAsync();

            // ASSERT
            Assert.True(Services.Any());
        }

        [Fact]
        public async Task PrictListProductTestCsv()
        {
            // ARRANGE
            PriceListClientConfig Config = new PriceListClientConfig()
            {
                Extension = Extension.CSV
            };

            PriceListClient Client = new PriceListClient(Config);

            // ACT
            string Content = await Client.GetProductAsync("AmazonRDS");

            // ASSERT
            Assert.True(!String.IsNullOrEmpty(Content));

            System.IO.File.WriteAllText("d:\\users\\mhaken\\desktop\\rds.csv", Content);
        }

        [Fact]
        public async Task PrictListProductTest()
        {
            // ARRANGE
            PriceListClient Client = new PriceListClient();

            // ACT
            string Content = await Client.GetProductAsync("AmazonRDS");

            // ASSERT
            Assert.True(!String.IsNullOrEmpty(Content));
        }

        [Fact]
        public async Task PriceListClientTest()
        {
            // ARRANGE
            PriceListClient Client = new PriceListClient();

            // ACT
            OfferIndexFile Index = await Client.GetOfferIndexFileAsync();

            // ASSERT
            Assert.NotNull(Index);
        }

        [Fact]
        public async Task PriceListClientTest2()
        {
            // ARRANGE
            PriceListClient Client = new PriceListClient();

            // ACT
            OfferIndexFile Index = await Client.GetOfferIndexFileAsync(PriceListClient.DefaultOfferIndexFileUrl);

            // ASSERT
            Assert.NotNull(Index);
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
