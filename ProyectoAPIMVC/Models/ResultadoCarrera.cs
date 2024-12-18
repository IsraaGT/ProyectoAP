using System;
using System.Collections.Generic;

namespace ProyectoAPIMVC.Models;

public partial class ResultadoCarrera
{
    public int Idresultado { get; set; }

    public int Idcarrera { get; set; }

    public int Idpiloto { get; set; }

    public byte PosiciónFinal { get; set; }

    public string? TiempoFinal { get; set; }

    public virtual Carrera IdcarreraNavigation { get; set; } = null!;

    public virtual Piloto IdpilotoNavigation { get; set; } = null!;
}
