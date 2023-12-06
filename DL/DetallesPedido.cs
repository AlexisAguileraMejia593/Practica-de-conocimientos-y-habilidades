using System;
using System.Collections.Generic;

namespace DL;

public partial class DetallesPedido
{
    public int DetallesPedidoId { get; set; }

    public int? PedidosId { get; set; }

    public int? MedicamentosId { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Total { get; set; }

    public virtual Medicamento? Medicamentos { get; set; }

    public virtual Pedido? Pedidos { get; set; }
}
