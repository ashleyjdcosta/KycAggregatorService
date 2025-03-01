using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using KycAggregatorService.Models;
using KycAggregatorService.Services;

[ApiController]
[Route("api/[controller]")]
public class KycAggregationController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly FileCache _cache;
    private readonly ILogger<KycAggregationController> _logger;

    public KycAggregationController(IHttpClientFactory httpClientFactory, FileCache cache, ILogger<KycAggregationController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _logger = logger;
    }

    [HttpGet("kyc-data/{ssn}")]
    public async Task<IActionResult> GetAggregatedKycData(string ssn)
    {
        try
        {
            //Create the cache key to enable caching for each ssn.
            string cacheKey = $"KycData_{ssn}";
            var cachedData = _cache.Get<AggKycData>(cacheKey);

            if (cachedData != null)
            {
                _logger.LogInformation($"File cache hit for SSN: {ssn}");
                return Ok(cachedData);
            }

            _logger.LogInformation($"File cache miss for SSN: {ssn}, fetching data from APIs");


            var personalDetailsResponse = await GetCustomerPersonalDetailsResponse(ssn);
            if (!personalDetailsResponse.IsSuccessStatusCode)
            {
                return HandleExternalApiResponse(personalDetailsResponse, "Personal details");
            }
            var personalDetails = await personalDetailsResponse.Content.ReadFromJsonAsync<PersonalDetails>();

            var contactDetailsResponse = await GetCustomerContactDetailsResponse(ssn);
            if (!contactDetailsResponse.IsSuccessStatusCode)
            {
                return HandleExternalApiResponse(contactDetailsResponse, "Contact details");
            }
            var contactDetails = await contactDetailsResponse.Content.ReadFromJsonAsync<ContactDetails>();

            var kycFormResponse = await GetKycFormResponse(ssn, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!kycFormResponse.IsSuccessStatusCode)
            {
                return HandleExternalApiResponse(kycFormResponse, "KYC form");
            }
            var kycForm = await kycFormResponse.Content.ReadFromJsonAsync<KycForm>();

            var aggregatedKycData = new AggKycData
            {
                Ssn = ssn,
                FirstName = personalDetails.FirstName,
                LastName = personalDetails.SurName,
                Address = contactDetails.Address != null && contactDetails.Address.Any() ? $"{contactDetails.Address.FirstOrDefault()?.Street}, {contactDetails.Address.FirstOrDefault()?.City}, {contactDetails.Address.FirstOrDefault()?.Country}" : null,
                PhoneNumber = contactDetails.PhoneNumbers != null && contactDetails.PhoneNumbers.Any() ? contactDetails.PhoneNumbers.FirstOrDefault()?.Number : null,
                Email = contactDetails.Emails != null && contactDetails.Emails.Any() ? contactDetails.Emails.FirstOrDefault()?.EmailAddress : null,
                TaxCountry = kycForm.Items.FirstOrDefault(item => item.Key == "tax_country")?.Value,
                Income = int.TryParse(kycForm.Items.FirstOrDefault(item => item.Key == "annual_income")?.Value, out var income) ? income : (int?)null
            };

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
            };
            //Saving to cache memory via FileCache.
            _cache.Set(cacheKey, aggregatedKycData);
            _logger.LogInformation($"Cached data for SSN: {ssn}");

            return Ok(aggregatedKycData);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"HTTP request error for SSN: {ssn}");
            return StatusCode(500, new { error = "Error fetching data from external services." });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, $"JSON parsing error for SSN: {ssn}");
            return StatusCode(500, new { error = "Error parsing data from external services." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unexpected error for SSN: {ssn}");
            return StatusCode(500, new { error = "An unexpected error occurred." });
        }
    }

    private IActionResult HandleExternalApiResponse(HttpResponseMessage response, string resourceName)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound(new { error = $"{resourceName} not found for the provided SSN." });
        }
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            return BadRequest(new { error = $"Invalid SSN format or date for {resourceName}." });
        }
        return StatusCode((int)response.StatusCode, new { error = $"Error fetching {resourceName} from external service." });
    }

    private async Task<HttpResponseMessage> GetCustomerPersonalDetailsResponse(string ssn)
    {
        var client = _httpClientFactory.CreateClient();
        return await client.GetAsync($"https://customerdataapi.azurewebsites.net/api/personal-details/{ssn}");
    }

    private async Task<HttpResponseMessage> GetCustomerContactDetailsResponse(string ssn)
    {
        var client = _httpClientFactory.CreateClient();
        return await client.GetAsync($"https://customerdataapi.azurewebsites.net/api/contact-details/{ssn}");
    }

    private async Task<HttpResponseMessage> GetKycFormResponse(string ssn, string asOfDate)
    {
        var client = _httpClientFactory.CreateClient();
        return await client.GetAsync($"https://customerdataapi.azurewebsites.net/api/kyc-form/{ssn}/{asOfDate}");
    }
}