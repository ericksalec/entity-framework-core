using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFC.Data.Configurations;
using EFC.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFC.Data
{
    public class ApplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Data Source=DESKTOP-T8KD2BL;Initial Catalog=efc;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                 p => p.EnableRetryOnFailure(
                     maxRetryCount : 2,
                     maxRetryDelay: 
                     TimeSpan.FromSeconds(5),
                     errorNumbersToAdd: null).MigrationsHistoryTable("tabela_de_migracoes"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());

            //pode ser usado para mapear todos os modelos de dados da aplicação que implementem o IEntitytypeConfiguration
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

            MapearPropriedadesEsquecidas(modelBuilder);
        }

        //método para garantir que todas as entidades estão com suas propriedades definidas
        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach(var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties()
                                       .Where(p => p.ClrType == typeof(string));
                foreach(var property in properties)
                {
                    if(string.IsNullOrEmpty(property.GetColumnType())
                        && !property.GetMaxLength().HasValue)
                    {
                        //property.SetMaxLength(100);
                        property.SetColumnName("VARCHAR(100)");
                    }

                }
            }
        }

    }
}
