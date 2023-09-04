using Microsoft.AspNetCore.Mvc;
using ProductCRUD.CLIENT.DTOs;
using ProductCRUD.CLIENT.Interfaces;
using ProductCRUD.CLIENT.Repositories; 

namespace ProductCRUD.CLIENT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();
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
                var isSuccess = await _productRepository.AddProductAsync(product);

                if (isSuccess)
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
}
