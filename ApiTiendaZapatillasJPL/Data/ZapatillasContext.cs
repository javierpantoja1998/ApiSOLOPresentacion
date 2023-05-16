using Microsoft.EntityFrameworkCore;
using NuggetTiendaZapatillasJPL.Models;

namespace ApiTiendaZapatillasJPL.Data
{
    public class ZapatillasContext : DbContext
    {
        public ZapatillasContext(DbContextOptions<ZapatillasContext> options)
           : base(options) { }

        public DbSet<Zapatilla> Zapatillas { get; set; }
        public DbSet<VistaZapatillasCategoria> ZapatillasCategoria { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Compra> Compras { get; set; }
    }
}
