using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MobileOperatorAppServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace MobileOperatorAppServerTest
{
    public abstract class BaseTest
    {
        protected Context Context { get; private set; }

        protected BaseTest()
        {
            var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();

            // Получите строку подключения к базе данных из конфигурации основного проекта
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            // Создайте DbContextOptionsBuilder для подключения к MySQL
            var optionsBuilder = new DbContextOptionsBuilder<Context>()
                .UseMySQL(connectionString);

            // Создайте экземпляр DbContext
            Context = new Context(optionsBuilder.Options);
        }
    }
}
