using System;
using System.Collections.Generic;

namespace DL;

public partial class Pedido
{
    public int PedidosId { get; set; }

    public int? UsuariosId { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();

    public virtual Usuario? Usuarios { get; set; }
}
