using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public IActionResult AddCarrito(int IdMedicamento)
        {
            List<ML.DetallesPedido> listaDetallesPedidos;
            var result = BL.Medicamentos.GetById(IdMedicamento);
            if (HttpContext.Session.GetString("Carrito") == null)
            {
                listaDetallesPedidos = new List<ML.DetallesPedido>();
                if (result != null)
                {
                    ML.DetallesPedido detallesPedido = new ML.DetallesPedido();
                    detallesPedido.Medicamentos = new ML.Medicamentos();
                    detallesPedido.Medicamentos = result;
                    detallesPedido.Cantidad = 1;
                    listaDetallesPedidos.Add(detallesPedido);
                }
            }
            else
            {
                // Deserializar la lista del carrito de la sesión
                listaDetallesPedidos = JsonConvert.DeserializeObject<List<ML.DetallesPedido>>(HttpContext.Session.GetString("Carrito"));
                bool existe = false;
                foreach (var detallesPedido in listaDetallesPedidos)
                {
                    if (detallesPedido.Medicamentos.IdMedicamentos == result.IdMedicamentos)
                    {
                        detallesPedido.Cantidad += 1;
                        existe = true;
                        break;
                    }
                }
                if (!existe)
                {
                    ML.DetallesPedido detallesPedido = new ML.DetallesPedido();
                    detallesPedido.Medicamentos = new ML.Medicamentos();
                    detallesPedido.Medicamentos = result;
                    detallesPedido.Cantidad = 1;
                    listaDetallesPedidos.Add(detallesPedido);
                }
            }
            // Serializar la lista del carrito a la sesión
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var json = JsonConvert.SerializeObject(listaDetallesPedidos, settings);
            HttpContext.Session.SetString("Carrito", json);
            return RedirectToAction("Index");
        }
        public ML.DetallesPedido GetCarrito(ML.DetallesPedido detallesPedido)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));

            foreach (var obj in ventaSession)
            {
                string json = HttpContext.Session.GetString("Carrito");
                List<ML.DetallesPedido> detallesPedidos = JsonConvert.DeserializeObject<List<ML.DetallesPedido>>(json);
                detallesPedido.DetallesPedidos.Add(detallesPedido);
            }
            return detallesPedido;
        }
        public IActionResult Carrito()
        {
            ML.DetallesPedido detallesPedido = new ML.DetallesPedido();
            detallesPedido.DetallesPedidos = new List<ML.DetallesPedido>();
            if (HttpContext.Session.GetString("Carrito") == null)
            {
                return View(detallesPedido);
            }
            else
            {
                GetCarrito(detallesPedido);
                return View(detallesPedido);
            }

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
    }
}
