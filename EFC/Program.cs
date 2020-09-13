using EFC.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.ComponentModel;
using System.Linq;

namespace EFC
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new Data.ApplicationContext();

            //opção para migrar os scripts mapeados do BD, não indicado para ambiente de produção
            //db.Database.Migrate();

            //verificar se esxistem migrações pendentes na base de dados
            //var existe = db.Database.GetPendingMigrations().Any();

            //InserirDados();
            //InserirDadosEmMassa();
            ConsultarDados();
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            //var consultarPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
            var consultaPorMetodo = db.Clientes
                .Where(p => p.Id > 0).ToList()
                .OrderBy(p => p.Id)
                .ToList();

            foreach(var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Clientes: {cliente.Id}");
                //db.Clientes.Find(cliente.Id);
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "123456789666",
                Valor = 10m,
                Ativo = true,
            };

            var cliente = new Cliente
            {
                Nome = "Produto Teste",
                CEP = "33070060",
                Cidade = "Belo Horizonte",
                Estado = "MG",
                Telefone = "31999999999"
            };

            using var db = new Data.ApplicationContext();
            db.AddRange(produto, cliente);

            var registro = db.SaveChanges();
            Console.WriteLine($"Total Registros: {registro}");
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "123456789666",
                Valor = 10m,
                Ativo = true,
            };

            using var db = new Data.ApplicationContext();
            //db.Produto.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registros(s): {registros}");

        }

    }
}
