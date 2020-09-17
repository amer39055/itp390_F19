using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce.Models;
using Ecommerce.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ECommerce.Controllers
{
    public class SprovidersController : Controller
    {
        private readonly ISprovider sproviderRepository;
        private readonly ICategory categoryRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public SprovidersController(ISprovider sproviderRepository, ICategory categoryRepository, IHostingEnvironment hostingEnvironment)
        {
            this.sproviderRepository = sproviderRepository;
            this.categoryRepository = categoryRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Sproviders
        [Authorize(Policy = "Admin_CustomerService")]
        public ActionResult Index()
        {
            var sproviders = sproviderRepository.List();
            return View(sproviders);
        }
        public ActionResult CategoryCompanies(int catId)
        {
            var sProviders = sproviderRepository.List().Where(x => x.CategoryId == catId);
            ViewBag.category = categoryRepository.Find(catId);
            return View(sProviders);
        }

        // GET: Sproviders/Details/5
        [Authorize(Policy = "Admin_CustomerService")]
        public ActionResult Details(int id)
        {
            var sprovider = sproviderRepository.Find(id);
            if (sprovider == null)
            {
                return NotFound();
            }
            return View(sprovider);
        }

        // GET: Sproviders/Edit/5
        [Authorize(Policy = "Admin_CustomerService")]
        public ActionResult Edit(int id)
        {
            Sprovider sprovider = sproviderRepository.Find(id);
            ViewData["CategoryId"] = new SelectList(categoryRepository.List(), "Id", "Name", sprovider.CategoryId);
            //ViewData["UserId"] = new SelectList(categoryRepository.List(), "Id", "Id", sprovider.UserId);
            return View(sprovider);
        }

        // POST: Sproviders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sprovider sprovider)
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

                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads/sprovider");
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
                Sprovider newSprovider = sproviderRepository.Find(id);
                if (UrlImage != "")
                {
                    var Directory = Path.Combine(hostingEnvironment.WebRootPath, "uploads/sprovider");
                    var fullPath = Path.Combine(Directory, newSprovider.Image);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    newSprovider.Image = UrlImage;
                }
                newSprovider.CompanyName = sprovider.CompanyName;
                newSprovider.CategoryId = sprovider.CategoryId;

                sproviderRepository.Update(id, newSprovider);

                return RedirectToAction(nameof(Index));
            }
            return View(sprovider);
        }

        // GET: Sproviders/Delete/5
        public ActionResult Delete(int id)
        {
            var sprovider = sproviderRepository.Find(id);

            return View(sprovider);
        }

        // POST: Sproviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var sprovider = sproviderRepository.Find(id);
                var Directory = Path.Combine(hostingEnvironment.WebRootPath, "uploads/sprovider");
                var fullPath = Path.Combine(Directory, sprovider.Image);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                sproviderRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
