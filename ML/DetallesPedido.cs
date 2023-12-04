using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class DetallesPedido
    {
        public int IdDetallesPedido {get; set;}
        public ML.Pedidos? Pedidos { get; set;}
        public ML.Medicamentos? Medicamentos { get; set;}
        public int Cantidad { get; set;}
        public Decimal? Total { get; set;}
    }
}
