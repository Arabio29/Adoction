using static Adoction.Domains.Enums;

namespace Adoction.Domains.Models;

public class Pet
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Raza { get; set; } = default!;
    public int Age { get; set; }
    public bool Vacunado { get; set; } = default!;
    public List<string>? NombreVacunas { get; set; } 
    public bool Esterilizado { get; set; } = default!;
    public bool CertificadoPedigree { get; set; } = default!;
    
    public Size Size { get; set; } = default!;
    public Gender Genero { get; set; } = default!;
    public PetStatus Status { get; set; } = PetStatus.Available; // Available | Adopted
    public PetSpecies Species { get; set; } = default!; // "Dog" | "Cat"

    public int ShelterId { get; set; }
    public Shelter Shelter { get; set; } = default!;
}
