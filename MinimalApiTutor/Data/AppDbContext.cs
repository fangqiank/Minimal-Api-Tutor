using Microsoft.EntityFrameworkCore;
using MinimalApiTutor.Models;

namespace MinimalApiTutor.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {

    }

    public DbSet<Command> commands { get; set; }
}
