using Microsoft.AspNetCore.Mvc;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        // GET: api/<TicketController>
        [HttpGet]
        public IEnumerable<Ticket> GetAll()
        {
            using (var context = new LotteryContext())
            {
                return context.Tickets.ToList();
            }
        }

        // GET api/<TicketController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (var context = new LotteryContext())
            {
                var ticket = context.Tickets.FirstOrDefault(t => t.Number == id);
                if (ticket != null)
                {
                    return Ok(ticket);
                }
                else {
                    return NotFound();
                }
            }
        }

        // POST api/<TicketController>
        [HttpPost]
        public IActionResult Post([FromBody] Ticket ticket)
        {
            using (var context = new LotteryContext())
            {
                var existingTicket = context.Tickets.FirstOrDefault(t => t.Number == ticket.Number);
                if (existingTicket != null)
                {
                    existingTicket.ReservedBy = ticket.ReservedBy;
                    existingTicket.IsPaid = ticket.IsPaid;
                    existingTicket.IsDrawn = ticket.IsDrawn;
                    context.SaveChanges();
                    return Ok(existingTicket);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // PUT api/<TicketController>
        [HttpPut]
        public Ticket Put([FromBody] Ticket ticket)
        {
            using (var context = new LotteryContext())
            {
                context.Tickets.Add(ticket);
                context.SaveChanges();
                return ticket;
            }
        }

        // DELETE api/<TicketController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var context = new LotteryContext())
            {
                var ticket = context.Tickets.FirstOrDefault(t => t.Number == id);
                if (ticket != null)
                {
                    context.Tickets.Remove(ticket);
                    context.SaveChanges();
                }
            }
        }
    }
}
