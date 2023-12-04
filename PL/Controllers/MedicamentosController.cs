using Microsoft.AspNetCore.Mvc;

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
