using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

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

        public async Task<ActionResult> Index(string id,string[] itemid)
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
            ViewBag.Items = new SelectList(items,"Id","Name");

            if (itemid.Length != 0)
            {
                var selectedItem = await _db.Items.Where(x => x.Id == itemid[0]).ToListAsync(); //sort item 
                return View(selectedItem);
            }
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
            var bitmask = item.BitMask;
            for(int i = 0; i <= 14; i++)
            {

                if ((bitmask & (1 << i)) != 0)
                {
                    if (i == 0)
                    {
                        ViewBag.FInt1_Name = item.FInt1_Name;
                        ViewBag.FInt1 = item.FInt1;
                    }
                    if (i == 1)
                    {
                        ViewBag.FInt2_Name = item.FInt2_Name;
                        ViewBag.FInt2 = item.FInt2;
                    }
                    if (i == 2)
                    {
                        ViewBag.FInt3_Name = item.FInt3_Name;
                        ViewBag.FInt3 = item.FInt3;
                    }
                    ////////
                    if (i == 3)
                    {
                        ViewBag.FStr1_Name = item.FStr1_Name;
                        ViewBag.FStr1 = item.FStr1;
                    }
                    if (i == 4)
                    {
                        ViewBag.FStr2_Name = item.FInt2_Name;
                        ViewBag.FStr2 = item.FStr2;
                    }
                    if (i == 5)
                    {
                        ViewBag.FStr3_Name = item.FInt2_Name;
                        ViewBag.FStr3 = item.FStr3;
                    }
                    //////////
                    if (i == 6)
                    {
                        ViewBag.FText1_Name = item.FText1_Name;
                        ViewBag.FText1 = item.FText1;
                    }
                    if (i == 7)
                    {
                        ViewBag.FText2_Name = item.FText2_Name;
                        ViewBag.FText2 = item.FText2;
                    }
                    if (i == 8)
                    {
                        ViewBag.FText3_Name = item.FText3_Name;
                        ViewBag.FText3 = item.FText3;
                    }
                    //////////
                    if (i == 9)
                    {
                        ViewBag.FDate1_Name = item.FDate1_Name;
                        ViewBag.FDate1 = item.FDate1;
                    }
                    if (i == 10)
                    {
                        ViewBag.FDate2_Name = item.FDate2_Name;
                        ViewBag.FDate2 = item.FDate2;
                    }
                    if (i == 11)
                    {
                        ViewBag.FDate3_Name = item.FDate3_Name;
                        ViewBag.FDate3 = item.FDate3;
                    }
                    ////////////
                    if (i == 12)
                    {
                        ViewBag.FChBox1_Name = item.FChBox1_Name;
                        ViewBag.FChBox1 = item.FChBox1;
                    }
                    if (i == 13)
                    {
                        ViewBag.FChBox2_Name = item.FChBox2_Name;
                        ViewBag.FChBox2 = item.FChBox2;
                    }
                    if (i == 14)
                    {
                        ViewBag.FChBox3_Name = item.FChBox3_Name;
                        ViewBag.FChBox3 = item.FChBox3;
                    }
                    /////////////
                }
            }

            if (item != null)
                return View(item);
            else
                return NotFound();
        }

        public async Task<IActionResult> Update(string Name, List<string> tags,string FI1,string FI2,string FI3, string S1, 
            string S2, string S3, string T1, string T2, string T3, DateTime D1, bool FChBox1,bool FChBox2,bool FChBox3)
        {
            string id = Request.Cookies["currentItem"];
            var curItem = await _db.Items.FindAsync(id);
            curItem.Name = Name;
            if (FI1 != null)
                curItem.FInt1 = Convert.ToInt32(FI1);
            if (FI2 != null)
                curItem.FInt2 = Convert.ToInt32(FI2);
            if (FI3 != null)
                curItem.FInt3 = Convert.ToInt32(FI3);
            if (S1 != null)
                curItem.FStr1 = S1;
            if (S2 != null)
                curItem.FStr2 = S2;
            if (S3 != null)
                curItem.FStr3 = S3;
            if (T1 != null)
                curItem.FText1 = T1;
            if (T2 != null)
                curItem.FText1 = T2;
            if (T3 != null)
                curItem.FText1 = T3;
            if(D1 != null)
            {
                curItem.FDate1 = D1;
            }
            curItem.FChBox1 = FChBox1;
            curItem.FChBox2 = FChBox2;
            curItem.FChBox3 = FChBox3;
            var oldTags = await _db.Tags.Where(x => x.ItemId == curItem.Id).ToListAsync();
            foreach (var r in oldTags)
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
            var currentUser = await _userManager.GetUserAsync(User);
            //var currentIdItem = Request.Cookies["currentCollection"];

            ViewBag.Id = id;
            var item = await _db.Items.FindAsync(id);
            if(item != null)
            {
                var tags = await _db.Tags.Where(x => x.ItemId == item.Id).ToListAsync();
                var like = _db.Likes.FirstOrDefault(x => x.UserId == currentUser.Id);
                var bitmask = item.BitMask;
                for(int i = 0; i <= 14; i++)
                {
                    if((bitmask & (1 << i)) != 0)
                    {
                        if(i == 0)
                        {
                            ViewBag.FInt1_Name = item.FInt1_Name;
                            ViewBag.FInt1 = item.FInt1;
                        }
                        if (i == 1)
                        {
                            ViewBag.FInt2_Name = item.FInt2_Name;
                            ViewBag.FInt2 = item.FInt2;
                        }
                        if (i == 2)
                        {
                            ViewBag.FInt3_Name = item.FInt3_Name;
                            ViewBag.FInt3 = item.FInt3;
                        }
                        ////////
                        if (i == 3)
                        {
                            ViewBag.FStr1_Name = item.FStr1_Name;
                            ViewBag.FStr1 = item.FStr1;
                        }
                        if (i == 4)
                        {
                            ViewBag.FStr2_Name = item.FInt2_Name;
                            ViewBag.FStr2 = item.FStr2;
                        }
                        if (i == 5)
                        {
                            ViewBag.FStr3_Name = item.FInt2_Name;
                            ViewBag.FStr3 = item.FStr3;
                        }
                        //////////
                        if (i == 6)
                        {
                            ViewBag.FText1_Name = item.FText1_Name;
                            ViewBag.FText1 = item.FText1;
                        }
                        if (i == 7)
                        {
                            ViewBag.FText2_Name = item.FText2_Name;
                            ViewBag.FText2 = item.FText2;
                        }
                        if (i == 8)
                        {
                            ViewBag.FText3_Name = item.FText3_Name;
                            ViewBag.FText3 = item.FText3;
                        }
                        //////////
                        if (i == 9)
                        {
                            ViewBag.FDate1_Name = item.FDate1_Name;
                            ViewBag.FDate1 = item.FDate1;
                        }
                        if (i == 10)
                        {
                            ViewBag.FDate2_Name = item.FDate2_Name;
                            ViewBag.FDate2 = item.FDate2;
                        }
                        if (i == 11)
                        {
                            ViewBag.FDate3_Name = item.FDate3_Name;
                            ViewBag.FDate3 = item.FDate3;
                        }
                        ////////////
                        if (i == 12)
                        {
                            ViewBag.FChBox1_Name = item.FChBox1_Name;
                            ViewBag.FChBox1 = item.FChBox1;
                        }
                        if (i == 13)
                        {
                            ViewBag.FChBox2_Name = item.FChBox2_Name;
                            ViewBag.FChBox2 = item.FChBox2;
                        }
                        if (i == 14)
                        {
                            ViewBag.FChBox3_Name = item.FChBox3_Name;
                            ViewBag.FChBox3 = item.FChBox3;
                        }
                        /////////////
                    }
                }
                ViewBag.TagForOpen = tags;
                
                // ViewBag.isLike = like.isLike;
                if (like == null || like.isLike == false || like.ItemId != id)
                    ViewBag.isLike = false;
                else
                    ViewBag.isLike = true;
                return View(item);
            }
            return NotFound();
        }
      
        public async Task<IActionResult> Like(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentLike =  _db.Likes.FirstOrDefault(x => x.UserId == currentUser.Id);
           
            if (currentLike == null || currentLike.ItemId != id)
            {
                Like like = new Like() { isLike = true, ItemId = id, UserId = currentUser.Id };
                await _db.Likes.AddAsync(like);
            }
            else
            {
                bool fl = false;
                if (currentLike.isLike == true && fl == false)
                {
                    currentLike.isLike = false;
                    fl = true;                    
                }
                    
                if (currentLike.isLike == false && fl == false)
                {
                    currentLike.isLike = true;
                    fl = true;
                }

                _db.Likes.Update(currentLike);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Open", "Item", new { id = id });
        }



        public async Task<IActionResult> AddField(string typeField, string nameField, string valueField)
        {
            var currentCollection = Request.Cookies["currentCollection"];
            var listItems = await _db.Items.Where(x => x.CollectionId == currentCollection).ToListAsync();
            foreach(var item in listItems)
            {
                if(typeField == "0")
                {
                    var bitMask = item.BitMask;    
                    for(int i=0; i <= 2; i++)
                    {
                        if((bitMask & (1 << i)) == 0)
                        {
                            if(i == 0)
                            {
                                bitMask = bitMask | (1 << 0);
                                item.FInt1 = Convert.ToInt32(valueField);
                                item.FInt1_Name = nameField;
                                break;
                            }
                            if (i == 1)
                            {
                                bitMask = bitMask | (1 << 1);
                                item.FInt2 = Convert.ToInt32(valueField);
                                item.FInt2_Name = nameField;
                                break;
                            }
                            if (i == 2)
                            {
                                bitMask = bitMask | (1 << 2);
                                item.FInt3 = Convert.ToInt32(valueField);
                                item.FInt3_Name = nameField;
                                break;
                            }
                            
                        }
                    }
                    item.BitMask = bitMask;
                    await _db.SaveChangesAsync();
                }

                if(typeField == "1")
                {
                    var bitMask = item.BitMask;
                    for (int i = 3; i <= 5; i++)
                    {
                        if ((bitMask & (1 << i)) == 0)
                        {
                            if (i == 3)
                            {
                                bitMask = bitMask | (1 << 3);
                                item.FStr1 = valueField;
                                item.FStr1_Name = nameField;
                                break;
                            }
                            if (i == 4)
                            {
                                bitMask = bitMask | (1 << 4);
                                item.FStr2 = valueField;
                                item.FStr2_Name = nameField;
                                break;
                            }
                            if (i == 5)
                            {
                                bitMask = bitMask | (1 << 5);
                                item.FStr3 = valueField;
                                item.FStr3_Name = nameField;
                                break;
                            }

                        }
                    }
                    item.BitMask = bitMask;
                    await _db.SaveChangesAsync();
                }


                if (typeField == "2")
                {
                    var bitMask = item.BitMask;
                    for (int i = 6; i <= 8; i++)
                    {
                        if ((bitMask & (1 << i)) == 0)
                        {
                            if (i == 6)
                            {
                                bitMask = bitMask | (1 << 6);
                                item.FText1 = valueField;
                                item.FText1_Name = nameField;
                                break;
                            }
                            if (i == 7)
                            {
                                bitMask = bitMask | (1 << 7);
                                item.FText2 = valueField;
                                item.FText2_Name = nameField;
                                break;
                            }
                            if (i == 8)
                            {
                                bitMask = bitMask | (1 << 8);
                                item.FText3 = valueField;
                                item.FText3_Name = nameField;
                                break;
                            }

                        }
                    }
                    item.BitMask = bitMask;
                    await _db.SaveChangesAsync();
                }

                if (typeField == "3")
                {
                    var bitMask = item.BitMask;
                    for (int i = 9; i <= 11; i++)
                    {
                        if ((bitMask & (1 << i)) == 0)
                        {
                            if (i == 9)
                            {
                                bitMask = bitMask | (1 << 9);
                                item.FDate1 = DateTime.Now;
                                item.FDate1_Name = nameField;
                                break;
                            }
                            if (i == 10)
                            {
                                bitMask = bitMask | (1 << 10);
                                item.FDate2 = DateTime.Now;
                                item.FDate2_Name = nameField;
                                break;
                            }
                            if (i == 11)
                            {
                                bitMask = bitMask | (1 << 11);
                                item.FDate3 = DateTime.Now;
                                item.FDate3_Name = nameField;
                                break;
                            }

                        }
                    }
                    item.BitMask = bitMask;
                    await _db.SaveChangesAsync();
                }


                if (typeField == "4")
                {
                    var bitMask = item.BitMask;
                    for (int i = 12; i <= 14; i++)
                    {
                        if ((bitMask & (1 << i)) == 0)
                        {
                            if (i == 12)
                            {
                                bitMask = bitMask | (1 << 12);
                                item.FChBox1 = valueField == null ? false : true;
                                item.FChBox1_Name = nameField;
                                break;
                            }
                            if (i == 13)
                            {
                                bitMask = bitMask | (1 << 13);
                                item.FChBox2 = valueField == null ? false : true;
                                item.FChBox2_Name = nameField;
                                break;
                            }
                            if (i == 14)
                            {
                                bitMask = bitMask | (1 << 14);
                                item.FChBox3 = valueField == null ? false : true;
                                item.FChBox3_Name = nameField;
                                break;
                            }

                        }
                    }
                    item.BitMask = bitMask;
                    await _db.SaveChangesAsync();
                }



            }

            
            return RedirectToAction("Index", "Item", new { id = currentCollection });
        }


        //public async void Comment(string id, string message)
        //{
        //   var currentUser = await _userManager.GetUserAsync(User);
        //   if(id != null && message != null)
        //    {
        //        Comment comment = new Comment() { ItemId = id, Text = message, UserId = currentUser.Id };
        //        _db.Comments.Add(comment);
        //    }

        //    await _db.SaveChangesAsync();

        //   // return NotFound();
        //}


        ////////////////BITMASK/////////////////////////////

        //if(((bitMask & (1 << 0)) == 0) && ((bitMask & (1 << 1)) == 0))
        //{
        //    var newMask = bitMask | (1 << 0);
        //    newMask = newMask | (1 << 1);
        //    for(int i = 0; i <= 29; i++)
        //    {
        //        if((newMask & (1 << i)) != 0)
        //        {
        //            if(i == 0)
        //            {
        //                item.FInt1 = Convert.ToInt32(valueField);
        //            }
        //            if(i == 1)
        //            {
        //                item.FInt1_Name = nameField;                               
        //            }
        //            item.BitMask = newMask;
        //            await _db.SaveChangesAsync();
        //        }

        //    }
        //}
        //if (((bitMask & (1 << 2)) == 0) && ((bitMask & (1 << 3)) == 0))
        //{
        //    var newMask = bitMask | (1 << 2);
        //    newMask = newMask | (1 << 3);
        //    for (int i = 0; i <= 29; i++)
        //    {
        //        if ((newMask & (1 << i)) != 0)
        //        {
        //            if (i == 2)
        //            {
        //                item.FInt2 = Convert.ToInt32(valueField);
        //            }
        //            if (i == 3)
        //            {
        //                item.FInt2_Name = nameField;
        //            }
        //            item.BitMask = newMask;
        //            await _db.SaveChangesAsync();
        //        }

        //    }
        //}

    }
}
