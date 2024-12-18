using System;
using System.Collections.Generic;

namespace ProyectoAPIMVC.Models;

public partial class Piloto
{
    public int Idpiloto { get; set; }

    public string NombrePiloto { get; set; } = null!;

    public int Idequipo { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public string Nacionalidad { get; set; } = null!;

    public virtual Equipo IdequipoNavigation { get; set; } = null!;

    public virtual ICollection<ResultadoCarrera> ResultadoCarreras { get; set; } = new List<ResultadoCarrera>();
}
