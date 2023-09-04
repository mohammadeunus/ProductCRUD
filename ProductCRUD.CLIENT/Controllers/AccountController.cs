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

            // Parse the JSON response to object
            var responseContentJson = JsonConvert.DeserializeObject<dynamic>(responseContent);
            var isCookieSaved = IsCookieCreated(responseContentJson, user.userNameOrEmailAddress);

            // Check if the response is successful (status code 200)
            if (!response.IsSuccessStatusCode || (int)response.StatusCode >= 400)
            {
                string errorDetails = responseContentJson?.error?.details;

                ViewBag.ErrorDetails = errorDetails;

                return View();
            }


            return RedirectToAction("Index", "Home");

        }
        catch (Exception ex)
        {
            _logger.LogError("AccountController > Login Error: " + ex.ToString());
            return View(null);
        }
    }

    public IActionResult LogOut()
    {
        try
        {
            var userNameOrEmailAddress = ReadCookie("userName");
            if (userNameOrEmailAddress == null) return View("LogIn");
            string accessToken = string.Empty;

            // Store the expireInSeconds in a cookie
            CookieOptions expireOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddSeconds(-10),
            };

            Response.Cookies.Append(userNameOrEmailAddress, accessToken, expireOptions);
            Response.Cookies.Append("userName", accessToken, expireOptions);
              
            return View("LogIn");

        }
        catch (Exception ex)
        {
            _logger.LogError("AccountController > LogOut > Error: " + ex.ToString());
            return View("LogIn");
        }
    }

    public bool IsCookieCreated(object responseContentJson, string userNameOrEmailAddress)
    {
        try
        {
            if (responseContentJson == null) return false;

            // Deserialize the JSON string to get the expireInSeconds and accessToken values
            var jsonResponse = JsonConvert.DeserializeAnonymousType(responseContentJson.ToString(), new { result = new { accessToken = "", expireInSeconds = 0 } });
            int expireInSeconds = jsonResponse.result.expireInSeconds;
            string accessToken = jsonResponse.result.accessToken;

            // Store the expireInSeconds in a cookie
            CookieOptions expireOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddSeconds(expireInSeconds),
            };

            Response.Cookies.Append(userNameOrEmailAddress, accessToken, expireOptions);
            Response.Cookies.Append("userName", userNameOrEmailAddress, expireOptions);

            var res = ReadCookie(userNameOrEmailAddress);

            return true;

        }
        catch (Exception ex)
        {
            _logger.LogError("AccountController > IsCookieCreated > Error: " + ex.ToString());
            return false;
        }

    }
    public string ReadCookie(string key)
    { 
        string cookieValue = Request.Cookies[key];
        return cookieValue;
    }
}
