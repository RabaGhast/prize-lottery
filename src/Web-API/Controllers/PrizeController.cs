using Microsoft.AspNetCore.Mvc;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrizeController : ControllerBase
    {
        // GET: api/<PrizeController>
        [HttpGet]
        public IActionResult GetAll()
        {
            using (var context = new LotteryContext())
            {
                var tickets = context.Tickets.ToList();
                var prizes = context.Prizes.ToList(); 
                return Ok(context.Prizes.ToList());
            }
        }

        // GET api/<PrizeController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (var context = new LotteryContext())
            {
                var prize = context.Prizes.FirstOrDefault(t => t.Id == id);
                if (prize != null)
                {
                    return Ok(prize);
                }
                else {
                    return NotFound();
                }
            }
        }

        // POST api/<PrizeController>
        [HttpPost]
        public IActionResult Post([FromBody] Prize prize)
        {
            using (var context = new LotteryContext())
            {
                var existingPrize = context.Prizes.FirstOrDefault(p => p.Id == prize.Id);
                if (existingPrize != null)
                {
                    existingPrize.Name = prize.Name;
                    existingPrize.Description = prize.Description;
                    existingPrize.Cost = prize.Cost;
                    existingPrize.TicketNumber = prize.TicketNumber;
                    context.SaveChanges();
                    return Ok(existingPrize);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // PUT api/<PrizeController>
        [HttpPut]
        public IActionResult Put([FromBody] Prize prize)
        {
            using (var context = new LotteryContext())
            {
                context.Prizes.Add(prize);
                context.SaveChanges();
                return Ok(prize);
            }
        }

        // DELETE api/<PrizeController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var context = new LotteryContext())
            {
                var prize = context.Prizes.FirstOrDefault(p => p.Id == id);
                if (prize != null)
                {
                    context.Prizes.Remove(prize);
                    context.SaveChanges();
                    return Ok(prize);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // POST api/<PrizeController>/initialize
        [HttpPost("initialize")]
        public IActionResult Initialize()
        {
            using (var context = new LotteryContext())
            {
                // Remove all existing prizes from the database.
                context.Prizes.RemoveRange(context.Prizes);
                context.SaveChanges();

                return Ok(context.Prizes.ToList());
            }
        }

        // GET api/<PrizeController>/draw
        [HttpGet("draw")]
        public IActionResult Draw([FromQuery] int ticketNumber)
        {
            using (var context = new LotteryContext())
            {
                // Get a the cheapest not already drawn prize from the database.
                var relevantPrizes = context.Prizes.Where(p => !p.TicketNumber.HasValue).ToList();
                if (!relevantPrizes.Any()) return NotFound();
                var selectedPrize = relevantPrizes.OrderBy(p => p.Cost).FirstOrDefault();

                if (selectedPrize != null)
                {
                    // Set the prize as drawn.
                    selectedPrize.TicketNumber = ticketNumber;
                    context.SaveChanges();

                    return Ok(selectedPrize);
                }
                else
                {
                    return NotFound();
                }
            }
        }



    }
}
