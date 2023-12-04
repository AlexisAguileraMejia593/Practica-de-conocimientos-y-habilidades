using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usuarios
    {
       public int? IdUsuarios {  get; set; }
       public string? Nombre { get; set; }
       public string? ApellidoPaterno { get; set; }
       public string? ApellidoMaterno { get; set; }
       public string? Email { get; set; }
       public string? Contraseña { get; set; }
        public ML.Rol? Rol { get; set; }
       public List<ML.Usuarios>? usuarios { get; set; }
    }
}
