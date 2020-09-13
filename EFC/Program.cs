using EFC.Domain;
using EFC.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
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

            //CRUD com EFC

            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPeido();
            //ConsultarPedidoCarregamentoAdiantado();
            //AtualizarDados();
            //RemoverRegistro();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();
            //var cliente = db.Clientes.Find(2);
            var cliente = new Cliente { Id = 3 }; //forma desconectada, somente uma interação com o BD (não realiza a busca)

            //db.Clientes.Remove(cliente);
            //db.Entry(cliente).State = EntityState.Deleted;
            db.Remove(cliente);

            db.SaveChanges();
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            //var cliente = db.Clientes.Find(1);

            var cliente = new Cliente
            {
                Id = 1,
            };

            var clienteDesconectado = new
            {
                Nome = "Cliente Desconectado",
                Telefone = "317777777"
            };

            db.Attach(cliente);
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            //atualiza todos os dados idempendente se sofreram alteração
            //db.Clientes.Update(cliente);

            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = db
                .Pedidos
                .Include(p => p.Itens)//inclui o carregamento dos itens
                .ThenInclude(p => p.Produto)//inclui o carregamento dos produtos presentes no item 
                .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void CadastrarPeido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now.ToString(),
                FinalizadoEm = DateTime.Now.ToString(),
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10,
                    }
                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();

        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            //var consultarPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
            var consultaPorMetodo = db.Clientes
                .Where(p => p.Id > 0).ToList()
                .OrderBy(p => p.Id)
                .ToList();

            foreach (var cliente in consultaPorMetodo)
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
