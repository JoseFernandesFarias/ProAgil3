using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestranteController : ControllerBase
    {
        private readonly IProAgilRepository _repo;

        public PalestranteController(IProAgilRepository repo)
        {
            _repo = repo;
        }
        
        [HttpGet("getPalestrante/{idPalestrante}")]
        public async Task<IActionResult> Get(int idPalestrante)
        {
            try
            {
                var _result = await _repo.GetAllPalestrantesAsync(idPalestrante,false);
                return Ok(_result);
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou"); 
            }
        }
    }
}