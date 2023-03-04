using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using employmently_be.DbContexts;

namespace employmently_be_test
{
    public class TestDbContext : dbContext
    {
        public TestDbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
        }
    }

}
