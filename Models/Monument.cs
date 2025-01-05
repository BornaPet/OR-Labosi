using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OtvorenoRacunalstvoLabosi.Models;

public partial class Monument
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Type { get; set; } = null!;
    [DisplayName("Year installed")]
    public int? YearInstalled { get; set; }

    public string? Material { get; set; }

    public double? Height { get; set; }
    [DisplayName("Historical significance")]
    public string? HistoricalSignificance { get; set; }

    public int? Popularity { get; set; }

    public string? District { get; set; }

    public virtual Artist Artist { get; set; }
}
