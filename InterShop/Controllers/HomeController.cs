using InterShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InterShop.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		InterShopContext db;
		List<Product> products;

        public HomeController(ILogger<HomeController> logger, InterShopContext context)
		{
			_logger = logger;

			db=context;
		}

		//default index
		[HttpGet]
		public IActionResult Index()
		{
			ViewBag.Photos=db.photos.ToList();
			ViewBag.Categories=db.categories.ToList();
			products=db.products.ToList();

			return View(products);
		}

		//choose category index
		[HttpPost]
        public IActionResult Index(int id)
        {
            ViewBag.Photos = db.photos.ToList();
            ViewBag.Categories = db.categories.ToList();
			ViewBag.ChooseCategory = db.categories.Where(c=>c.Id==id).First().Title;
            products = db.products.Where(p=>p.CategoryId==id).ToList();

            return View(products);
        }

		//sort index
        [HttpPost]
        public IActionResult IndexSort(string sortString, string CategoryTitle)
        {
            ViewBag.Photos = db.photos.ToList();
            ViewBag.Categories = db.categories.ToList();

			var categoryId=db.categories.Where(c=>c.Title==CategoryTitle).FirstOrDefault()?.Id;

			if (categoryId != null)
			{
                products = db.products.Where(p => p.CategoryId == categoryId).ToList();
			}
			else
			{
                products = db.products.ToList();
            }

			switch (sortString)
			{
				case "title":
					{
                        products = products.OrderBy(p=>p.Title).ToList();
                    }
                    break;

                case "expensive":
                    {
                        products = products.OrderByDescending(p => p.Price).ToList();
                    }
                    break;

                case "chip":
                    {
                        products = products.OrderBy(p => p.Price).ToList();
                    }
                    break;
            }
			return View("Index",products);
			
        }
		
		[HttpPost]
		public IActionResult IndexSearch(string searchString)
		{
            ViewBag.Photos = db.photos.ToList();
            ViewBag.Categories = db.categories.ToList();

            products =db.products.Where(p=>p.Title!.ToLower().Contains(searchString.ToLower())).ToList();

			return View("Index", products);
		}
        public IActionResult Authorization()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}