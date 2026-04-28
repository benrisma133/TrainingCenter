using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TrainingCenter.Repository.Data
{
    public static class DbContextFactory
    {
        public static AppDbContext Create()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config
                .GetConnectionString("TrainingCenterDB");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new AppDbContext(options);
        }
    }
}