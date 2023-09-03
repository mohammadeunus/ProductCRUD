using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductCRUD.CLIENT.DTOs;
using ProductCRUD.CLIENT.Models;

namespace ProductCRUD.CLIENT.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    HttpClient _client = new HttpClient();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            List<ProductModel> products = new List<ProductModel>();

            _client.BaseAddress = new Uri("https://stg-zero.propertyproplus.com.au/api/services/app/ProductSync");
            HttpResponseMessage response = await _client.GetAsync("/GetAllproduct");

            //convert response from api into list of productModel
            if (response.IsSuccessStatusCode) products = JsonConvert.DeserializeObject<List<ProductModel>>(await response.Content.ReadAsStringAsync());

            return View(products);

        }
        catch (Exception ex)
        {
            _logger.LogError("HomeController > Index Error: " + ex.ToString());
            return View(null);
        }
    }

    [HttpGet]
    public IActionResult AddProduct()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(AddProductDTO product)
    {
        try
        {
            _client.BaseAddress = new Uri("https://stg-zero.propertyproplus.com.au/api/services/app/ProductSync");
            HttpResponseMessage response = await _client.PostAsJsonAsync("/CreateOrEdit", product);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }
        catch (Exception ex)
        {
            _logger.LogError("HomeController > AddProduct Error: " + ex.ToString());
            return View(null);
        }
    }
}