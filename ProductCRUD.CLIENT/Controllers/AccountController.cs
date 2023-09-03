using Microsoft.AspNetCore.Mvc;
using ProductCRUD.CLIENT.DTOs;
using ProductCRUD.CLIENT.Models;

namespace ProductCRUD.CLIENT.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    HttpClient _client = new HttpClient();

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
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
            _client.BaseAddress = new Uri("https://stg-zero.propertyproplus.com.au/api/TokenAuth");
            HttpResponseMessage response = await _client.PostAsJsonAsync("/Authenticate", user);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }
        catch (Exception ex)
        {
            _logger.LogError("HomeController > AddProduct Error: " + ex.ToString());
            return View(null);
        }
    }
}
