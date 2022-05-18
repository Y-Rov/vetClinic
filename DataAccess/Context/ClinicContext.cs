using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class ClinicContext : DbContext
    {
        public ClinicContext() { }
        public ClinicContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ᓚᘏᗢ For fluent API configurations
        }
    }
}
