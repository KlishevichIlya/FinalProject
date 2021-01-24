using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        UserManager<User> _userManager;

        public ItemController(ApplicationDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
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
            var currentUser = await _userManager.GetUserAsync(User);
            var currentCollection = await _db.Collections.FindAsync(id);
            bool flag = false;
            if (currentUser.Id == currentCollection.UserId)
                flag = true;
            Response.Cookies.Append("currentCollection", id);
            var items = await _db.Items.Where(x => x.CollectionId == id).ToListAsync();
            ViewBag.Id = id;
            var tags = await _db.Tags.ToListAsync();
            ViewBag.Tags = tags;
            ViewBag.Flag = flag;
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
           
            var item = await _db.Items.FindAsync(id);
            var tags = await _db.Tags.Where(x => x.ItemId == id).ToListAsync();
            if(item != null)
            {
                _db.Items.Remove(item);
                foreach(var tg in tags)
                {
                    _db.Tags.Remove(tg);
                }
               
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Item", new { id = Request.Cookies["currentCollection"] });
            }
            return NotFound();
        }


        public async Task<IActionResult> Edit(string id)
        {
            Response.Cookies.Append("currentItem", id);
            var item = await _db.Items.FindAsync(id);
            var tagsItem = await _db.Tags.Where(x => x.ItemId == item.Id).ToListAsync();
            ViewBag.TagForItem = tagsItem;
            if (item != null)
                return View(item);
            else
                return NotFound();
        }

        public async Task<IActionResult> Update(string Name, List<string> tags)
        {
            string id = Request.Cookies["currentItem"];
            var curItem = await _db.Items.FindAsync(id);
            curItem.Name = Name;
            var oldTags = await _db.Tags.Where(x => x.ItemId == curItem.Id).ToListAsync();
            foreach(var r in oldTags)
            {
                _db.Tags.Remove(r);
            }

            for (int i = 0; i < tags.Count; i++)
            {
                Tag currentTag = new Tag() { TagName = tags[i], ItemId = curItem.Id };
                await _db.Tags.AddAsync(currentTag);
            }
            _db.SaveChanges();
            return RedirectToAction("Index", "Item", new { id = Request.Cookies["currentCollection"] });
        }


        public async Task<IActionResult> Open(string id)
        {
            var item = await _db.Items.FindAsync(id);
            if(item != null)
            {
                var tags = await _db.Tags.Where(x => x.ItemId == item.Id).ToListAsync();
                ViewBag.TagForOpen = tags;
                return View(item);
            }
            return NotFound();
        }
      
    }
}
