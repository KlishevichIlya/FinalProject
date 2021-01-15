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
            var currentUser = await _userManager.GetUserAsync(User);
            var items =  _db.Collections.Where(x => x.User == currentUser );
            return View(items);
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
                collection.Id = Guid.NewGuid().ToString();
                collection.User = await _userManager.GetUserAsync(User);
                _db.Collections.Add(collection);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        
        public async Task<IActionResult> Edit(string id)
        {
            var item = await _db.Collections.FindAsync(id);
            if (item != null)
                return View(item);
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id,Collection collection)
        {
            var item = await _db.Collections.FindAsync(id);
            if (item != null)
            {
                item.name = collection.name;
                item.description = collection.description;
                if (collection.formFile != null)
                {
                    if(item.ImageStorageName != null)
                    {
                        await _cloudStorage.DeleteFileAsync(item.ImageStorageName);
                    }
                    await UploadFile(collection);
                    item.formFile = collection.formFile;
                    item.url = collection.url;
                    item.ImageStorageName = collection.ImageStorageName;
                   
                }
                    
                _db.Collections.Update(item);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }



            [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _db.Collections.FindAsync(id);
            if(item != null)
            {
                _db.Collections.Remove(item);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return NotFound();
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
