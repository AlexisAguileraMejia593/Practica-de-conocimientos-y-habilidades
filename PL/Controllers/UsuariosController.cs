using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace PL.Controllers
{
    public class UsuariosController : Controller
    {
        /*       public IActionResult Index()
               {
                   ML.Usuarios usuarios = BL.Usuarios.GetAll();
                   var result = usuarios.usuarios;
                   return View(usuarios);
               }
        */
        [HttpGet]
        public IActionResult Index()
        {
            ML.Usuarios usuarios = new ML.Usuarios();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5143/api/");
                var responseTask = client.GetAsync("Usuarios/GetAll");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Usuarios>();
                    readTask.Wait();
                    usuarios = readTask.Result;
                }
            }
            return View(usuarios);
        }
        /*  [HttpGet]
          public IActionResult Form(int? IdUsuarios) 
          {
              ML.Usuarios usuarios = new ML.Usuarios();
              if(IdUsuarios != null)
              {
                  var list = BL.Usuarios.GetById(IdUsuarios.Value);
                  if(list != null) 
                  {
                      //Unboxing
                      usuarios = list;
                  }
              }
              else
              {

              }
              return View(usuarios);
          }*/
        [HttpGet]
        public IActionResult Form(int? IdUsuarios)
        {
            ML.Usuarios usuarios = new ML.Usuarios();
            if (IdUsuarios != null)
            {
                using (var cliente = new HttpClient())
                {
                    cliente.BaseAddress = new Uri("http://localhost:5143/api/");
                    var responseTask = cliente.GetAsync("Usuarios/GetById/" + IdUsuarios);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ML.Usuarios>();
                        readTask.Wait();
                        usuarios = readTask.Result;
                    }

                }
            }
            return View(usuarios);
        }
        /*    [HttpPost]
            public IActionResult Form(ML.Usuarios usuarios)
            {
                if(ModelState.IsValid)
                {
                    if(usuarios.IdUsuarios == 0)
                    {
                        var result = BL.Usuarios.Add(usuarios);
                        if(result != null)
                        {
                            ViewBag.Mensaje = "Se ha ingresado correctamente el Usuario";
                        }
                        else
                        {
                            ViewBag.Mensaje = "NO se ha ingresado correctamente el Usuario";
                        }
                    }
                    else
                    {
                        var result = BL.Usuarios.Update(usuarios);
                        if(result != null)
                        {
                            ViewBag.Mensaje = "Se ha actualizado correctamente el Usuario";
                        }
                        else
                        {
                            ViewBag.Mensaje = "NO se ha actualizado correctamente el Usuario";
                        }
                    }
                }
                else
                {

                }
                return PartialView("Modal");
            }
        */
        public IActionResult Form(ML.Usuarios usuarios)
        {
            if (usuarios.IdUsuarios == 0) //ADD
            {
                using (var cliente = new HttpClient())
                {
                    cliente.BaseAddress = new Uri("http://localhost:5143/api/");

                    var postTask = cliente.PostAsJsonAsync("Usuarios/Add", usuarios);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se agrego correctamente el Usuario";
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se pudo agregar el Usuario";
                    }
                }
            }
            else //UPDATE
            {
                using (var cliente = new HttpClient())
                {
                    int IdUsuarios = usuarios.IdUsuarios.Value;

                    cliente.BaseAddress = new Uri("http://localhost:5143/api/");

                    var putTask = cliente.PutAsJsonAsync("Emisor/Update/" + IdUsuarios, usuarios);
                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se actualizo correctamente el Usuario";
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se logro actualizar el Usuario";
                    }
                }
            }
            return PartialView("Modal");
        }
        public IActionResult Delete(int IdUsuarios)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5143/api/");

                var deleteTask = client.DeleteAsync("Usuarios/Delete/" + IdUsuarios);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View("Error");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Contraseña)
        {
            var result = BL.Usuarios.GetByEmail(Email);

            if (result != null)
            {
                ML.Usuarios usuario = result;
                string hashedPassword = EncryptPassword(Contraseña);
                if (hashedPassword == usuario.Contraseña)
                {
                    usuario.Rol.Tipo = HttpContext.Session.GetString("Role");
                    usuario.IdUsuarios = HttpContext.Session.GetInt32("Usuarios");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Mensaje = "Contraseña Incorrecta";
                    ViewBag.Correo = false;
                    return RedirectToAction("Login", "Usuario");
                }
            }
            else
            {
                ViewBag.Mensaje = "No existe la cuenta";
                ViewBag.Correo = false;
                return PartialView("Modal");
            }
        }
        public static string EncryptPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
