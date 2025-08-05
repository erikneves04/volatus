using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Domain.Interfaces.Services;
using Volatus.Domain.View;

namespace Volatus.Domain.Services;

public class EventServices : IEventServices
{
    private readonly IEventRepository _eventRepository;

    public EventServices(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public EventViewModel CreateEvent(string title, string description)
    {
        var eventEntity = new Event
        {
            Title = title,
            Description = description
        };

        var createdEvent = _eventRepository.Insert(eventEntity);
        return ConvertToViewModel(createdEvent);
    }

    public IEnumerable<EventViewModel> GetRecentEvents(int count = 50)
    {
        var events = _eventRepository.GetRecentEvents(count);
        return events.Select(e => ConvertToViewModel(e)).ToList();
    }

    private static EventViewModel ConvertToViewModel(Event eventEntity)
    {
        return new EventViewModel
        {
            Id = eventEntity.Id,
            Title = eventEntity.Title,
            Description = eventEntity.Description,
            CreatedAt = eventEntity.CreatedAt
        };
    }
} 