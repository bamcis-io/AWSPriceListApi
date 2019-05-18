# BAMCIS AWS Price List API

This project is a convenience wrapper around the AWS Price List API. It uses the publicly available
endpoint by default to retrieve the offer index files and the specific service offer
files the offer index file points to. You can also retrieve the version index file and region
index file in case you want to get the URLs for those specific offer files.

## Table of Contents
- [Usage](#usage)
    * [Price List Client](#price-list-client)
    * [Index Files](#index-files)
	* [Serde](#serde)
- [Revision History](#revision-history)

## Usage

### Price List Client

Here's an example of some usage:

    PriceListClientConfig config = new PriceListClientConfig();

    PriceListClient client = new PriceListClient(config);

    GetProductRequest request = new GetProductRequest("AmazonRDS")
    {
        Format = Format.CSV
    };

    GetProductResponse response = await Client.GetProductAsync(request);

    System.IO.File.WriteAllText("c:\\users\\me\\desktop\\rds.csv", response.GetProductInfoAsString());

This set of commands gets the pricing information for RDS and writes the CSV content to a file.

### Index Files

    OfferIndexFile file = await OfferIndexFile.GetAsync();
    ConcurrentBag<RegionIndex> indexes = new ConcurrentBag<RegionIndex>();

    IEnumerable<Task> tasks = file.Offers.Select(async x =>
    {
        RegionIndex index = await x.Value.GetRegionIndexAsync();
        if (index != null)
        {
            indexes.Add(index);
        }                       
    });

    await Task.WhenAll(tasks);

This example gets the region index files for the services that have a region index file url defined in the offer index file.

You could have also used the `PriceListClient` to do the same thing.

    PriceListClient client = new PriceListClient();
	GetOfferIndexFileResponse response = await client.GetOfferIndexFileAsync();
	OfferIndexFile file = response.OfferIndexFile;

### Listing Services

    PriceListClient client = new PriceListClient();
    ListServicesResponse response = await client.ListServicesAsync();
	IEnumerable<string> services = response.Services;

This provides a list of all services that have pricing data available in the offer index file.

### Serde

There are serialization/deserialization classes for parsing the JSON content from a `GetProductResponse`. For example,

    PriceListClientConfig config = new PriceListClientConfig();

    PriceListClient client = new PriceListClient(config);

    GetProductRequest request = new GetProductRequest("AmazonRDS")
    {
        Format = Format.CSV
    };

    GetProductResponse response = await client.GetProductAsync(request);

	ProductOffer rdsOffer = ProductOffer.FromJson(response.GetProductInfoAsString());
  
Now in the `rdsOffer` object you can explore the different pricing terms, dimensions in each term, and the set of products in that offer. 

## Revision History

### 3.0.0
To prevent unnecessary memory utilization the response data is maintained as a stream instead of being read as a string by default. This allows you to read or copy the stream to another stream to perform csv or json operations.

### 2.3.1
Fixed bug in the PurchaseOption parsing. The price list files sometimes contain spaces between words like "All Upfront" and sometimes not, like "AllUpfront". The EnumConverter now handles this.

### 2.3.0
Added convenience methods for converting purchase option, offering class, term, and lease contract length.

### 2.2.3
Corrected the handling of term attributes that are empty objects in the JSON file.

### 2.2.2
Made all dictionary key comparisons case insensitive.

### 2.2.1
Fixed property name `ProductInfo`.

### 2.2.0
Added deserialization classes for parsing the JSON for product offers.

### 2.1.2
Added disposing for http response messages.

### 2.1.1
Internal changes to handling URLs.

### 2.1.0
Updated GetProductRequest properties.

### 2.0.0
Wrapped all request and response for the client in their own classes.

### 1.0.0
Initial release of the application.
