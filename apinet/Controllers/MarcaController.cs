using apinet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apinet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaController : ControllerBase
    {
        public readonly BddMiguelpContext _dbcontext;
        public MarcaController(BddMiguelpContext context)
        {
            _dbcontext = context;
        }
        [HttpGet]
        [Route("lista")]
        public IActionResult Lista()
        {
            try
            {
                List<Marcazapa> lista = _dbcontext.Marcazapas.ToList();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
        [HttpGet]
        [Route("Obtener/{idMarca:int}")]
        public IActionResult Obtener(int idMarca)
        {
            try
            {
                Marcazapa oMarca = _dbcontext.Marcazapas.FirstOrDefault(p => p.Id == idMarca);

                if (oMarca == null)
                {
                    return NotFound("Marca con id " + idMarca + " no encontrado");
                }

                return Ok(new { mensaje = "ok", response = oMarca });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
