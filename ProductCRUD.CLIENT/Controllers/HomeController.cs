using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductCRUD.CLIENT.Models;

namespace ProductCRUD.CLIENT.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    HttpClient _client = new HttpClient();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _client.BaseAddress = new Uri("https://stg-zero.propertyproplus.com.au/api/services/app/ProductSync");
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<ProductModel> products = new List<ProductModel>();

        HttpResponseMessage response = await _client.GetAsync("/GetAllproduct");

        //convert response from api into list of productModel
        if (response.IsSuccessStatusCode) products = JsonConvert.DeserializeObject<List<ProductModel>>(await response.Content.ReadAsStringAsync());

        return View(products);
    }

     

    [HttpPost]
    public async Task<IActionResult> Add(ProductModel product)
    { 
        HttpResponseMessage response = await _client.PostAsJsonAsync("/CreateOrEdit", product);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
            //message= await response.Content.ReadAsStringAsync();
        }
        //ViewBag.message = message;
        return View(product);
    }
}