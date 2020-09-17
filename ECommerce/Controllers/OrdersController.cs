using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Ecommerce.Repositories.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Controllers
{
    public class OrdersController : Controller
    {
       
        private readonly IOrder orderRepository;
        private readonly ISprovider sproviderRepository;

        public OrdersController(IOrder orderRepository, ISprovider sproviderRepository)
        {
            this.orderRepository = orderRepository;
            this.sproviderRepository = sproviderRepository;
        }

        public ActionResult Index()
        {
            var orders = orderRepository.List();
            return View(orders);
        }

        // GET: Orders
        [Authorize(Policy = "Customer")]
        public ActionResult CustomerOrders(string customerId)
        {
            var orders = orderRepository.List().Where(x=>x.CustomerId == customerId);

            return View(orders);
        }

        [Authorize(Policy = "ServiceProvider")]

        public ActionResult ServiceProviderOrders(string sproviderId)
        {
            var orders = orderRepository.List().Where(x => x.Service.Sprovider.User.Id == sproviderId);

            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {          
            var order =  orderRepository.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Policy = "Customer")]
        public ActionResult Create(int myServiceID)
        {
            TempData["ServiceID"] = myServiceID;
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Customer")]
        public ActionResult Create(Order order)
        {
            order.OrderDate = DateTime.Now;
            order.Rating = 0;
            order.RatingNotes = "";
            order.ServiceId = (int)TempData["ServiceID"];
            order.OrderStatus = "Opened";
            order.CustomerId = HttpContext.User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                orderRepository.Add(order);
                return RedirectToAction(nameof(CustomerOrders), new { customerId = order.CustomerId });
            }

            return View();
        }

        // GET: Orders/Edit/5
        //[Authorize(Policy = "ServiceProvider")]
        public ActionResult Edit(int id)
        {      
            var order = orderRepository.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = "ServiceProvider")]
        public ActionResult Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            var oldOrder = orderRepository.Find(id);
            if (order.OrderStatus != null)
            {
                oldOrder.OrderStatus = order.OrderStatus;
                if (ModelState.IsValid)
                {
                    orderRepository.Update(id, oldOrder);
                    return RedirectToAction(nameof(ServiceProviderOrders), new { sproviderId = oldOrder.Service.Sprovider.UserId });
                }
            }
            else if (order.OrderNotes != null)
            {
                oldOrder.OrderNotes = order.OrderNotes;
                if (ModelState.IsValid)
                {
                    orderRepository.Update(id, oldOrder);
                    return RedirectToAction(nameof(CustomerOrders), new { customerId = oldOrder.CustomerId });
                }
            }

       
            return View(order);
        }

        // GET: Orders/Rate/5
        [Authorize(Policy = "Customer")]
        public ActionResult Rate(int id)
        {
            var order = orderRepository.Find(id);
            order.Rating /= 20;

            return View(order);
        }

        // POST: Orders/Rate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Customer")]
        public ActionResult Rate(int id, Order order)
        {

            var oldOrder = orderRepository.Find(id);
      
            oldOrder.Rating = order.Rating*20;
            oldOrder.RatingNotes = order.RatingNotes;
            if (ModelState.IsValid)
            {
                orderRepository.Update(id, oldOrder);
                //Start serviceProvider Rating
                var sprovider = oldOrder.Service.Sprovider;
                var ordersRating = orderRepository.List().Where(s => s.Rating > 0).Select(s => s.Rating);
                sprovider.Rating = 0;
                foreach (var item in ordersRating)
                {
                    sprovider.Rating += item;
                }
                sprovider.Rating = sprovider.Rating / ordersRating.Count();
                sproviderRepository.Update(sprovider.Id, sprovider);
                //End serviceProvider Rating

                return RedirectToAction(nameof(CustomerOrders), new { customerId = oldOrder.CustomerId });
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Policy = "Customer")]
        public ActionResult Delete(int id)
        {
            var order = orderRepository.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Customer")]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = orderRepository.Find(id);
            orderRepository.Delete(id);
            return RedirectToAction(nameof(CustomerOrders));
        }
    }
}
