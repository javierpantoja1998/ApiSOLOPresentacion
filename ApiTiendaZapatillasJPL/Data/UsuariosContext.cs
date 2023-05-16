using Microsoft.EntityFrameworkCore;
using NuggetTiendaZapatillasJPL.Models;

namespace ApiTiendaZapatillasJPL.Data
{
    public class UsuariosContext : DbContext
    {
        public UsuariosContext(DbContextOptions<UsuariosContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
