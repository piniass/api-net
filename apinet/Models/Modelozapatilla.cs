using System;
using System.Collections.Generic;

namespace apinet.Models;

public partial class Modelozapatilla
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Color { get; set; } = null!;

    public decimal Precio { get; set; }

    public string? Imagen { get; set; }

    public byte[]? ImagenBlob { get; set; }

    public int? IdMarca { get; set; }

    public virtual Marcazapa? oMarcaZapa { get; set; }
}
