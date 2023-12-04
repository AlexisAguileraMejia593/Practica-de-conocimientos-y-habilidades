using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Web_Api.Controllers
{
    [Route("api/Usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = BL.Usuarios.GetAll();
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return NotFound();
            }

        }
        [Route("GetById/{idUsuarios?}")]
        [HttpGet]
        public IActionResult GetById(int idUsuarios)
        {
            var list = BL.Usuarios.GetById(idUsuarios);
            if (list != null)
            {
                return Ok(list);

            }
            else
            {
                return NotFound();
            }
        }
        [Route("Add")]
        [HttpPost]
        public IActionResult Add([FromBody] ML.Usuarios usuarios)
        {
            var result = BL.Usuarios.Add(usuarios);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
        [Route("Delete/{idUsuarios?}")]
        [HttpDelete]
        public IActionResult Delete(int idUsuarios)
        {
            var result = BL.Usuarios.Delete(idUsuarios);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("Update/{idUsuarios?}")]
        [HttpPut]
        public IActionResult Update(int idUsuarios, [FromBody] ML.Usuarios usuarios)
        {
            usuarios.IdUsuarios = idUsuarios;
            var result = BL.Usuarios.Update(usuarios);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
