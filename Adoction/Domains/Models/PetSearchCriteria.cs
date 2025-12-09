using Adoction.Domains.Enums;

namespace Adoction.Domains.Models;

public record PetSearchCriteria
{
    public Enums.PetStatus? Status { get; init; }
    public Enums.PetSpecies? Species { get; init; }
    public Enums.Gender? Gender { get; init; }
    public Enums.Size? Size { get; init; }
}
