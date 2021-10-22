using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IProAgilRepository _context;

        public EventosController(IProAgilRepository context)
        {
            _context = context;
        }
        [HttpGet]

        public async Task<IActionResult> Get()
        {
              try
              {
                var _results = await _context.GetAllEventosAsync(false);
                return Ok(_results);
              }
              catch (System.Exception)
              {
                  return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco de Dados Falhou");
              }  
        }

        [HttpGet("EventoId")]

        public async Task<IActionResult> Get (int EventoId)
        {
            try
            {
                var _results = await _context.GetAllEventosasyncById(EventoId,false);
                return Ok(_results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco de Dados Falhou");
            }
        }

        [HttpGet("getByTema{tema}")]

        public async Task<IActionResult> Get (string tema) 
        {
            try
            {
                var results = await _context.GetAllEventosAsyncByTema(tema,true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha no Banco de Dados");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post (Evento model)
        {
            try
            {
                _context.Add(model);
                if(await _context.SaveChangesAsync())
                {
                    return Created($"/api/eventos/{model.Id}",model);        
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha no Banco de Dados");
            }
            return BadRequest();
        }
        
        [HttpPut]
        public async Task<IActionResult> Put (int EventoId, Evento model)
        {
            try
            {
                var evento = _context.GetAllEventosasyncById(EventoId,false);
                if (evento == null) return NotFound();
                _context.Update(model);
                if ( await _context.SaveChangesAsync())
                {
                       return Created($"/api/evento/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha no Banco de Dados");
            }
            return BadRequest();
        }

        [HttpDelete]

        public async Task<IActionResult> Delete (int EventoId)
        {
            try
            {
                var evento = await _context.GetAllEventosasyncById(EventoId,false);
                if(evento == null)
                {
                    return NotFound();
                }
                _context.Delete(evento);
                if(await _context.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falhou Banco de Dados");
            }
            return BadRequest();
        }
    }
}