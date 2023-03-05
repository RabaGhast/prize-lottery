using Microsoft.AspNetCore.Mvc;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private static List<Ticket> DummyData = new List<Ticket>
        {
            new Ticket // Not reserved
            {
                Number = 1,
                ReservedBy = null,
                IsDrawn = false,
                IsPaid = false
            },
            new Ticket // Reserverd
            {
                Number = 2,
                ReservedBy = "Fabian",
                IsDrawn = true,
                IsPaid = false
            },
            new Ticket // Reserved and paid
            {
                Number = 3,
                ReservedBy = "Fabian",
                IsDrawn = true,
                IsPaid = true
            }
        };

        // GET: api/<TicketController>
        [HttpGet]
        public IEnumerable<Ticket> Get()
        {
            return DummyData;
        }

        // GET api/<TicketController>/5
        [HttpGet("{id}")]
        public Ticket? Get(int id)
        {
            return DummyData.FirstOrDefault(t => t.Number == id);
        }

        // POST api/<TicketController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TicketController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TicketController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
