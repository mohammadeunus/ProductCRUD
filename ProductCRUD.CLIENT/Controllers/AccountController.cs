using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductCRUD.CLIENT.Models;

namespace ProductCRUD.CLIENT.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly HttpClient _client;

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
        _client = new HttpClient();
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
      
    [HttpPost]
    public async Task<IActionResult> Login(UserModel user)
    {
        try
        {
            // Set the base address for the API
            _client.BaseAddress = new Uri("https://stg-zero.propertyproplus.com.au");

            // Set the tenant ID in the header
            _client.DefaultRequestHeaders.Add("Abp.TenantId", "10");

            // Serialize the user object to JSON and send it as the request body
            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/TokenAuth/Authenticate", user);


            // Read the response content as a string
            string responseContent = await response.Content.ReadAsStringAsync();

            // Parse the JSON response to extract error details
            var responseContentJson = JsonConvert.DeserializeObject<dynamic>(responseContent);

            // Check if the response is successful (status code 200)
            if (!response.IsSuccessStatusCode || (int)response.StatusCode >= 400)
            {  
                string errorDetails = responseContentJson?.error?.details;
                 
                ViewBag.ErrorDetails = errorDetails;

                return View();
            }

            // Set the response content in ViewBag
            ViewBag.ResponseContent = responseContent;

            return RedirectToAction("Index", "Home");

        }
        catch (Exception ex)
        {
            _logger.LogError("AccountController > Login Error: " + ex.ToString());
            return View(null);
        }
    }
}
