# BAMCIS AWS Price List API
This project is a convenience wrapper around the AWS Price List API. It uses the publicly available endpoint by default to retrieve the offer index files and the specific service offer files the offer index file points to. You can also retrieve the version index file and region index file in case you want to get the URLs for those specific offer files.

## Table of Contents
- [Usage](#usage)
    * [Price List Client](#price-list-client)
    * [Index Files](#index-files)
	* [Serde](#serde)
- [Revision History](#revision-history)

## Usage

### Price List Client
Here's an example of some usage:

```csharp
PriceListClientConfig config = new PriceListClientConfig();
PriceListClient client = new PriceListClient(config);
GetProductRequest request = new GetProductRequest("AmazonRDS")
{
    Format = Format.CSV
};

GetProductResponse response = await client.GetProductAsync(request);

if (response.TryGetResponseDataAsString(out string data))
{
    System.IO.File.WriteAllText("c:\\users\\me\\desktop\\rds.csv", data);
}
```
This set of commands gets the pricing information for RDS and writes the CSV content to a file.

### Index Files
Index files contain data about where to get pricing data. This example first gets the "Offer Index File", which is metadata about where to find pricing information for each service. It looks like this:
```json
{
   "formatVersion":"The version number for the offer index format",
   "disclaimer":"The disclaimers for this offer index",
   "publicationDate":"The publication date of this offer index",
   "offers":{
        "firstService":{
             "offerCode":"The service that this price list is for",
             "versionIndexUrl" : "The URL for the index of versions of this data",
             "currentVersionUrl":"The URL for this offer file",
             "currentRegionIndexUrl":"The URL for the regional offer index file",
             "savingsPlanVersionIndexUrl":"The URL for the Savings Plan index file (if applicable)",
             "currentSavingsPlanIndexUrl":"The URL for the current regional Savings Plan index (if applicable)"
        },
        "secondService":{
             "offerCode":"The service that this price list is for",
             "versionIndexUrl" : "The URL for the index of versions of this data",
             "currentVersionUrl":"The URL for this offer file",
             "currentRegionIndexUrl":"The URL for the regional offer index file"
             "savingsPlanVersionIndexUrl":"The URL for the Savings Plan index file (if applicable)",
             "currentSavingsPlanIndexUrl":"The URL for the current regional Savings Plan index (if applicable)"
        },
        ...
   }, 
}
```
With this data, for each service you can get a list of available regional offer files via the `currentRegionIndexUrl`. This example gets the list of regional URLs for every service. The `RegionIndex` object contains the dictionary for each region and it's corresponding URL relative path for the service. Once all the tasks are complete, you'd have a region index for every available region for each service.

```csharp
PriceListClient client = newPriceListClient();
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
```

You can use each of the URL relative paths in the `RegionIndex` objects to get offer data for every service in every region. However, this is relatively inefficient, and you can more simply get the current offer data for all services in every region like this:
```csharp
PriceListClient client = new PriceListClient();
GetOfferIndexFileResponse response = await client.GetOfferIndexFileAsync();
OfferIndexFile file = response.OfferIndexFile;
```

The `OfferIndexFile` has the URLs to the current pricing data in the `CurrentVersionUrl` of each `Offer`, which is information specific to a service.

### Listing Services
This provides a list of all services that have pricing data available in the Offer Index File.

```csharp
PriceListClient client = new PriceListClient();
ListServicesResponse response = await client.ListServicesAsync(new ListServicesRequest());
IEnumerable<string> services = response.Services;
```

### Getting Product Pricing
The index files can be useful for seeing regional or historic data, but the primary use case of the price list API and this SDK is to get pricing information about a specific service. You can do that with the following code:
```csharp
PriceListClientConfig config = new PriceListClientConfig();
PriceListClient client = new PriceListClient(config);

GetProductRequest request = new GetProductRequest("AmazonRDS")
{
    Format = Format.JSON
};

GetProductResponse response = await client.GetProductAsync(request);
ProductOffer rdsOffer = response.ProductOffer;
```

This gets the product data for `AmazonRDS` as JSON and converts it inot a ProductOffer. You can use the ProductOffer structure to evaluate On Demand and Reserved Instance pricing for a product. In the `rdsOffer` object you can explore the different pricing terms, dimensions in each term, and the set of products in that offer. The product data does not contain information about Savings Plans.

### Serde
When the data is requested as JSON, it is automatically deserialized in the response. However, the original `Stream` is still available so you can read the data again. If you acquire just the content stream or have a JSON string, you can you the `FromJsonStream` or `FromJson` methods of the `ProductOffer` class to deserialize the data. Prefer using `FromJsonStream` so you don't convert the stream into a string first. This is faster and more memory efficient.

If you request the content as CSV, it is not deserialized and you will need to access the response's `Content` property to read the data `Stream`.

## Revision History

### 5.0.0 
The AmazonEC2 offer file has grown to be too large to be read as a string directly. The `GetProductInfoAsString` method has been changed to `TryGetResponseDataAsString` and a new method in the `ProductOffer` class was added to deserialize the stream directly, `FromJsonStream`. Added Savings Plans support. All responses that were requested as JSON are automatically deserialized into a corresponding object.

### 4.0.0
Converted all requests to the `PriceListClient` and added base classes for requests and responses.

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
