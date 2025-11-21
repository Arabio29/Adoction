using System.Collections;
using System.Collections.Generic;

namespace Adoction.Domains.Models;

public class Shelter
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
