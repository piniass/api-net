using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apinet.Models;

namespace apinet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZapatillaController : ControllerBase
    {
        public readonly BddMiguelpContext _dbcontext;

        public ZapatillaController(BddMiguelpContext context)
        {
            _dbcontext = context;
        }

        [HttpGet]
        [Route("lista")]
        public IActionResult Lista()
        {
            try
            {
                List<Modelozapatilla> lista = _dbcontext.Modelozapatillas.Include(c => c.oMarcaZapa).ToList();
                return Ok(lista);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje=ex.Message});
            }
        }

        [HttpGet]
        [Route("Obtener/{idZapatilla:int}")]
        public IActionResult Obtener(int idZapatilla)
        {
            try
            {
                Modelozapatilla oZapatilla = _dbcontext.Modelozapatillas.Include(c => c.oMarcaZapa).FirstOrDefault(p => p.Id == idZapatilla);

                if (oZapatilla == null)
                {
                    return NotFound("Zapatilla con id " + idZapatilla + " no encontrado");
                }

                return Ok(new { mensaje = "ok", response = oZapatilla });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ListarPrecios")]
        public IActionResult ListaPrecio()
        {
            try
            {
                // Obtén la lista de zapatillas incluyendo la relación con la marca
                List<Modelozapatilla> lista = _dbcontext.Modelozapatillas.Include(c => c.oMarcaZapa).ToList();

                // Ordena la lista por el campo de precio
                lista = lista.OrderBy(zapatilla => zapatilla.Precio).ToList();

                // Devuelve la lista ordenada
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpPost]
        [Route("Guardar")]
        [Consumes("multipart/form-data")]
        public IActionResult Guardar([FromForm] ZapatillaFormData formData)
        {
            try
            {
                Modelozapatilla zapatilla = new Modelozapatilla
                {
                    Nombre = formData.Nombre,
                    Color = formData.Color,
                    Precio = formData.Precio,
                    IdMarca = formData.IdMarca,
                    Imagen = formData.ImagenBlob.FileName,
                };

                if (formData.ImagenBlob != null && formData.ImagenBlob.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        formData.ImagenBlob.CopyTo(ms);
                        byte[] imageBytes = ms.ToArray();
                        zapatilla.ImagenBlob = imageBytes;
                    }
                }

                _dbcontext.Modelozapatillas.Add(zapatilla);
                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "ok", Response = zapatilla });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        public class ZapatillaFormData
        {
            public string Nombre { get; set; }

            public string Color { get; set; }

            public decimal Precio { get; set; }
            public IFormFile ImagenBlob { get; set; }
            public int? IdMarca { get; set; }
        }
        [HttpPut]
        [Route("Editar/{idZapatilla:int}")]
        [Consumes("multipart/form-data")]
        public IActionResult Editar([FromForm] ZapatillaFormData formData, int idZapatilla)
        {
            try
            {
                // Busca la zapatilla por su ID en la base de datos
                Modelozapatilla zapatilla = _dbcontext.Modelozapatillas.Find(idZapatilla);

                // Verifica si la zapatilla existe
                if (zapatilla == null)
                {
                    return BadRequest("Zapatilla no encontrada");
                }

                // Actualiza los campos de la zapatilla con los datos del formulario
                zapatilla.Nombre = formData.Nombre;
                zapatilla.Color = formData.Color;
                zapatilla.Precio = formData.Precio;
                zapatilla.IdMarca = formData.IdMarca;

                // Verifica si se ha proporcionado una nueva imagen
                if (formData.ImagenBlob != null && formData.ImagenBlob.Length > 0)
                {
                    // Lee la imagen en un array de bytes
                    using (MemoryStream ms = new MemoryStream())
                    {
                        formData.ImagenBlob.CopyTo(ms);
                        byte[] imageBytes = ms.ToArray();
                        zapatilla.ImagenBlob = imageBytes;
                    }
                }

                // Actualiza la zapatilla en la base de datos
                _dbcontext.Modelozapatillas.Update(zapatilla);
                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "ok", Response = zapatilla });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al actualizar la zapatilla", error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idZapatilla:int}")]
        public IActionResult Eliminar(int idZapatilla)
        {
            try
            {
                Modelozapatilla oZapatilla = _dbcontext.Modelozapatillas.Find(idZapatilla);

                if (oZapatilla == null)
                {
                    return NotFound("Zapatilla con id " + idZapatilla + " no encontrado");
                }

                _dbcontext.Modelozapatillas.Remove(oZapatilla);
                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
        [HttpGet]
        [Route("ObtenerImagen/{idZapatilla:int}")]
        public IActionResult ObtenerImagen(int idZapatilla)
        {
            try
            {
                // Busca la zapatilla por su ID en la base de datos
                Modelozapatilla zapatilla = _dbcontext.Modelozapatillas.Find(idZapatilla);

                // Verifica si se encontró la zapatilla
                if (zapatilla == null || zapatilla.ImagenBlob == null)
                {
                    // Si la zapatilla no se encuentra o no tiene una imagen, devuelve un error o una imagen predeterminada
                    return NotFound("Imagen no encontrada");
                }

                // Devuelve la imagen como un FileContentResult con el tipo MIME adecuado
                return new FileContentResult(zapatilla.ImagenBlob, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

    }

}
