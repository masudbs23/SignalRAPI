using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SignalRHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IHubContext<NotifyHub, ITypedHubClient> _hubContext;
        public AppContext _context { get; }
        public MessageController(IHubContext<NotifyHub, ITypedHubClient> hubContext, AppContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok("ok");
        }

        [HttpPost("send-message")]
        public IActionResult SendMessage([FromBody]Message msg)
        {
            string retMessage = string.Empty;
            try
            {
                _hubContext.Clients.All.SendMessage(msg.Sender, msg.Payload);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return Ok(retMessage);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Message msg)
        {
            string retMessage = string.Empty;
            try
            {
                Notification notification = new Notification { HasRead = false };
                _context.Add(notification);
                _context.SaveChanges();
                int counter = _context.Notifications.Count(a => a.HasRead == false);
                _hubContext.Clients.All.BroadcastMessage(counter + msg.Payload);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return Ok(retMessage);
        }

        [HttpGet("read/{id}")]
        public IActionResult Read(int id)
        {
            try
            {
                Notification notification = _context.Notifications.FirstOrDefault(a => a.Id == id);
                if(notification != null)
                {
                    notification.HasRead = true;
                    _context.Update(notification);
                    _context.SaveChanges();
                    int counter = _context.Notifications.Count(a => a.HasRead == false);
                    _hubContext.Clients.All.BroadcastMessage(counter + " Notification(s)"); 

                    return Ok(new {Message = "Success"});
                }
                else
                {
                    return NotFound();
                }                
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}