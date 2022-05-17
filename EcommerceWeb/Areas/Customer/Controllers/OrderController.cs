using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceWeb.Data;
using EcommerceWeb.Models;
using EcommerceWeb.Utility;

namespace EcommerceWeb.Areas.Customer.Controllers

{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer/Orders
        public async Task<IActionResult> Checkout()
        {
            return View();
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            //lay product trong cart tu session
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");

            //tao cac orderdetail trong 1 order
            if (products != null)
            {
                // 1 product = 1 orderdetail
                foreach (var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = product.Id;

                    //luu vao order
                    order.OrderDetails.Add(orderDetails);
                }
            }
            //them stt order
            DateTime now = DateTime.Now;
            order.OrderDate = now;
            order.OrderNo = GetOrderNo();

            //save vao context
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Set("products", new List<Products>());
            return View();
        }
        public string GetOrderNo()
        {
            int rowCount = _context.Order.ToList().Count() + 1;
            return rowCount.ToString("000");
        }
    }
}

