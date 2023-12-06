using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Pedidos
    {
        public static int Insert(ML.Pedidos pedido)
        {
            using (SqlConnection connection = new SqlConnection("Server=LAPTOP-CHR9HTDV; Database=Practica de Conocimientos y Habilidades; TrustServerCertificate=True; User ID=sa; Password=pass@word1;"))
            {
                string query = "INSERT INTO Pedidos (UsuariosID, Fecha) OUTPUT INSERTED.PedidosID VALUES (@UsuariosID, @Fecha)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    pedido.Usuarios = new ML.Usuarios();
                    command.Parameters.AddWithValue("@UsuariosID", pedido.Usuarios.IdUsuarios);
                    command.Parameters.AddWithValue("@Fecha", pedido.Fecha);
                    connection.Open();
                    int PedidosID = (int)command.ExecuteScalar();
                    return PedidosID;
                }
            }
        }
    }
}
