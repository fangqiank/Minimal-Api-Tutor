using MinimalApiTutor.Models;

namespace MinimalApiTutor.Repository;
public interface ICommandRepo
{
    Task CreateCommand(Command command);
    void DeleteCommand(Command cmd);
    Task<IEnumerable<Command>> GetAllCommands();
    Task<Command> GetCommandById(int id);
    Task SaveChanges();
}
