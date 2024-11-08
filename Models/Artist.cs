using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OtvorenoRacunalstvoLabosi.Models;

public partial class Artist
{
    public int Id { get; set; }

    public int? MonumentId { get; set; }

    public string Name { get; set; } = null!;

    public int? BirthYear { get; set; }

    public int? DeathYear { get; set; }

    public string? Nationality { get; set; }
    [JsonIgnore]
    public virtual Monument? Monument { get; set; }
}
