using Microsoft.EntityFrameworkCore;
using MinimalApiTutor.Data;
using MinimalApiTutor.Models;

namespace MinimalApiTutor.Repository;

public class CommandRepo : ICommandRepo
{
    private readonly AppDbContext _context;

    public CommandRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateCommand(Command command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        await _context.AddAsync(command);
        await _context.SaveChangesAsync();
    }

    public void DeleteCommand(Command cmd)
    {
        if (cmd == null)
            throw new ArgumentNullException(nameof(cmd));

        _context.commands.Remove(cmd);

        _context.SaveChanges();
    }

    public async Task<IEnumerable<Command>> GetAllCommands()
    {
        var results = await _context.commands!.ToListAsync();

        return results;
    }

    public async Task<Command> GetCommandById(int id)
    {
        var result = await _context.commands.FirstOrDefaultAsync(c => c.Id == id);

        return result;
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
