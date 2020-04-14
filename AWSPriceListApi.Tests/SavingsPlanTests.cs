using BAMCIS.AWSPriceListApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AWSPriceListApi.Tests
{
    public class SavingsPlanTests
    {
        [Fact]
        public async Task GetSavingsPlan_VersionIndex_EC2()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetOfferIndexFileRequest indexRequest = new GetOfferIndexFileRequest();

            GetOfferIndexFileResponse indexResponse = await client.GetOfferIndexFileAsync(indexRequest);

            GetSavingsPlanVersionIndexRequest spVersionIndexRequest = new GetSavingsPlanVersionIndexRequest(indexResponse.OfferIndexFile.Offers["AmazonEC2"].SavingsPlanVersionIndexUrl);

            // ACT
            GetSavingsPlanVersionIndexResponse spVersionIndexResponse = await client.GetSavingsPlanVersionIndexAsync(spVersionIndexRequest);

            // ASSERT
            Assert.Equal(200, (int)spVersionIndexResponse.HttpStatusCode);
        }

        [Fact]
        public async Task GetSavingsPlan_VersionIndex_ECS()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetOfferIndexFileRequest indexRequest = new GetOfferIndexFileRequest();

            GetOfferIndexFileResponse indexResponse = await client.GetOfferIndexFileAsync(indexRequest);

            GetSavingsPlanVersionIndexRequest spVersionIndexRequest = new GetSavingsPlanVersionIndexRequest(indexResponse.OfferIndexFile.Offers["AmazonECS"].SavingsPlanVersionIndexUrl);

            // ACT
            GetSavingsPlanVersionIndexResponse spVersionIndexResponse = await client.GetSavingsPlanVersionIndexAsync(spVersionIndexRequest);

            // ASSERT
            Assert.Equal(200, (int)spVersionIndexResponse.HttpStatusCode);
        }

        [Fact]
        public async Task GetSavingsPlan_RegionIndex_EC2()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetOfferIndexFileRequest indexRequest = new GetOfferIndexFileRequest();

            GetOfferIndexFileResponse indexResponse = await client.GetOfferIndexFileAsync(indexRequest);

            GetSavingsPlanIndexFileRequest spRegionIndexRequest = new GetSavingsPlanIndexFileRequest(indexResponse.OfferIndexFile.Offers["AmazonEC2"].CurrentSavingsPlanIndexUrl);

            // ACT
            GetSavingsPlanIndexFileResponse spRegionIndexResponse = await client.GetSavingsPlanRegionIndexAsync(spRegionIndexRequest);

            // ASSERT
            Assert.Equal(200, (int)spRegionIndexResponse.HttpStatusCode);
        }

        [Fact]
        public async Task GetSavingsPlan_RegionIndex_ECS()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetOfferIndexFileRequest indexRequest = new GetOfferIndexFileRequest();

            GetOfferIndexFileResponse indexResponse = await client.GetOfferIndexFileAsync(indexRequest);

            GetSavingsPlanIndexFileRequest spRegionIndexRequest = new GetSavingsPlanIndexFileRequest(indexResponse.OfferIndexFile.Offers["AmazonECS"].CurrentSavingsPlanIndexUrl);

            // ACT
            GetSavingsPlanIndexFileResponse spRegionIndexResponse = await client.GetSavingsPlanRegionIndexAsync(spRegionIndexRequest);

            // ASSERT
            Assert.Equal(200, (int)spRegionIndexResponse.HttpStatusCode);
        }

        [Fact]
        public async Task GetSavingsPlan_Offer_UsEast1_EC2()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetOfferIndexFileRequest indexRequest = new GetOfferIndexFileRequest();

            GetOfferIndexFileResponse indexResponse = await client.GetOfferIndexFileAsync(indexRequest);

            GetSavingsPlanIndexFileRequest spRegionIndexRequest = new GetSavingsPlanIndexFileRequest(indexResponse.OfferIndexFile.Offers["AmazonEC2"].CurrentSavingsPlanIndexUrl);
            GetSavingsPlanIndexFileResponse spRegionIndexResponse = await client.GetSavingsPlanRegionIndexAsync(spRegionIndexRequest);

            GetSavingsPlanRequest spRequest = new GetSavingsPlanRequest(spRegionIndexResponse.RegionIndex.Regions.First(x => x.RegionCode.Equals("us-east-1")).VersionUrl);

            // ACT
            GetSavingsPlanResponse spResponse = await client.GetSavingsPlanAsync(spRequest);

            // ASSERT
            Assert.Equal(200, (int)spResponse.HttpStatusCode);
            Assert.NotNull(spResponse.SavingsPlan.Products);
            Assert.True(spResponse.SavingsPlan.Products.Any());
            Assert.NotNull(spResponse.SavingsPlan.Terms);
            Assert.NotNull(spResponse.SavingsPlan.Terms.SavingsPlan);
            Assert.True(spResponse.SavingsPlan.Terms.SavingsPlan.Any());
        }

        [Fact]
        public async Task GetSavingsPlan_Offer_UsEast1_ECS()
        {
            // ARRANGE
            PriceListClient client = new PriceListClient();
            GetOfferIndexFileRequest indexRequest = new GetOfferIndexFileRequest();

            GetOfferIndexFileResponse indexResponse = await client.GetOfferIndexFileAsync(indexRequest);

            GetSavingsPlanIndexFileRequest spRegionIndexRequest = new GetSavingsPlanIndexFileRequest(indexResponse.OfferIndexFile.Offers["AmazonECS"].CurrentSavingsPlanIndexUrl);
            GetSavingsPlanIndexFileResponse spRegionIndexResponse = await client.GetSavingsPlanRegionIndexAsync(spRegionIndexRequest);

            GetSavingsPlanRequest spRequest = new GetSavingsPlanRequest(spRegionIndexResponse.RegionIndex.Regions.First(x => x.RegionCode.Equals("us-east-1")).VersionUrl);

            // ACT
            GetSavingsPlanResponse spResponse = await client.GetSavingsPlanAsync(spRequest);

            // ASSERT
            Assert.Equal(200, (int)spResponse.HttpStatusCode);
            Assert.NotNull(spResponse.SavingsPlan.Products);
            Assert.True(spResponse.SavingsPlan.Products.Any());
            Assert.NotNull(spResponse.SavingsPlan.Terms);
            Assert.NotNull(spResponse.SavingsPlan.Terms.SavingsPlan);
            Assert.True(spResponse.SavingsPlan.Terms.SavingsPlan.Any());
        }
    }
}
