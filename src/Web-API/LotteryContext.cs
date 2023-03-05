using Microsoft.EntityFrameworkCore;
using Web_API.Models;

/// <summary>
/// Database context for all Web API database logic.
/// </summary>
public class LotteryContext : DbContext
{
    public LotteryContext()
    {
        Database.EnsureCreated();
    }
    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=mydatabase.db");
    }

    /// <summary>
    /// The tickets used in the current lottery.
    /// </summary>
    public DbSet<Ticket> Tickets { get; set; }

    /// <summary>
    /// The Prizes used in the current lottery.
    /// </summary>
    public DbSet<Prize> Prizes { get; set; }
}
