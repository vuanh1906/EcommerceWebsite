using EcommerceWeb.Data;
using EcommerceWeb.Models;
using EcommerceWeb.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace EcommerceWeb.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _dbContext = context;
            _userManager = userManager;
        }
        public IActionResult Index(string? productName, string sortOrder, int? page)
        {

            var products = _dbContext.Products.AsQueryable();
            if (productName != null)
            {
                products = products.Where(p => p.Name.Contains(productName));
            }
            ViewBag.Order = string.IsNullOrEmpty(sortOrder) ? "order" : " ";

            switch (sortOrder)
            {

                case "order":
                    products = _dbContext.Products.AsQueryable();
                    break;
                case "ascending":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "descending":
                    products = products.OrderByDescending(p => p.Price);
                    break;
            }

            return View(products.ToList()/*.ToPagedList(page??1, 3)*/);
        }

        // Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _dbContext.Products
                .Include(p => p.Categories)
                .Include(p => p.ProductTypes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var comments = _dbContext.Comment.Where(c => c.ProductsId == id).ToList();
            
            
            ViewBag.Comments = comments;
            ViewBag.User = _userManager; 

           
            return View(product);
        }

        //POST
        [HttpPost]
        [ActionName("Details")]
        //Add To Cart
        public async Task<IActionResult> ProductsDetails(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }
            var product = await _dbContext.Products.Include(c => c.ProductTypes).FirstOrDefaultAsync(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));

        }
        [ActionName("Remove")]
        public async Task<IActionResult> RemoveFromCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                //var product =  products
                //    .FirstOrDefault(m => m.Id == id);
                var product = products.FirstOrDefault(product => product.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                //var product =  products
                //    .FirstOrDefault(m => m.Id == id);
                var product = products.FirstOrDefault(product => product.Id == id); 
                if (product != null)
                {
                    products.Remove(product);
                     HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            return View(products);
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

        [HttpPost]
        public async Task<IActionResult> addComment(string content, int productId)
        {
            var newComment = new Comment();
         
            DateTime now = DateTime.Now;
            string strDate = now.ToString("YYYY-MM-dd");

            var userId = getUserId();
            var author = _dbContext.ApplicationUsers.FirstOrDefault(a => a.Id == userId);
            if (userId == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            if (ModelState.IsValid)
            {
                newComment.author = author.Email;
                newComment.content =content;
                newComment.CreatedDate= now;
                newComment.ProductsId = productId;
                newComment.ApplicationUserId = userId;


                _dbContext.Comment.Add(newComment);
                await _dbContext.SaveChangesAsync();
            }
         
            return Redirect("/Home/Details/" + productId);

        }

        
        public string getUserId ()
        {
            return _userManager.GetUserId(HttpContext.User);
        }

       
    }
}