using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;

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

            Console.WriteLine("Hello World!");
        }
    }
}
