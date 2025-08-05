using Volatus.Domain.Interfaces.Entities;

namespace Volatus.Domain.Entities;

public class Event : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
} 