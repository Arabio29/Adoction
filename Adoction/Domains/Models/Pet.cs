using Adoction.Domains.Enums;

namespace Adoction.Domains.Models;

public class Pet
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Raza { get; set; } = default!;
    public int Age { get; set; }
    public bool Vacunado { get; set; }
    public List<string>? NombreVacunas { get; set; }
    public bool Esterilizado { get; set; }
    public bool CertificadoPedigree { get; set; }

    public Size Size { get; set; }
    public Gender Genero { get; set; }
    public PetStatus Status { get; set; } = PetStatus.Available; // Available | Adopted
    public PetSpecies Species { get; set; } // "Dog" | "Cat"

    public int ShelterId { get; set; }
    public Shelter? Shelter { get; set; }
}
