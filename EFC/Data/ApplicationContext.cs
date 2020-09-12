using System;
using System.Collections.Generic;
using System.Text;
using EFC.Data.Configurations;
using EFC.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFC.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Pedido> Pedidos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-T8KD2BL; Initial Catalog=efc; Integrated Security=SSPI;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());

            //pode ser usado para mapear todos os modelos de dados da aplicação que implementem o IEntitytypeConfiguration
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        }

    }
}
