using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ML;
using Newtonsoft.Json;
using iText.Layout;

namespace PL.Controllers
{
    public class MedicamentosController : Controller
    {
        public IActionResult Index()
        {
            ML.Medicamentos medicamentos = BL.Medicamentos.GetAll();
            var result = medicamentos.medicamentos;
            return View(medicamentos);
        }
        [HttpGet]
        public async Task<IActionResult> Form(int? IdMedicamentos, IFormFile Image)
        {
            ML.Medicamentos medicamentos = new ML.Medicamentos();
            if (IdMedicamentos != null)
            {
                var list = BL.Medicamentos.GetById(IdMedicamentos.Value);
                if (list != null)
                {
                    //Unboxing
                    medicamentos = list;
                }
                if (Image != null && Image.Length > 0)
                {
                    // Guardar la imagen en el servidor
                    var filePath = Path.Combine("~/Imagen/", Image.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    // Convertir la imagen a base64
                    byte[] imageData;
                    using (var stream = new MemoryStream())
                    {
                        await Image.CopyToAsync(stream);
                        imageData = stream.ToArray();
                    }
                    medicamentos.Imagen = Convert.ToBase64String(imageData);
                }
            }
            return View(medicamentos);
        }
        [HttpPost]
        public async Task<IActionResult> FormAsync(ML.Medicamentos medicamentos, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
                if (Imagen != null && Imagen.Length > 0)
                {
                    var fileName = Path.GetFileName(Imagen.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(stream);
                    }

                    medicamentos.Imagen = ConvertirABase64(Imagen); // Guarda la ruta de la imagen en la base de datos
                }
                if (medicamentos.IdMedicamentos == 0)
                {
                    var result = BL.Medicamentos.Add(medicamentos);
                    if (result != null)
                    {
                        ViewBag.Mensaje = "Se ha ingresado correctamente el Medicamento";
                    }
                    else
                    {
                        ViewBag.Mensaje = "NO se ha ingresado correctamente el Medicamento";
                    }
                }
                else
                {
                    var result = BL.Medicamentos.Update(medicamentos);
                    if (result != null)
                    {
                        ViewBag.Mensaje = "Se ha actualizado correctamente el Medicamento";
                    }
                    else
                    {
                        ViewBag.Mensaje = "NO se ha actualizado correctamente el Medicamento";
                    }
                }
            }
            else
            {

            }
            return PartialView("Modal");
        }
        public IActionResult Delete(int IdMedicamentos)
        {
            var result = BL.Medicamentos.Delete(IdMedicamentos);
            if (result)
            {
                ViewBag.Mensaje = "Se elimino correctamente el Medicamento";
            }
            else
            {
                ViewBag.Mensaje = "No se Elimino correctamente el Medicamento";
            }
            return PartialView("Modal");
        }
        public IActionResult AddCarrito(int IdMedicamentos)
        {
            bool existe = false;
            ML.Carrito carrito = new ML.Carrito();
            carrito.Carritos = new List<object>();
            var result = BL.Medicamentos.GetById(IdMedicamentos);
            if (HttpContext.Session.GetString("Carrito") == null)
            {
                if (result != null)
                {
                    ML.Medicamentos medicamentos = result;
                    medicamentos.Cantidad = 1;
                    medicamentos.PrecioTotal = medicamentos.Precio; 
                    carrito.Carritos.Add(medicamentos);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carritos));
                }
            }
            else
            {
                ML.Medicamentos medicamentos = result;
                GetCarrito(carrito);
                foreach (ML.Medicamentos medicamentos1 in carrito.Carritos)
                {
                    if (medicamentos.IdMedicamentos == medicamentos1.IdMedicamentos)
                    {
                        medicamentos1.Cantidad++;
                        medicamentos1.PrecioTotal += medicamentos1.Precio; 
                        existe = true;
                        break;
                    }
                    else
                    {
                        existe = false;
                    }
                }
                if (existe == true)
                {
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carritos));
                }
                else
                {
                    medicamentos.Cantidad = 1;
                    medicamentos.PrecioTotal = medicamentos.Precio;
                    carrito.Carritos.Add(medicamentos);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carritos));
                }
            }
            return RedirectToAction("Index");
        }
        public ML.Carrito GetCarrito(ML.Carrito carrito)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));

            foreach (var obj in ventaSession)
            {
                ML.Medicamentos objMateria = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamentos>(obj.ToString());
                carrito.Carritos.Add(objMateria);
            }
            return carrito;
        }
        public IActionResult Carrito()
        {
            ML.Carrito carrito = new ML.Carrito();
            carrito.Carritos = new List<object>();
            if (HttpContext.Session.GetString("Carrito") == null)
            {
                return View(carrito);
            }
            else
            {
                GetCarrito(carrito);
                carrito.Total = 0;
                foreach(ML.Medicamentos medicamentos in carrito.Carritos)
                {
                    carrito.Total += medicamentos.PrecioTotal;
                }
                return View(carrito);
            }

        }
        [HttpPost]
        public IActionResult Agregar(int id)
        {
            var carrito = HttpContext.Session.GetString("Carrito");
            if (carrito != null)
            {
                var carritoDeserializado = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ML.Medicamentos>>(carrito);
                var medicamento = carritoDeserializado.FirstOrDefault(m => m.IdMedicamentos == id);
                if (medicamento != null)
                {
                    medicamento.Cantidad++;
                    medicamento.PrecioTotal += medicamento.Precio;
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carritoDeserializado));
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Quitar(int id)
        {
            var carrito = HttpContext.Session.GetString("Carrito");
            if (carrito != null)
            {
                var carritoDeserializado = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ML.Medicamentos>>(carrito);
                var medicamento = carritoDeserializado.FirstOrDefault(m => m.IdMedicamentos == id);
                if (medicamento != null)
                {
                    medicamento.Cantidad--;
                    medicamento.PrecioTotal -= medicamento.Precio;
                    if (medicamento.Cantidad == 0)
                    {
                        carritoDeserializado.Remove(medicamento);
                    }
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carritoDeserializado));
                }
            }
            return RedirectToAction("Index");
        }
        public string ConvertirABase64(IFormFile Foto)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Foto.CopyTo(ms);
                byte[] data = ms.ToArray();
                string imagen = Convert.ToBase64String(data);
                return imagen;
            }
        }
        public ActionResult GenerarPDF()
        {
            ML.Carrito carrito = new ML.Carrito();
            carrito.Carritos = new List<object>();
            GetCarrito(carrito);

            // Crear un nuevo documento PDF en una ubicación temporal
            string rutaTempPDF = Path.GetTempFileName() + ".pdf";

            using (PdfDocument pdfDocument = new PdfDocument(new PdfWriter(rutaTempPDF)))
            {
                using (iText.Layout.Document document = new iText.Layout.Document(pdfDocument))
                {
                    document.Add(new Paragraph("Resumen de Compra"));

                    // Crear la tabla para mostrar la lista de objetos
                    iText.Layout.Element.Table table = new iText.Layout.Element.Table(5); // 5 columnas
                    table.SetWidth(UnitValue.CreatePercentValue(100)); // Ancho de la tabla al 100% del documento

                    // Añadir las celdas de encabezado a la tabla
                    table.AddHeaderCell("ID Producto");
                    table.AddHeaderCell("Producto");
                    table.AddHeaderCell("Precio Unitario");
                    table.AddHeaderCell("Cantidad");
                    table.AddHeaderCell("Imagen");


                    foreach (ML.Medicamentos medicamentos in carrito.Carritos)
                    {
                        table.AddCell(medicamentos.IdMedicamentos.ToString());
                        table.AddCell(medicamentos.Nombre);
                        table.AddCell(medicamentos.Precio.ToString());
                        table.AddCell(medicamentos.Cantidad.ToString());
                        byte[] imageBytes = Convert.FromBase64String(medicamentos.Imagen);
                        ImageData data = ImageDataFactory.Create(imageBytes);
                        Image image = new Image(data);
                        table.AddCell(image.SetWidth(50).SetHeight(50));

                    }

                    // Añadir la tabla al documento
                    document.Add(table);
                }
            }

            // Leer el archivo PDF como un arreglo de bytes
            byte[] fileBytes = System.IO.File.ReadAllBytes(rutaTempPDF);

            // Eliminar el archivo temporal
            System.IO.File.Delete(rutaTempPDF);
            HttpContext.Session.Remove("Carrito");

            // Descargar el archivo PDF
            return new FileStreamResult(new MemoryStream(fileBytes), "application/pdf")
            {
                FileDownloadName = "ReporteProductos.pdf"
            };
        }
        public IActionResult GuardarPedido()
        {
            // Recupera el ID del usuario de la sesión
            var UsuariosID = HttpContext.Session.GetInt32("Usuarios");
            ML.DetallesPedido detallespedido = new ML.DetallesPedido();
            ML.Pedidos pedidos = new ML.Pedidos();
            if (UsuariosID == null)
            {
                // Maneja el caso en que no hay un usuario en la sesión
                return RedirectToAction("Login", "Usuario");
            }

            // Recupera el carrito de la sesión
            var carrito = HttpContext.Session.GetString("Carrito");
            if (carrito != null)
            {
                var carritoDeserializado = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ML.Medicamentos>>(carrito);

                // Inserta un nuevo pedido en la tabla Pedidos
                var PedidosID = BL.Pedidos.Insert(new ML.Pedidos { IdPedidos = UsuariosID.Value, Fecha = DateTime.Now });

                // Inserta los detalles del pedido en la tabla DetallesPedido
                foreach (var medicamento in carritoDeserializado)
                {
                    BL.DetallesPedido.Insert(new ML.DetallesPedido { IdDetallesPedido = detallespedido.IdDetallesPedido, Medicamentos = detallespedido.Medicamentos, Cantidad = medicamento.Cantidad, Total = medicamento.PrecioTotal.Value });
                }

                // Limpia el carrito
                HttpContext.Session.Remove("Carrito");
            }
            return RedirectToAction("Index");
        }
    }
}
