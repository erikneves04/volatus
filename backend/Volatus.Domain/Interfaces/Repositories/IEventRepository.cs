using Volatus.Domain.Entities;

namespace Volatus.Domain.Interfaces.Repositories;

public interface IEventRepository : IRepository<Event>
{
    IEnumerable<Event> GetRecentEvents(int count = 50);
} 