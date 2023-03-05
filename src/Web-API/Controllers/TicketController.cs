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
        public IActionResult GetAll()
        {
            using (var context = new LotteryContext())
            {
                return Ok(context.Tickets.ToList());
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
        public IActionResult Put([FromBody] Ticket ticket)
        {
            using (var context = new LotteryContext())
            {
                context.Tickets.Add(ticket);
                context.SaveChanges();
                return Ok(ticket);
            }
        }

        // DELETE api/<TicketController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var context = new LotteryContext())
            {
                var ticket = context.Tickets.FirstOrDefault(t => t.Number == id);
                if (ticket != null)
                {
                    context.Tickets.Remove(ticket);
                    context.SaveChanges();
                    return Ok(ticket);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // POST api/<TicketController>/5/reserve
        [HttpPost("{id}/reserve")]
        public IActionResult Reserve(int id, [FromQuery] string user)
        {
            using (var context = new LotteryContext())
            {
                var ticket = context.Tickets.FirstOrDefault(t => t.Number == id);
                if (ticket != null)
                {
                    if(ticket.ReservedBy != null)
                    {
                        return BadRequest("Ticket is already reserved!");
                    }
                    ticket.ReservedBy = user;
                    context.SaveChanges();
                    return Ok(ticket);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // POST api/<TicketController>/pay
        [HttpPost("pay")]
        public IActionResult Pay([FromQuery] string user)
        {
            using (var context = new LotteryContext())
            {
                var tickets = context.Tickets.Where(t => t.ReservedBy == user).ToList();
                if (tickets.Any())
                {
                    foreach (var ticket in tickets)
                    {
                        ticket.IsPaid = true;
                    }
                    context.SaveChanges();
                    return Ok(tickets);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // POST api/<TicketController>/initialize
        [HttpPost("initialize")]
        public IActionResult Initialize([FromQuery] int numberOfTickets)
        {
            using (var context = new LotteryContext())
            {
                // Remove all existing tickets from the database.
                context.Tickets.RemoveRange(context.Tickets);
                context.SaveChanges();

                // Generate the specified number of new blank tickets.
                for (int i = 1; i <= numberOfTickets; i++)
                {
                    var ticket = new Ticket
                    {
                        Number = i,
                        ReservedBy = null,
                        IsPaid = false,
                        IsDrawn = false
                    };
                    context.Tickets.Add(ticket);
                }
                context.SaveChanges();

                return Ok(context.Tickets.ToList());
            }
        }

        // GET api/<TicketController>/draw
        [HttpGet("draw")]
        public IActionResult Draw()
        {
            using (var context = new LotteryContext())
            {
                // Get a random paid and not drawn ticket from the database.
                var random = new Random();
                var relevantTickets = context.Tickets.Where(t => t.IsPaid && !t.IsDrawn).ToList();
                if (!relevantTickets.Any()) return NotFound();
                var randomTicket = relevantTickets[random.Next(relevantTickets.Count())];

                if (randomTicket != null)
                {
                    // Set the ticket as drawn.
                    randomTicket.IsDrawn = true;
                    context.SaveChanges();

                    return Ok(randomTicket);
                }
                else
                {
                    return NotFound();
                }
            }
        }



    }
}
