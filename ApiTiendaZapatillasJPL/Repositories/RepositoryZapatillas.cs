using ApiTiendaZapatillasJPL.Data;
using Microsoft.EntityFrameworkCore;
using NuggetTiendaZapatillasJPL.Models;

namespace ApiTiendaZapatillasJPL.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }

        //FUNCION PARA SACAR TODAS LAS ZAPATILLAS
        public async Task<List<Zapatilla>> GetZapatillassAsync()
        {
            return await this.context.Zapatillas.ToListAsync();

        }

        //FUNCION PARA LOS DETALLES
        public async Task<Zapatilla> FindZapatillaAsync(int id)
        {
            return await this.context.Zapatillas
            .FirstOrDefaultAsync(x => x.IdZapatilla == id);
        }

        //FUNCION PARA SACAR ZAPATILLAS Y CATEGORIA
        public async Task <List<VistaZapatillasCategoria>> zapatillasCategoria(string nombreCategoria)
        {
            return await this.context.ZapatillasCategoria
            .Where(x => x.NombreCategoria == nombreCategoria).ToListAsync();
        }

    }
}
