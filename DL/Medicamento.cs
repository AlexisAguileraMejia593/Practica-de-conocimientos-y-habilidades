using System;
using System.Collections.Generic;

namespace DL;

public partial class Medicamento
{
    public int MedicamentosId { get; set; }

    public string? Sku { get; set; }

    public string? Nombre { get; set; }

    public decimal? Precio { get; set; }

    public string? Imagen { get; set; }

    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();
}
