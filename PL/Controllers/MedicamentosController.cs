using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ML;
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
                    carrito.Carritos.Add(medicamentos);
                    //serializar carrito
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carritos));
                }

            }
            else
            {

                ML.Medicamentos medicamentos = result;
                GetCarrito(carrito); //ya recupere el carrito
                foreach (ML.Medicamentos medicamentos1 in carrito.Carritos)
                {
                    if (medicamentos.IdMedicamentos == medicamentos1.IdMedicamentos)
                    {
                        medicamentos.Cantidad += 1;
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
                return View(carrito);
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
