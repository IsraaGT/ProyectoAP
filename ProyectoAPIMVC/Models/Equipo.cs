using System;
using System.Collections.Generic;

namespace ProyectoAPIMVC.Models;

public partial class Equipo
{
    public int Idequipo { get; set; }

    public string NombreEquipo { get; set; } = null!;

    public string País { get; set; } = null!;

    public string Director { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ICollection<Piloto> Pilotos { get; set; } = new List<Piloto>();
}
