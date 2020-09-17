using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce.Models;
using Ecommerce.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategory categoryRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public CategoriesController(ICategory categoryRepository, IHostingEnvironment hostingEnvironment)
        {
            this.categoryRepository = categoryRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Policy = "Admin")]
        // GET: Categories
        public ActionResult Index()
        {
            var categories = categoryRepository.List();

            return View(categories);
        }

        public ActionResult Search(string term)
        {
            var categories = categoryRepository.Search(term);
            return View(categories);
        }

        [Authorize(Policy = "Admin")]
        // GET: Categories/Details/5
        public ActionResult Details(int id)
        {
            var category = categoryRepository.Find(id);

            return View(category);
        }

        [Authorize(Policy = "Admin")]
        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Create(CategoryViewModel categoryViewModel)
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

                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads/category");
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
                Category category = new Category
                {
                    Id = categoryViewModel.Id,
                    Image = UrlImage,
                    Name = categoryViewModel.Name,
                    Description = categoryViewModel.Description,
                };
                categoryRepository.Add(category);

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Categories/Edit/5
        [Authorize(Policy = "Admin")]
        public ActionResult Edit(int id)
        {
            var category = categoryRepository.Find(id);
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Edit(int id, Category category)
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

                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads/category");
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

                Category newCategory = categoryRepository.Find(id);

                if (UrlImage != "")
                {
                    var Directory = Path.Combine(hostingEnvironment.WebRootPath, "uploads/category");
                    var fullPath = Path.Combine(Directory, newCategory.Image);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    newCategory.Image = UrlImage;
                }
                newCategory.Description = category.Description;
                newCategory.Name = category.Name;

                categoryRepository.Update(id, newCategory);

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var category = categoryRepository.Find(id);

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = categoryRepository.Find(id);
                var Directory = Path.Combine(hostingEnvironment.WebRootPath, "uploads/category");
                var fullPath = Path.Combine(Directory, category.Image);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                categoryRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
