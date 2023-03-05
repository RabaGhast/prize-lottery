using System.ComponentModel.DataAnnotations;

namespace Web_API.Models
{
    /// <summary>
    /// Represents a ticket for a lottery or raffle.
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// The number on the ticket, used to uniquely identify it.
        /// </summary>
        [Key]
        public int Number { get; set; }

        /// <summary>
        /// The name of the person who reserved the ticket.
        /// </summary>
        public string? ReservedBy { get; set; } = null;

        /// <summary>
        ///  A value indicating whether the ticket has been paid for.
        /// </summary>
        public bool IsPaid { get; set; } = false;

        /// <summary>
        ///  A value indicating whether the ticket has been drawn as a winner.
        /// </summary>
        public bool IsDrawn { get; set; } = false;
    }
}
