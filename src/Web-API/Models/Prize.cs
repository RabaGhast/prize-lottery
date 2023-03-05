using System.ComponentModel.DataAnnotations;

namespace Web_API.Models
{
    /// <summary>
    /// Represents a prize for a lottery or raffle.
    /// </summary>

    public class Prize
    {
        /// <summary>
        /// The Id of the prize. Only used internally to uniquely identify it.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the prize.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Full description of the prize.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The monetary cost of the prize.
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// The ticket number which won this price.
        /// </summary>
        /// <remarks>
        /// This property is null if the prize has not been won yet.
        /// </remarks>
        public int? TicketNumber { get; set; }

    }
}
