using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WebAPIAutores1
{
    public class Blogging1ContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Data Source=DESARROLLO09\\SQL2016;Initial Catalog=CursoWebApis;Integrated Security=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
