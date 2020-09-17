using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce.Models;
using Ecommerce.Repositories.Interfaces;
using Ecommerce.Repositories;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;
using Ecommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Models.ViewModels;

namespace ECommerce.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IService serviceRepository;
        private readonly ISprovider sproviderRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public ServicesController( IService serviceRepository, ISprovider sproviderRepository, IHostingEnvironment hostingEnvironment)
        {
            this.serviceRepository = serviceRepository;
            this.sproviderRepository = sproviderRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Services
        [Authorize(Policy = "ServiceProvider")]
        public ActionResult Index(int SproviderId)
        {
            var Services = serviceRepository.List().Where(x => x.Sprovider.Id == SproviderId);
            return View(Services);
        }


        public ActionResult CompanyServices(int SproviderId)
        {
            var Services = serviceRepository.List().Where(x => x.Sprovider.Id == SproviderId);
            return View(Services);
        }

        // GET: Services/Details/5
        public ActionResult Details(int id)
        {
            var service = serviceRepository.Find(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        [Authorize(Policy = "ServiceProvider")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServiceProvider")]
        public async Task<ActionResult> Create(ServiceViewModel serviceViewModel)
        {
            if (ModelState.IsValid)
            {
                string UrlImage = "";
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;

                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads/service");
                        if (file.Length > 0)
                        {
                            // var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + file.FileName;
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                UrlImage = fileName;
                            }
                        }
                    }
                }

                var UserId = HttpContext.User.Identity.GetUserId();
                var SproviderId = sproviderRepository.List().Where(x => x.UserId == UserId).FirstOrDefault().Id;
                Service service = new Service
                {
                    Id = serviceViewModel.Id,
                    Image = UrlImage,
                    Name = serviceViewModel.Name,
                    Description = serviceViewModel.Description,
                    Price = serviceViewModel.Price,
                    ExpectedTime = serviceViewModel.ExpectedTime,
                    SproviderId = SproviderId
                };
                serviceRepository.Add(service);

                return RedirectToAction(nameof(Index), new { SproviderId = SproviderId });
            }
            return View();
        }

        [Authorize(Policy = "ServiceProvider")]
        // GET: Services/Edit/5
        public ActionResult Edit(int id)
        {
            var service = serviceRepository.Find(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["SproviderId"] = new SelectList(sproviderRepository.List(), "Id", "CompanyName");
            return View(service);
        }

        [Authorize(Policy = "ServiceProvider")]
        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Service service)
        {
            if (ModelState.IsValid)
            {
                string UrlImage = "";
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;

                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads/service");
                        if (file.Length > 0)
                        {
                            // var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + file.FileName;
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                UrlImage = fileName;
                            }
                        }
                    }
                }
                Service newService = serviceRepository.Find(id);

                if (UrlImage != "")
                {
                    var Directory = Path.Combine(hostingEnvironment.WebRootPath, "uploads/service");
                    var fullPath = Path.Combine(Directory, newService.Image);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    newService.Image = UrlImage;
                }
                newService.Description = service.Description;
                newService.Name = service.Name;
                newService.Price = service.Price;
                newService.ExpectedTime = service.ExpectedTime;

                serviceRepository.Update(id, newService);

                return RedirectToAction(nameof(Index), new { SproviderId = newService.SproviderId });
            }
            ViewData["SproviderId"] = new SelectList(sproviderRepository.List(), "Id", "CompanyName");
            return View(service);
        }


        // GET: Services/Delete/5
        public ActionResult Delete(int id)
        {
            var service = serviceRepository.Find(id);

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var service = serviceRepository.Find(id);
            var Directory = Path.Combine(hostingEnvironment.WebRootPath, "uploads/service");
            var fullPath = Path.Combine(Directory, service.Image);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            serviceRepository.Delete(id);
            return RedirectToAction(nameof(Index), new { SproviderId = service.SproviderId });
        }
    }
}
