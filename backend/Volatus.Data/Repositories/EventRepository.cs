using Microsoft.EntityFrameworkCore;
using Volatus.Data.Context;
using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;

namespace Volatus.Data.Repositories;

public class EventRepository : Repository<Event>, IEventRepository
{
    public EventRepository(AppDbContext context) : base(context)
    {
    }

    public IEnumerable<Event> GetRecentEvents(int count = 50)
    {
        return _context.Set<Event>()
            .OrderByDescending(e => e.CreatedAt)
            .Take(count)
            .ToList();
    }
} 