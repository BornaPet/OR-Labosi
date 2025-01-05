using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OtvorenoRacunalstvoLabosi.Models;

public partial class Artist
{
    public int Id { get; set; }

    public int? MonumentId { get; set; }

    public string? Name { get; set; } = null!;
    [DisplayName("Birth name")]
    public int? BirthYear { get; set; }
    [DisplayName("Death year")]
    public int? DeathYear { get; set; }

    public string? Nationality { get; set; }
    [JsonIgnore]
    public virtual Monument? Monument { get; set; }
}
