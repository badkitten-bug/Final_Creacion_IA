using System;
using System.Collections.Generic;

namespace CrearCuentos.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? NombreCompletos { get; set; }

    public string? Dni { get; set; }

    public string? NombreDeUsuario { get; set; }

    public string? Correo { get; set; }

    public string? Contraseña { get; set; }

    public bool PermisoInicial { get; set; }

    public bool PermisoPrimaria { get; set; }

    public bool PermisoSecundaria { get; set; }

    public ulong? Estado { get; set; }
}
