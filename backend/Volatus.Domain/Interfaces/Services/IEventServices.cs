using Volatus.Domain.Entities;
using Volatus.Domain.View;

namespace Volatus.Domain.Interfaces.Services;

public interface IEventServices
{
    EventViewModel CreateEvent(string title, string description);
    IEnumerable<EventViewModel> GetRecentEvents(int count = 50);
} 