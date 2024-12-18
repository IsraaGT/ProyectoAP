using System;
using System.Collections.Generic;

namespace ProyectoAPIMVC.Models;

public partial class Usuario
{
    public int Idusuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Contraseña { get; set; }

    public byte Rol { get; set; }
}
