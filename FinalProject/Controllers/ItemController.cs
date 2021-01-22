using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class ItemController : Controller
    {
        ApplicationDbContext _db;
        

        public ItemController(ApplicationDbContext db)
        {
            _db = db;
        }
       
        public IActionResult Create(string id)
        {
            ViewBag.Id = id;
            return View();
        }
       
        public async Task<IActionResult> AddItem(string Name, List<string> tags,string id)
        {
            if (Name == null)
                return NotFound();
            else
            {
                 
                Item item = new Item() { Name = Name, CollectionId = id, BitMask = 0};
                await _db.Items.AddAsync(item);
                for(int i=0;i<tags.Count;i++)
                {
                    Tag currentTag = new Tag() { TagName = tags[i], ItemId = item.Id };
                    await _db.Tags.AddAsync(currentTag);                   
                }

                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Item", new { id = id });
        }

        public async Task<ActionResult> Index(string id)
        {
            var items = await _db.Items.Where(x => x.CollectionId == id).ToListAsync();
            ViewBag.Id = id;
            

            return View(items);
        }

      
    }
}
