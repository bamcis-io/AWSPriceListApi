# BAMCIS AWS Price List API

This project is a convenience wrapper around the AWS Price List API. It uses the publicly available
endpoint by default to retrieve the offer index files and the specific service offer
files the offer index file points to. You can also retrieve the version index file and region
index file in case you want to get the URLs for those specific offer files.

## Table of Contents
- [Usage](#usage)
    * [Price List Client](#price-list-client)
    * [Index Files](#index-files)
- [Revision History](#revision-history)

## Usage

### Price List Client

Here's an example of some usage:

    PriceListClientConfig Config = new PriceListClientConfig()
    {
        Extension = Extension.CSV
    };

    PriceListClient Client = new PriceListClient(Config);

    string Content = await Client.GetProductAsync("AmazonRDS");

    System.IO.File.WriteAllText("c:\\users\\me\\desktop\\rds.csv", Content);

This set of commands gets the pricing information for RDS and writes the CSV content to a file.

### Index Files

    OfferIndexFile File = await OfferIndexFile.GetAsync();
    ConcurrentBag<RegionIndex> Indexes = new ConcurrentBag<RegionIndex>();

    IEnumerable<Task> Tasks = File.Offers.Select(async x =>
    {
        RegionIndex Index = await x.Value.GetRegionIndexAsync();
        if (Index != null)
        {
            Indexes.Add(Index);
        }                       
    });

    await Task.WhenAll(Tasks);

This example gets the region index files for the services that have a region index file url
defined in the offer index file.

You could have also used the `PriceListClient` to do the same thing.

    PriceListClient Client = new PriceListClient();
	OfferIndexFile File = await Client.GetOfferIndexFileAsync();

### Listing Services

    PriceListClient Client = new PriceListClient();
    IEnumerable<string> Services = await Client.ListServicesAsync();

This provides a list of all services that have pricing data available in the
offer index file.

## Revision History

### 2.0.0
Wrapped all request and response for the client in their own classes.

### 1.0.0
Initial release of the application.