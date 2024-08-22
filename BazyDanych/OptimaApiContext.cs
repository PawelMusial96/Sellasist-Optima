using Microsoft.EntityFrameworkCore;

namespace Sellasist_Optima.BazyDanych
{
    public class OptimaApiContext : DbContext
    {

        public OptimaApiContext(DbContextOptions<OptimaApiContext> options)
        : base(options)
        {
        }
    }
}
