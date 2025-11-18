using Crazy_zoo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Crazy_zoo
{
    public class CrazyZooContextFactory : IDesignTimeDbContextFactory<CrazyZooContext>
    {
        public CrazyZooContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CrazyZooContext>();

            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CrazyZoo;Trusted_Connection=True;");

            return new CrazyZooContext(optionsBuilder.Options);
        }
    }
}
