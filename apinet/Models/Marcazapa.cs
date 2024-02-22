using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace apinet.Models;

public partial class Marcazapa
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Modelozapatilla> Modelozapatillas { get; set; } = new List<Modelozapatilla>();
}
