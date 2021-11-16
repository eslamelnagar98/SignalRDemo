using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRDemo.Data;
using SignalRDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace SignalRDemo.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController:ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;

        public NotificationsController(ApplicationDbContext context, IHubContext<BroadcastHub, IHubClient> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/Notifications/notificationcount  
        [Route("notificationcount")]
        [HttpGet]
        public async Task<ActionResult<NotificationCountResult>> GetNotificationCount()
        {
            var count = (from not in _context.Notifications select not).CountAsync();
            //var count = _context.Notifications.CountAsync();
            NotificationCountResult result = new NotificationCountResult
            {
                Count = await count
            };
            return result;
        }

        // GET: api/Notifications/notificationresult  
        [Route("notificationresult")]
        [HttpGet]
        public async Task<ActionResult<List<NotificationResult>>> GetNotificationMessage()
        {
            return await _context.Notifications.OrderByDescending(n => n.Id)
                                               .Select(n => new NotificationResult
                                               { 
                                                EmployeeName = n.EmployeeName, TranType = n.TranType 
                                               })
                                               .ToListAsync();
                
        }

        // DELETE: api/Notifications/deletenotifications  
        [HttpDelete]
        [Route("deletenotifications")]
        public async Task<IActionResult> DeleteNotifications()
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Notification");
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.BroadcastMessage();

            return NoContent();
        }
    }
}
