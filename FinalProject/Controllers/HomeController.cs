using FinalProject.CloudStorage;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        
        ApplicationDbContext _db;
        private readonly ICloudStorage _cloudStorage;
        UserManager<User> _userManager;

        public HomeController(ApplicationDbContext db, ICloudStorage cloudStorage, UserManager<User> userManager)
        {
           
            _db = db;
            _cloudStorage = cloudStorage;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Collections.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Collection collection)
        {
            if (ModelState.IsValid)
            {
                if(collection.formFile != null)
                {
                    await UploadFile(collection);
                }
                collection.id = Guid.NewGuid().ToString();
                collection.User = await _userManager.GetUserAsync(User);
                _db.Collections.Add(collection);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var item = id;
            return LocalRedirect("~/Identity/Account/Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task UploadFile(Collection collect)
        {
            string fileNameForStorage = FormFileName(collect.name, collect.formFile.FileName);
            collect.url = await _cloudStorage.UploadFileAsync(collect.formFile, fileNameForStorage);
            collect.ImageStorageName = fileNameForStorage;
        }

        private static string FormFileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return fileNameForStorage;
        }
    }
}
