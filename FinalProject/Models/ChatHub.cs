
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        ApplicationDbContext _db;


        public ChatHub(UserManager<User> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [Authorize]
        public async Task Send(string nick, string message,string id)
        {
            //var allCom = await _db.Comments.Where(x => x.ItemId == id).ToListAsync();
            //HashSet<string> users = new HashSet<string>();
            //foreach (var com in allCom)
            //{
            //    users.Add(com.UserId);
            //}
            //users.Add(Context.UserIdentifier);

            //await this.Clients.All.SendAsync("Send", message, nick);
            await this.Clients.Group(id).SendAsync("Send", message, nick);
            Comment comment = new Comment() { Text = message, ItemId = id, Nick = nick, UserId = Context.UserIdentifier };
            _db.Comments.Add(comment);           
            await _db.SaveChangesAsync();
        }


        public override async Task OnConnectedAsync()
        {
            var itemId = Context.GetHttpContext().Request.Cookies["curIt"];
            await Groups.AddToGroupAsync(Context.ConnectionId, itemId);
            var comment = await _db.Comments.ToListAsync();
            await Clients.Caller.SendAsync("Notify", comment);
            await base.OnConnectedAsync();
        }
    }
}
