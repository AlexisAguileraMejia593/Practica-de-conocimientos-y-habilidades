using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DetallesPedido
    {
        public static void Insert(ML.DetallesPedido detallePedido)
        {
            using (SqlConnection connection = new SqlConnection("Server=LAPTOP-CHR9HTDV; Database=Practica de Conocimientos y Habilidades; TrustServerCertificate=True; User ID=sa; Password=pass@word1;"))
            {
                string query = "INSERT INTO DetallesPedido (PedidosID, MedicamentosID, Cantidad, Total) VALUES (@PedidosID, @MedicamentosID, @Cantidad, @Total)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    detallePedido.Pedidos = new ML.Pedidos();
                    detallePedido.Medicamentos = new ML.Medicamentos();
                    command.Parameters.AddWithValue("@PedidosID", detallePedido.Pedidos.IdPedidos);
                    command.Parameters.AddWithValue("@MedicamentosID", detallePedido.Medicamentos.IdMedicamentos);
                    command.Parameters.AddWithValue("@Cantidad", detallePedido.Cantidad);
                    command.Parameters.AddWithValue("@Total", detallePedido.Total);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
