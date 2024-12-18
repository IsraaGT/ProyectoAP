using System;
using System.Collections.Generic;

namespace ProyectoAPIMVC.Models;

public partial class Carrera
{
    public int Idcarrera { get; set; }

    public string NombreCarrera { get; set; } = null!;

    public string Ubicación { get; set; } = null!;

    public DateOnly FechaCarrera { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<ResultadoCarrera> ResultadoCarreras { get; set; } = new List<ResultadoCarrera>();
}
