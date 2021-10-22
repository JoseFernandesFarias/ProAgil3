using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {

     private readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
            this._context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }


        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

         public async Task<bool> SaveChangesAsync()
        {
            return ( await _context.SaveChangesAsync()) > 0;
        }

        //EVENTOS

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                    .Include(l => l.Lotes)
                    .Include(r => r.RedesSocials);
        
            if(includePalestrantes)
            {
                query = query.Include(pe => pe.PalestranteEventos)
                        .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(d => d.DataEvento);

            return await query.ToArrayAsync();

        }

        public async Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                    .Include(l => l.Lotes)
                    .Include(r => r.RedesSocials);
        
            if(includePalestrantes)
            {
                query = query.Include(pe => pe.PalestranteEventos)
                        .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(d => d.DataEvento)
                    .Where(t => t.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetAllEventosasyncById(int EventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                    .Include(l => l.Lotes)
                    .Include(r => r.RedesSocials);
        
            if(includePalestrantes)
            {
                query = query.Include(pe => pe.PalestranteEventos)
                        .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(d => d.DataEvento)
                    .Where(c => c.Id == EventoId);

            return await query.FirstOrDefaultAsync();
        }



        //PALESTRANTES
        public async Task<Palestrante> GetAllPalestrantesAsync(int PalestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                    .Include(r => r.RedesSociais);
        
            if(includeEventos)
            {
                query = query.Include(pe => pe.PalestranteEventos)
                        .ThenInclude(e => e.Evento);
            }

            query = query.AsNoTracking().OrderBy(n => n.Nome)
                    .Where(i => i.Id == PalestranteId);
                    

            return await query.FirstOrDefaultAsync();          
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                    .Include(r => r.RedesSociais);
        
            if(includeEventos)
            {
                query = query.Include(pe => pe.PalestranteEventos)
                        .ThenInclude(e => e.Evento);
            }

            query = query.AsNoTracking().Where(n => n.Nome.ToLower().Contains(name.ToLower()));
                    

            return await query.ToArrayAsync();                 
            
        }


    }
}