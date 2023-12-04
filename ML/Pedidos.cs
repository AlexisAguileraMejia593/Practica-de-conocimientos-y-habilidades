using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Pedidos
    {
        public int IdPedidos { get; set; }
        public ML.Usuarios? Usuarios { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
